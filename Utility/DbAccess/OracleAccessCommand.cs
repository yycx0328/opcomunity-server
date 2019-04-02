using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using System.Data.Common; 

namespace Utility.DataAccess
{
    /// <summary>
    /// OracleAccessCommand
    /// </summary>
    public sealed class OracleAccessCommand : DbAccessCommand
    {
        #region TraceInfo
        static string TraceClass = typeof(OracleAccessCommand).FullName;
        static bool IsTraced = Tracer.Instance.IsTraced(typeof(OracleAccessCommand));
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public OracleAccessCommand()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the OracleAccessCommand class.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the database. </param>
        public OracleAccessCommand(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        /// A <see cref="DbProviderFactory"/> object.
        /// </summary>
        protected override DbProviderFactory DbProviderFactory
        {
            get { return OracleClientFactory.Instance; }
        }

        /// <summary>
        /// The prefix of the DbParameters
        /// </summary>
        public override char ParameterPrefix { get { return ':'; } }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the DbCommand and populates the Parameters collection of the specified DbCommand object. 
        /// </summary>
        /// <param name="dbCommand">Which the parameter infomation is to be derived.</param>
        protected override void DeriveParameters(DbCommand dbCommand)
        {
            OracleCommandBuilder.DeriveParameters((OracleCommand)dbCommand);
        }

        /// <summary>
        /// Create a new instance of a DbParameter object.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="providerType">One of the provider type values.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="direction">One of the ParameterDirection values.</param>
        /// <param name="isNullable">true if the value of the field can be null; otherwise false.</param>
        /// <param name="precision">The total number of digits to the left and right of the decimal point to which Value is resolved.</param>
        /// <param name="scale">The total number of decimal places to which Value is resolved.</param>
        /// <param name="srcColumn">The name of the source column.</param>
        /// <param name="srcVersion">One of the DataRowVersion values.</param>
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <returns></returns>
        public override DbParameter CreateParameter(string parameterName, int providerType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, Object value)
        {
            return new OracleParameter(parameterName, (OracleType)providerType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);
        }
    }    
}
