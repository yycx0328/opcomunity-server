using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common; 
namespace Utility.DataAccess
{
    /// <summary>
    /// SqlAccessCommand
    /// </summary>
    public sealed class SqlAccessCommand : DbAccessCommand
    {
        #region TraceInfo
        static string TraceClass = typeof(SqlAccessCommand).FullName;
        static bool IsTraced = Tracer.Instance.IsTraced(typeof(SqlAccessCommand));
        #endregion

        /// <summary>
        /// constructor
        /// </summary>
        public SqlAccessCommand()
            : base()
        { }

        /// <summary>
        /// Initializes a new instance of the SqlAccessCommand class.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the database. </param>
        public SqlAccessCommand(string connectionString)
            : base(connectionString)
        { }

        /// <summary>
        /// A <see cref="DbProviderFactory"/> object.
        /// </summary>
        protected override DbProviderFactory DbProviderFactory
        {
            get { return SqlClientFactory.Instance; }
        }
        
        /// <summary>
        /// The prefix of the DbParameters
        /// </summary>
        public override char ParameterPrefix { get { return '@'; } }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the DbCommand and populates the Parameters collection of the specified DbCommand object. 
        /// </summary>
        /// <param name="dbCommand">Which the parameter infomation is to be derived.</param>
        protected override void DeriveParameters(DbCommand dbCommand)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)dbCommand);
        }

        /// <summary>
        /// Sends the CommandText to the Connection and builds an XmlReader object. 
        /// </summary>
        /// <returns>An XmlReader object.</returns>
        public override System.Xml.XmlReader ExecuteXmlReader()
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteXmlReader";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                System.Xml.XmlReader reader = ((SqlCommand)DbCommand).ExecuteXmlReader();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "Null:" + (reader == null), GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return reader;
            }
            finally
            {
                context.FinallyCall();
            }
        }        

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteNonQuery.</returns>
        public override IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, Object state)
        {
            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteNonQuery";
            if (IsTraced)
            {
                Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                    new string[] { "CommandText", "callback", "state", "Parameters" },
                    new object[] { this.CommandText, callback, state, GetTraceParameters(true) });
            }
            #endregion

            IAsyncResult asyncResult = ((SqlCommand)DbCommand).BeginExecuteNonQuery(callback, state);

            return asyncResult;
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginExecuteNonQuery.</param>
        /// <returns>The number of rows affected (the same behavior as ExecuteNonQuery).</returns>
        public override int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            ParameterChecker.CheckNull("SqlAccessCommand.EndExecuteNonQuery", "asyncResult", asyncResult);

            int result = ((SqlCommand)DbCommand).EndExecuteNonQuery(asyncResult);

            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteNonQuery";
            if (IsTraced)
            {
                Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                    new string[] { "asyncResult", "result", "Parameters" },
                    new object[] { "Null:" + (asyncResult == null), result, GetTraceParameters(false) });
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <param name="behavior">One of the CommandBehavior values, indicating options for statement execution and data retrieval.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteReader.</returns>        
        public override IAsyncResult BeginExecuteReader(AsyncCallback callback, Object state, CommandBehavior behavior)
        {
            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteReader";
            if (IsTraced)
            {
                Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                    new string[] { "CommandText", "callback", "state", "behavior", "Parameters" },
                    new object[] { this.CommandText, callback, state, behavior, GetTraceParameters(true) });
            }
            #endregion

            IAsyncResult asyncResult = ((SqlCommand)DbCommand).BeginExecuteReader(callback, state, behavior);

            return asyncResult;
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to EndExecuteReader.</param>
        /// <returns>A DbDataReader object that can be used to retrieve the requested rows (the same behavior as ExecuteReader).</returns>
        public override DbDataReader EndExecuteReader(IAsyncResult asyncResult)
        {
            ParameterChecker.CheckNull("SqlAccessCommand.EndExecuteReader", "asyncResult", asyncResult);

            DbDataReader result = ((SqlCommand)DbCommand).EndExecuteReader(asyncResult);

            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteReader";
            if (IsTraced)
            {
                Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                    new string[] { "asyncResult", "result", "Parameters" },
                    new object[] { "Null:" + (asyncResult == null), "Null:" + (result == null), GetTraceParameters(false) });
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteXmlReader.</returns>        
        public override IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, Object state)
        {
            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteXmlReader";
            if (IsTraced)
            {
                Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                    new string[] { "CommandText", "callback", "state", "Parameters" },
                    new object[] { this.CommandText, callback, state, GetTraceParameters(true) });
            }
            #endregion

            IAsyncResult asyncResult = ((SqlCommand)DbCommand).BeginExecuteXmlReader(callback, state);

            return asyncResult;
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to EndExecuteReader.</param>
        /// <returns>An XmlReader object that can be used to fetch the resulting XML data (the same behavior as ExecuteXmlReader).</returns>
        public override System.Xml.XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
        {
            ParameterChecker.CheckNull("SqlAccessCommand.EndExecuteXmlReader", "asyncResult", asyncResult);

            System.Xml.XmlReader result = ((SqlCommand)DbCommand).EndExecuteXmlReader(asyncResult);

            #region TraceInfo
            const string TRACE_METHOD = "AsyncExecuteXmlReader";
            if (IsTraced)
            {
                Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                    new string[] { "asyncResult", "result", "Parameters" },
                    new object[] { "Null:" + (asyncResult == null), "Null:" + (result == null), GetTraceParameters(false) });
            }
            #endregion
            
            return result;
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
            return new SqlParameter(parameterName, (SqlDbType)providerType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value);
        }
    }    
}
