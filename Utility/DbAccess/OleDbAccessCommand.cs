using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.Common; 

namespace Utility.DataAccess
{
    /// <summary>
    /// OleDbAccessCommand
    /// </summary>
    public sealed class OleDbAccessCommand : DbAccessCommand
    {
        #region TraceInfo
        static string TraceClass = typeof(OleDbAccessCommand).FullName;
        static bool IsTraced = Tracer.Instance.IsTraced(typeof(OleDbAccessCommand));
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public OleDbAccessCommand()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the OleDbAccessCommand class.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the database. </param>
        public OleDbAccessCommand(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        /// A <see cref="DbProviderFactory"/> object.
        /// </summary>
        protected override DbProviderFactory DbProviderFactory
        {
            get { return OleDbFactory.Instance; }
        }        

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the DbCommand and populates the Parameters collection of the specified DbCommand object. 
        /// </summary>
        /// <param name="dbCommand">Which the parameter infomation is to be derived.</param>
        protected override void DeriveParameters(DbCommand dbCommand)
        {
            OleDbCommandBuilder.DeriveParameters((OleDbCommand)dbCommand);
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
            return new OleDbParameter(parameterName, (OleDbType)providerType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);
        }
    }    
}
