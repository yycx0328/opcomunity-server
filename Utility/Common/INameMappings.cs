using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    /// <summary>
    /// INameMappings
    /// </summary>
    public interface INameMappings
    {
        /// <summary>
        /// Map
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string Map(string name);
    }    

    /// <summary>
    /// NameValueConfigrationMappings
    /// </summary>
    public class NameValueConfigrationMappings : INameMappings
    {
        private System.Collections.Specialized.NameValueCollection NameValues;

        /// <summary>
        /// NameValueConfigrationMappings
        /// </summary>
        /// <param name="sectionName"></param>
        public NameValueConfigrationMappings(string sectionName)
        {
            NameValues = System.Configuration.ConfigurationManager.GetSection(sectionName) as System.Collections.Specialized.NameValueCollection;
        }

        #region INameMappings Members

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Map(string name)
        {            
            return NameValues[name] ?? name;
        }

        #endregion
    }

    /// <summary>
    /// HashtableConfigrationMappings
    /// </summary>
    public class HashtableConfigrationMappings : INameMappings
    {
        private System.Collections.Hashtable Hashtable;

        /// <summary>
        /// HashtableConfigrationMappings
        /// </summary>
        /// <param name="sectionName"></param>
        public HashtableConfigrationMappings(string sectionName)
        {
            Hashtable = System.Configuration.ConfigurationManager.GetSection(sectionName) as System.Collections.Hashtable;
        }

        #region INameMappings Members

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string Map(string name)
        {
            return (string)(Hashtable[name] ?? name);
        }

        #endregion
    }
}
