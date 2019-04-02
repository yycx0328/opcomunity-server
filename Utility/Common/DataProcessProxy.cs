using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;

namespace Utility
{
    /// <summary>
    /// DataProcessProxy
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DataProcessProxy<T> : IDisposable
    {        
        /// <summary>
        /// QueueMaxLength
        /// </summary>
        public int QueueMaxLength = 1000;
        /// <summary>
        /// ThreadSleepTime
        /// </summary>
        public int ThreadSleepTime = 100;

        private Queue<T> DataQueue;
        private Thread WorkThread;
        private Action<T> DataProcess;
        private bool IsRunning;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataProcess"></param>
        public DataProcessProxy(Action<T> dataProcess)
        {
            IsRunning = false;
            DataQueue = new Queue<T>();
            DataProcess = dataProcess;
        }

        /// <summary>
        /// Process the data through a proxy handler
        /// </summary>
        /// <param name="data"></param>
        public void Process(T data)
        {
            int count = DataQueue.Count;
            if (count > QueueMaxLength)
            {
                T temp = data;
                lock (DataQueue)
                {
                    DataQueue.Enqueue(data);
                    temp = DataQueue.Dequeue();
                }
                DataProcess(temp);
            }
            else
            {
                lock (DataQueue)
                {
                    DataQueue.Enqueue(data);
                }
                this.Start();
            }            
        }

        /// <summary>
        /// Start the processor
        /// </summary>
        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                WorkThread = new Thread(WorkThreadStart);
                WorkThread.Priority = ThreadPriority.BelowNormal;
                WorkThread.Start();
            }
        }

        /// <summary>
        /// Stop the processor
        /// </summary>
        public void Stop()
        {
            Dispose();
        }

        /// <summary>
        /// Work thread
        /// </summary>
        private void WorkThreadStart()
        {
            int count = DataQueue.Count;

            while (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        T data = DataQueue.Dequeue();
                        DataProcess(data);
                    }
                    catch
                    {
                    }
                    Thread.Sleep(0);
                }
                count = DataQueue.Count;
            }            
            IsRunning = false;
        }

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
            IsRunning = false;
            Thread.Sleep(100);

            int count = DataQueue.Count;
            Logger.Instance.BaseLogger.WriteEntry(string.Format("{0} objects skipped by dispose DataProcessProxy.", count));

            lock (DataQueue)
            {
                DataQueue.Clear();
            }
        }        
    }
}
