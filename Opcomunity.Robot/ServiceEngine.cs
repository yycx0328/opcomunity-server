using log4net;
using Opcomunity.Services.Tasks;
using Quartz.Impl;
using Quartz;
using System.Runtime.Remoting;
using System.Reflection;
using System;

namespace Opcomunity.Robot
{
    public class ServiceEngine
    {
        private static ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private IScheduler _scheduler = null;

        public async void Start()
        {
            try
            {
                TaskAdapterConfiguration setting = TaskAdapterConfiguration.GetConfig("taskSetting");
                TaskAdapterConfigurationStateCollection tasks = setting.TaskAdapters;
                
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _scheduler = await schedFact.GetScheduler();
                foreach (TaskAdapterConfigurationState state in tasks)
                {
                    try
                    {
                        ObjectHandle handle = Activator.CreateInstance(state.AssemblyName, state.TypeName);
                        IJob evt = (IJob)handle.Unwrap();

                        logger.Info(">>>" + state.TaskName + "正在启动");
                        string exp = state.ScheduleExpression;
                        if (string.IsNullOrEmpty(exp))
                        {
                            logger.Error("没有配置定时器");
                        }
                        else
                        {
                            logger.Info("开始配置计划任务");

                            IJobDetail job = JobBuilder.Create().OfType(evt.GetType())
                                .WithIdentity("job_" + state.TaskName, "group_" + state.TaskName)
                                .Build();
                            ITrigger trigger = TriggerBuilder.Create()
                                .WithIdentity("trigger_" + state.TaskName, "triggerGroup_" + state.TaskName)
                                .StartNow().WithCronSchedule(exp)
                                .Build();
                            DateTimeOffset ft = DateBuilder.EvenSecondDateAfterNow();

                            await _scheduler.ScheduleJob(job, trigger);

                            logger.Info(string.Format("任务 {0} 已经被预订 {1} 执行，并且按照下列表达式重复执行: {2}", job.GetType().ToString(), ft.ToLocalTime().ToString("r"), trigger));

                        }
                        logger.Info(">>>" + state.TaskName + "启动完成");
                    }
                    catch (Exception ex)
                    {
                        logger.Info(">>>" + state.TaskName + "启动时发生异常");
                        logger.Error(ex.Message + "|" + ex.StackTrace);
                    }

                }
                await _scheduler.Start();
            }
            catch (Exception e)
            {
                logger.Error(e.Message + "|" + e.StackTrace);
            }

        }

        public void Stop()
        {
            if (this._scheduler != null)
            {
                logger.Info(">>>正在停止所有计划任务");
                this._scheduler.Shutdown(true);
                logger.Info(">>>停止所有计划任务完成");
            }
        }
    }
}
