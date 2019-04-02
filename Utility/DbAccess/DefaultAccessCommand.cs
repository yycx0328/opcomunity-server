using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common; 

namespace Utility.DataAccess
{
    /// <summary>
    /// DefaultAccessCommand
    /// </summary>
    public sealed class DefaultAccessCommand : DbAccessCommand
    {
        private string ProviderName;

        private static Dictionary<string, DbProviderFactory> DbProviders = new Dictionary<string, DbProviderFactory>();

        /// <summary>
        /// DefaultAccessCommand
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        public DefaultAccessCommand(string providerName, string connectionString)
        {
            ParameterChecker.CheckNullOrEmpty("CommonDbAccessCommand", "providerName", providerName);
            ParameterChecker.CheckNullOrEmpty("CommonDbAccessCommand", "connectionString", connectionString);

            this.ProviderName = providerName;
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// DefaultAccessCommand
        /// </summary>
        /// <param name="connSettings"></param>
        public DefaultAccessCommand(System.Configuration.ConnectionStringSettings connSettings)
        {
            ParameterChecker.CheckNull("CommonDbAccessCommand", "connSettings", connSettings);

            this.ProviderName = connSettings.ProviderName;
            this.ConnectionString = connSettings.ConnectionString;
        }

        /// <summary>
        /// DefaultAccessCommand
        /// </summary>
        /// <param name="connSettingsName"></param>
        public DefaultAccessCommand(string connSettingsName)
        {
            ParameterChecker.CheckNullOrEmpty("CommonDbAccessCommand", "connSettingsName", connSettingsName);

            //var alias = System.Configuration.ConfigurationManager.AppSettings[connSettingsName];
            //if (!string.IsNullOrEmpty(alias))
            //    connSettingsName = alias;

            var connSettings = System.Configuration.ConfigurationManager.ConnectionStrings[connSettingsName];
            ParameterChecker.CheckNull("CommonDbAccessCommand", "connSettings", connSettings);

            this.ProviderName = connSettings.ProviderName;
            this.ConnectionString = connSettings.ConnectionString;
        }

        /// <summary>
        /// DbProviderFactory
        /// </summary>
        protected override DbProviderFactory DbProviderFactory
        {
            get
            {
                return Singleton<DbProviderFactory>.GetInstance(DbProviders, this.ProviderName,
                    delegate()
                    {
                        return DbProviderFactories.GetFactory(this.ProviderName);
                    });
            }
        }
    }
}
