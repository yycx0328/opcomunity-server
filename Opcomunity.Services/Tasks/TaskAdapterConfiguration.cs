using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Tasks
{
    public class TaskAdapterConfiguration : ConfigurationSection
    {

        public static TaskAdapterConfiguration GetConfig(string section)
        {
            return ConfigurationManager.GetSection(section) as TaskAdapterConfiguration;
        }

        [ConfigurationProperty("taskAdapters")]
        public TaskAdapterConfigurationStateCollection TaskAdapters
        {
            get { return this["taskAdapters"] as TaskAdapterConfigurationStateCollection; }
        }
    }

    public class TaskAdapterConfigurationState : ConfigurationElement
    {

        [ConfigurationProperty("taskName", IsRequired = true)]
        public string TaskName
        {
            get { return this["taskName"] as string; }
        }

        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get { return this["assemblyName"] as string; }
        }

        [ConfigurationProperty("typeName", IsRequired = true)]
        public string TypeName
        {
            get { return this["typeName"] as string; }
        }

        [ConfigurationProperty("scheduleExpression", IsRequired = true)]
        public string ScheduleExpression
        {
            get { return this["scheduleExpression"] as string; }
        }


    }

    public class TaskAdapterConfigurationStateCollection : ConfigurationElementCollection
    {
        public TaskAdapterConfigurationState this[int index]
        {
            get { return base.BaseGet(index) as TaskAdapterConfigurationState; }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public TaskAdapterConfigurationState this[string key]
        {
            get { return base.BaseGet(key) as TaskAdapterConfigurationState; }
            set
            {
                if (base.BaseGet(key) != null)
                {
                    base.BaseRemove(key);
                }
                this.BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new TaskAdapterConfigurationState();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((TaskAdapterConfigurationState)element).TaskName;
        }
    }

}
