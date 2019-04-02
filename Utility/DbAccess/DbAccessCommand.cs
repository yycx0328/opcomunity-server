using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Utility.DataAccess
{
    /// <summary>
    /// Represents an SQL statement or stored procedure to execute against a data source. Provides a base class for database-specific classes that represent commands. 
    /// </summary>
    public abstract class DbAccessCommand : IDbCommand, IDisposable
    {       
        #region TraceInfo
        static string TraceClass = typeof(DbAccessCommand).FullName;
        static bool IsTraced = Tracer.Instance.IsTraced(typeof(DbAccessCommand));
        #endregion

        #region DbAccessCommand
        /// <summary>
        /// Initializes a new instance of the DbAccessCommand class.
        /// </summary>                
        protected DbAccessCommand()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessCommand class.
        /// </summary>
        /// <param name="connectionString">The connection string used to open the database. </param>        
        protected DbAccessCommand(string connectionString)
        {
            ParameterChecker.CheckNullOrEmpty("DbAccessCommand", "connectionString", connectionString);
            
            this.ConnectionString = connectionString;
        }

        /// <summary>
        /// GetAccessCommand
        /// </summary>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static TCommand GetAccessCommand<TCommand>(string connectionString)
            where TCommand : DbAccessCommand, new()
        {
            TCommand t = new TCommand();
            t.ConnectionString = connectionString;
            return t;
        }
        #endregion

        #region Properties
        /// <summary>
        /// A <see cref="DbProviderFactory"/> object.
        /// </summary>
        protected abstract DbProviderFactory DbProviderFactory { get; }

        /// <summary>
        /// Gets or sets the DbConnection used by this DbCommand
        /// </summary>        
        public DbConnection Connection
        {
            get
            {
                if (_DbConnection == null) 
                    _DbConnection = DbProviderFactory.CreateConnection(); 
                return _DbConnection;
            }
            set
            {
                _DbConnection = value;
            }
        }
        private DbConnection _DbConnection = null;

        /// <summary>
        /// Gets or sets the DbTransaction within which this DbCommand object executes.
        /// </summary>        
        public DbTransaction Transaction
        {
            get { return DbCommand.Transaction; }
            set { DbCommand.Transaction = value; }
        }

        /// <summary>
        /// Gets or sets a command used to select, update, delete records in the data source.
        /// </summary>
        protected DbCommand DbCommand
        {
            get 
            {
                if (_DbCommand == null) 
                    _DbCommand = Connection.CreateCommand(); 
                return _DbCommand; 
            }
        }
        private DbCommand _DbCommand = null;

        /// <summary>
        /// Gets or sets the DbDataAdapter within which this DbCommand object executes.
        /// </summary>
        protected DbDataAdapter DbDataAdapter
        {
            get
            {
                if (_DbDataAdapter == null)
                {
                    _DbDataAdapter = DbProviderFactory.CreateDataAdapter();
                    _DbDataAdapter.SelectCommand = DbCommand;
                }
                return _DbDataAdapter; 
            }
        }
        private DbDataAdapter _DbDataAdapter = null;

        /// <summary>
        /// Gets or sets the DbCommandBuilder within which this DbCommand object executes.
        /// </summary>
        protected DbCommandBuilder DbCommandBuilder
        {
            get 
            {
                if (_DbCommandBuilder == null)
                    _DbCommandBuilder = DbProviderFactory.CreateCommandBuilder(); 
                return _DbCommandBuilder;
            }
        }
        private DbCommandBuilder _DbCommandBuilder = null;
        
        /// <summary>
        /// Gets or sets the text command to run against the data source.
        /// </summary>
        public string CommandText
        {
            get { return DbCommand.CommandText; }
            set { DbCommand.CommandText = value; }
        }

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error.
        /// </summary>
        public int CommandTimeout
        {
            get { return DbCommand.CommandTimeout; }
            set { DbCommand.CommandTimeout = value; }
        }

        /// <summary>
        /// Indicates or specifies how the CommandText property is interpreted.
        /// </summary>
        public CommandType CommandType
        {
            get { return DbCommand.CommandType; }
            set { DbCommand.CommandType = value; }
        }

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        public string ConnectionString
        {
            get { return this.Connection.ConnectionString; }
            set { this.Connection.ConnectionString = value; }
        }

        /// <summary>
        /// Gets the collection of DbParameter objects.
        /// </summary>
        public DbParameterCollection Parameters
        {
            get { return DbCommand.Parameters; }
        }        

        /// <summary>
        /// Gets or sets how command results are applied to the DataRow when used by the Update method of a DbDataAdapter.
        /// </summary>
        public UpdateRowSource UpdatedRowSource
        {
            get { return DbCommand.UpdatedRowSource; }
            set { DbCommand.UpdatedRowSource = value; }
        }

        /// <summary>
        /// The prefix of the DbParameters
        /// </summary>
        public virtual char ParameterPrefix { get { return default(char); } }

        /// <summary>
        /// Whether auto open or close connection when execute command.
        /// </summary>
        public bool AutoOpenClose
        {
            get { return _AutoOpenClose; }
            set { _AutoOpenClose = value; }
        }
        private bool _AutoOpenClose = true;
        #endregion

        #region DbParameter
        /// <summary>
        /// Creates a new instance of a DbParameter object. 
        /// </summary>
        /// <returns>A DbParameter object.</returns>
        public virtual DbParameter CreateParameter(string parameterName)
        {
            DbParameter parameter = DbCommand.CreateParameter();
            parameter.ParameterName = BuildParameterName(parameterName);
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <returns>A DbParameter object.</returns>
        public DbParameter CreateParameter(string parameterName, Object value)
        {
            DbParameter parameter = CreateParameter(parameterName);
            parameter.Value = value ?? DBNull.Value;
            return parameter;
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
        /// <returns>DbParameter</returns>
        public virtual DbParameter CreateParameter(string parameterName, int providerType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, Object value)
        {
            DbParameter parameter = CreateParameter(parameterName);
            parameter.DbType = (DbType)providerType;
            parameter.Size = size;
            parameter.Direction = direction;
            parameter.IsNullable = isNullable;
            ((IDbDataParameter)parameter).Precision = precision;
            ((IDbDataParameter)parameter).Scale = scale;
            parameter.SourceColumn = srcColumn;
            parameter.SourceVersion = srcVersion;
            parameter.Value = value ?? DBNull.Value;
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="providerType">One of the provider type values.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="direction">One of the ParameterDirection values.</param>        
        /// <param name="precision">The total number of digits to the left and right of the decimal point to which Value is resolved.</param>
        /// <param name="scale">The total number of decimal places to which Value is resolved.</param>        
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <returns>DbParameter</returns>
        public DbParameter CreateParameter(string parameterName, int providerType, int size, ParameterDirection direction, byte precision, byte scale, Object value)
        {
            return CreateParameter(parameterName, providerType, size, direction, true, precision, scale, string.Empty, DataRowVersion.Default, value);
        }

        /// <summary>
        /// Create a new instance of a DbParameter object and add to Parameters
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="providerType">One of the provider type values.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <param name="direction">One of the ParameterDirection values.</param>        
        /// <param name="precision">The total number of digits to the left and right of the decimal point to which Value is resolved.</param>
        /// <param name="scale">The total number of decimal places to which Value is resolved.</param>        
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <returns>DbParameter</returns>
        public DbParameter AddParameter(string parameterName, int providerType, int size, ParameterDirection direction, byte precision, byte scale, Object value)
        {
            DbParameter parameter = this.CreateParameter(parameterName, providerType, size, direction, precision, scale, value);
            this.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object and add to Parameters
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <returns>A DbParameter object.</returns>
        public DbParameter AddParameter(string parameterName, Object value)
        {
            DbParameter parameter = this.CreateParameter(parameterName, value);
            this.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object and add to Parameters
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">An Object that is the value of the DbParameter.</param>
        /// <param name="size">The length of the parameter.</param>
        /// <returns>A DbParameter object.</returns>
        public DbParameter AddParameter(string parameterName, string value, int size)
        {
            if (string.IsNullOrEmpty(value))
                value = string.Empty;
            else if (value.Length > size)
                value = value.Substring(0, size);
            DbParameter parameter = this.CreateParameter(parameterName, value);
            this.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object and add to Parameters
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="providerType">One of the provider type values.</param>
        /// <param name="size">The length of the parameter.</param>        
        /// <returns>DbParameter</returns>
        public DbParameter AddOutputParameter(string parameterName, int providerType, int size)
        {
            DbParameter parameter = this.CreateParameter(parameterName, providerType, size, ParameterDirection.Output, 0, 0, DBNull.Value);
            this.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Create a new instance of a DbParameter object and add to Parameters
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="providerType">One of the provider type values.</param>
        /// <param name="size">The length of the parameter.</param>        
        /// <returns>DbParameter</returns>
        public DbParameter AddReturnParameter(string parameterName, int providerType, int size)
        {
            DbParameter parameter = this.CreateParameter(parameterName, providerType, size, ParameterDirection.ReturnValue, 0, 0, DBNull.Value);
            this.Parameters.Add(parameter);
            return parameter;
        }
        
        #endregion

        #region Open & Close
        /// <summary>
        /// Opens a database connection with the settings specified by the ConnectionString. 
        /// </summary>
        public void Open()
        {
            if (this.Connection.State != ConnectionState.Open)
            {
                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, "Open_Close", this.ConnectionString);
                }
                #endregion

                this.Connection.Open();
            }
        }

        /// <summary>
        /// Closes the connection to the database. This is the preferred method of closing any open connection. 
        /// </summary>
        public void Close()
        {
            if (this.Connection.State != ConnectionState.Closed)
            {
                this.Connection.Close();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, "Open_Close", string.Empty);
                }
                #endregion
            }
        }
        #endregion

        #region Transaction
        /// <summary>
        /// Starts a database transaction. 
        /// </summary>
        public void BeginTransaction()
        {
            if (AutoOpenClose)
            {
                this.Open();
            }

            DbCommand.Transaction = this.Connection.BeginTransaction(); 

            #region TraceInfo
            if (IsTraced)
            {
                Tracer.Instance.EnterFunction(TraceClass, "TransactionBegin_End", string.Empty);
            }
            #endregion
        }

        /// <summary>
        /// Commits the database transaction. 
        /// </summary>
        public void CommitTransaction()
        {
            if (DbCommand.Transaction != null)
            {
                DbCommand.Transaction.Commit();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, "TransactionBegin_End", "CommitTransaction");
                }
                #endregion

                if (AutoOpenClose)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state. 
        /// </summary>
        public void RollbackTransaction()
        {
            if (DbCommand.Transaction != null)
            {
                DbCommand.Transaction.Rollback();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, "TransactionBegin_End", "RollbackTransaction");
                }
                #endregion

                if (AutoOpenClose)
                {
                    this.Close();
                }
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// Executes a SQL statement against a connection object. 
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery()
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteNonQuery";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                int result = DbCommand.ExecuteNonQuery();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// Executes the CommandText against the Connection and returns a DbDataReader. 
        /// </summary>
        /// <param name="readerHandler">A DbDataReader handler</param>
        /// <param name="behavior">One of the CommandBehavior values.</param>
        /// <param name="startRecord">The zero-based record number to start with.</param>        
        /// <returns>A object.</returns>
        public object ExecuteReader(Func<DbDataReader, object> readerHandler, CommandBehavior behavior, int startRecord)
        {
            ParameterChecker.CheckNull("DbAccessCommand.ExecuteReader", "readerHandler", readerHandler);

            ExecuteContext context = new ExecuteContext(this);
            DbDataReader dataReader = null;
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteReader";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "startRecord", "CommandText", "Parameters" },
                        new object[] { startRecord, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                dataReader = DbCommand.ExecuteReader(behavior);

                int tempIndex = -1;
                while (++tempIndex < startRecord && dataReader.Read()) ;

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "result is DbDataReader", GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return readerHandler(dataReader);
            }
            finally
            {
                context.FinallyCall();
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        /// <summary>
        /// Executes the CommandText against the Connection and returns a DbDataReader. 
        /// </summary>
        /// <param name="readerHandler">A DbDataReader handler</param>
        /// <param name="behavior">One of the CommandBehavior values.</param>        
        /// <returns>A object.</returns>
        public object ExecuteReader(Func<DbDataReader, object> readerHandler, CommandBehavior behavior)
        {
            return this.ExecuteReader(readerHandler, behavior, 0);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and returns a DbDataReader. 
        /// </summary>
        /// <param name="readerHandler">A DbDataReader handler</param>        
        /// <param name="startRecord">The zero-based record number to start with.</param>        
        /// <returns>A object.</returns>
        public object ExecuteReader(Func<DbDataReader, object> readerHandler, int startRecord)
        {
            CommandBehavior behavior = DbCommand.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
            return this.ExecuteReader(readerHandler, behavior, startRecord);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and returns a DbDataReader. 
        /// </summary>
        /// <param name="readerHandler">A DbDataReader handler</param>
        /// <returns>A object.</returns>
        public object ExecuteReader(Func<DbDataReader, object> readerHandler)
        {
            CommandBehavior behavior = DbCommand.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
            return this.ExecuteReader(readerHandler, behavior, 0);
        }
        #endregion

        #region ExecuteEntitySet
        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity">The type that applies to</typeparam>
        /// <param name="mappings"></param>
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>(INameMappings mappings, int startRecord) where TEntity : new()
        {
            ExecuteContext context = new ExecuteContext(this);
            DbDataReader dataReader = null;
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteEntitySet";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "startRecord", "CommandText", "Parameters" },
                        new object[] { startRecord, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                dataReader = DbCommand.ExecuteReader();

                int tempIndex = -1;
                while (++tempIndex < startRecord && dataReader.Read()) ;

                IEnumerable<System.Reflection.PropertyInfo> properties = typeof(TEntity).GetProperties();
                properties = properties.Where(p => p.CanWrite && p.GetIndexParameters().Length == 0);
                                
                List<TEntity> list = new List<TEntity>();
                while (dataReader.Read())
                {
                    TEntity entity = new TEntity();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {                        
                        string name = dataReader.GetName(i);
                        if (mappings != null)
                            name = mappings.Map(name);
                        var pi = properties.FirstOrDefault(p => string.Equals(p.Name, name, StringComparison.InvariantCultureIgnoreCase));
                        if (pi != null)
                        {
                            try
                            {
                                object value = dataReader.GetValue(i);
                                value = ReflectionHelper.ChangeValueType(value, pi.PropertyType);
                                ReflectionHelper.SetProperty(entity, pi, value);
                            }
                            catch(Exception ex)
                            {
                                string message = string.Format("PropertyName={0}&FieldName={1}&DataTypeName={2}&FieldType={3}", pi.Name, dataReader.GetName(i), dataReader.GetDataTypeName(i), dataReader.GetFieldType(i));
                                throw new ApplicationException(message, ex);
                            }
                        }
                    }
                    list.Add(entity);
                }

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "result is DbDataReader", GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return list;
            }
            finally
            {
                context.FinallyCall();
                if (dataReader != null && !dataReader.IsClosed)
                    dataReader.Close();
            }
        }

        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity">The type that applies to</typeparam>        
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>(int startRecord) where TEntity : new()
        {
            return ExecuteEntitySet<TEntity>(null, startRecord);
        }

        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity">The type that applies to</typeparam>     
        /// <param name="mappings"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>(INameMappings mappings) where TEntity : new()
        {
            return ExecuteEntitySet<TEntity>(mappings, 0);
        }

        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity">The type that applies to</typeparam>             
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>() where TEntity : new()
        {
            return ExecuteEntitySet<TEntity>(null, 0);
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// Executes the query and returns the first column of the first row in the result set returned by the query. All other columns and rows are ignored. 
        /// </summary>
        /// <returns>The first column of the first row in the result set.</returns>
        public object ExecuteScalar()
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteScalar";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                object result = DbCommand.ExecuteScalar();

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region ExecuteXmlReader
        /// <summary>
        /// Sends the CommandText to the Connection and builds an XmlReader object. 
        /// </summary>
        /// <returns>An XmlReader object.</returns>
        public virtual System.Xml.XmlReader ExecuteXmlReader()
        {
            DataSet dataSet = this.ExecuteDataSet();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();            
            dataSet.WriteXml(ms);
            ms.Position = 0;
            return new System.Xml.XmlTextReader(ms);
        }
        #endregion

        #region ExecuteSchemaTable
        /// <summary>
        /// Returns a DataTable that describes the column metadata of the DbDataReader.
        /// </summary>
        /// <returns>A DataTable that describes the column metadata.</returns>
        public virtual DataTable ExecuteSchemaTable()
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteSchemaTable";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                DataTable result = null;
                using (DbDataReader reader = DbCommand.ExecuteReader(CommandBehavior.KeyInfo))
                {
                    result = reader.GetSchemaTable();
                    reader.Close();
                }

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "Null:" + (result == null), GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region AttachAccessInfo
        /// <summary>
        /// Fill DbCommand property with a DbAccessInformation
        /// </summary>
        /// <param name="accessInfo">A DbAccessInformation object</param>
        public void AttachAccessInfo(DbAccessInformation accessInfo)
        {
            AttachAccessInfo(accessInfo, (DbParameter[])null);
        }

        /// <summary>
        /// Fill DbCommand property with a DbAccessInformation.
        /// </summary>
        /// <param name="accessInfo">A DbAccessInformation object.</param>
        /// <param name="parameters">Attached parameters.</param>
        public void AttachAccessInfo(DbAccessInformation accessInfo, DbParameter[] parameters)
        {
            if (accessInfo == null)
                return;

            DbCommand.CommandText = accessInfo.CommandText;
            DbCommand.CommandType = accessInfo.CommandType;
            DbCommand.CommandTimeout = accessInfo.CommandTimeout;

            DbCommand.Parameters.Clear();

            if (parameters == null && accessInfo.CommandType == CommandType.StoredProcedure)
                parameters = GetCachedParameters(accessInfo.CommandText);

            if (parameters == null)
            {
                if (accessInfo.Parameters != null)
                {
                    foreach (DbAccessParameter accParameter in accessInfo.Parameters)
                    {
                        if (accParameter.ProvideType >= 0)
                        {
                            DbParameter parameter = this.CreateParameter(
                                BuildParameterName(accParameter.ParameterName),
                                accParameter.ProvideType,
                                accParameter.Size,
                                accParameter.Direction,
                                accParameter.IsNullable,
                                accParameter.Precision,
                                accParameter.Scale,
                                accParameter.SourceColumn,
                                accParameter.SourceVersion,
                                accParameter.Value);
                            DbCommand.Parameters.Add(parameter);
                        }
                        else
                        {
                            DbParameter parameter = this.CreateParameter(accParameter.ParameterName);                            
                            parameter.DbType = accParameter.DbType;
                            parameter.Size = accParameter.Size;
                            parameter.Direction = accParameter.Direction;
                            parameter.IsNullable = accParameter.IsNullable;
                            ((IDbDataParameter)parameter).Precision = accParameter.Precision;
                            ((IDbDataParameter)parameter).Scale = accParameter.Scale;
                            parameter.SourceColumn = accParameter.SourceColumn;
                            parameter.SourceVersion = accParameter.SourceVersion;
                            parameter.Value = accParameter.Value ?? DBNull.Value;
                            parameter.SourceColumnNullMapping = accParameter.SourceColumnNullMapping;
                            DbCommand.Parameters.Add(parameter);
                        }
                    }
                }
            }
            else
            {
                DbCommand.Parameters.AddRange(parameters);
                if (accessInfo.Parameters != null)
                    AssignParameterValues(accessInfo.Parameters);
            }
        }

        /// <summary>
        /// Fill DbCommand property with a DbAccessInformation.
        /// </summary>        
        /// <param name="accessInfo">A DbAccessInformation object.</param>
        /// <param name="dbCommand">A DbCommand object.</param>
        /// <returns></returns>
        private DbCommand InnerAttachAccessInfo(DbAccessInformation accessInfo, DbCommand dbCommand)
        {
            if (accessInfo == null || dbCommand == null)
                return dbCommand;

            dbCommand.CommandText = accessInfo.CommandText;
            dbCommand.CommandType = accessInfo.CommandType;
            dbCommand.CommandTimeout = accessInfo.CommandTimeout;

            dbCommand.Parameters.Clear();

            if (accessInfo.Parameters != null)
            {
                foreach (DbAccessParameter accParameter in accessInfo.Parameters)
                {
                    if (accParameter.ProvideType >= 0)
                    {
                        DbParameter parameter = this.CreateParameter(
                            accParameter.ParameterName,
                            accParameter.ProvideType,
                            accParameter.Size,
                            accParameter.Direction,
                            accParameter.IsNullable,
                            accParameter.Precision,
                            accParameter.Scale,
                            accParameter.SourceColumn,
                            accParameter.SourceVersion,
                            accParameter.Value);
                        dbCommand.Parameters.Add(parameter);
                    }
                    else
                    {
                        DbParameter parameter = this.CreateParameter(accParameter.ParameterName);
                        parameter.DbType = accParameter.DbType;
                        parameter.Size = accParameter.Size;
                        parameter.Direction = accParameter.Direction;
                        parameter.IsNullable = accParameter.IsNullable;
                        ((IDbDataParameter)parameter).Precision = accParameter.Precision;
                        ((IDbDataParameter)parameter).Scale = accParameter.Scale;
                        parameter.SourceColumn = accParameter.SourceColumn;
                        parameter.SourceVersion = accParameter.SourceVersion;
                        parameter.Value = accParameter.Value ?? DBNull.Value;
                        parameter.SourceColumnNullMapping = accParameter.SourceColumnNullMapping;
                        dbCommand.Parameters.Add(parameter);
                    }
                }
            }

            return dbCommand;
        }

        /// <summary>
        /// Fill DbCommand property with a DataRow.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement, table name or stored procedure to execute at the data source.</param>
        /// <param name="commandType">Indicate how the CommandText property is to be interpreted</param>
        /// <param name="row">a DataRow object</param>
        /// <param name="parameters">Attached parameters.</param>
        public void AttachAccessInfo(string commandText, CommandType commandType, DataRow row, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(commandText))
                return;

            DbCommand.CommandText = commandText;
            DbCommand.CommandType = commandType;
            
            if (parameters == null && commandType == CommandType.StoredProcedure)
                parameters = GetCachedParameters(commandText);

            DbCommand.Parameters.Clear();
            if (parameters != null)
            {
                if (row != null)
                {
                    DataRowVersion defaultVersion = GetReferenceRowVersion(row);
                    DataColumn col;                    
                    foreach (DbParameter parameter in parameters)
                    {
                        col = row.Table.Columns[parameter.SourceColumn];
                        if (col != null)
                        {
                            if (row.HasVersion(parameter.SourceVersion))
                                parameter.Value = row[col, parameter.SourceVersion] ?? DBNull.Value;
                            else
                                parameter.Value = row[col, defaultVersion] ?? DBNull.Value;
                        }
                    }
                }
                
                DbCommand.Parameters.AddRange(parameters);
            }
        }

        /// <summary>
        /// Fill DbCommand property with a DataRow.
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement, table name or stored procedure to execute at the data source.</param>        
        /// <param name="parameters"></param>
        public void AttachAccessInfo(string commandText, params DbParameter[] parameters)
        {
            this.AttachAccessInfo(commandText, CommandType.Text, null, parameters);
        }

        /// <summary>
        /// Get reference DataRowVersion
        /// </summary>
        /// <param name="row">A DataRow object.</param>
        /// <returns>The reference DataRowVersion</returns>
        protected DataRowVersion GetReferenceRowVersion(DataRow row)
        {
            if (row == null)
                return DataRowVersion.Default;

            DataRowVersion[] versions = new DataRowVersion[] { DataRowVersion.Default, DataRowVersion.Current, DataRowVersion.Original, DataRowVersion.Proposed };
            for (int i = 0; i < versions.Length; i++)
            {
                if (row.HasVersion(versions[i]))
                    return versions[i];
            }
            return DataRowVersion.Default;
        }
        #endregion

        #region Procedure parameters
        /// <summary>
        /// BuildParameterName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected string BuildParameterName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            if (name[0] == ParameterPrefix || ParameterPrefix == default(char))
                return name;
            else
                return ParameterPrefix + name;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the DbCommand and populates the Parameters collection of the specified DbCommand object. 
        /// </summary>
        /// <param name="procedureName">A stored procedure name.</param>
        /// <param name="includeReturnValueParameter">If include return value parameter.</param>
        /// <returns>The parameters of the stored procedure</returns>
        public DbParameter[] DeriveParameters(string procedureName, bool includeReturnValueParameter)
        {
            ParameterChecker.CheckNullOrEmpty("DbAccessCommand.DeriveParameters", "procedureName", procedureName);

            DbCommand cmd = this.Connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            ExecuteContext context = new ExecuteContext(this);
            try
            {
                DeriveParameters(cmd);

                context.SuccessfullyCall();
            }
            finally
            {
                context.FinallyCall();
            }
            
            if (!includeReturnValueParameter)
            {
                DbParameter returnParameter = GetReturnParameter(cmd.Parameters);
                if (returnParameter != null)
                    cmd.Parameters.Remove(returnParameter);
            }
            DbParameter[] parameters = new DbParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(parameters, 0);
            return parameters;
        }

        /// <summary>
        /// Retrieves parameter information from the stored procedure specified in the DbCommand and populates the Parameters collection of the specified DbCommand object. 
        /// </summary>
        /// <param name="dbCommand">Which the parameter infomation is to be derived.</param>
        protected virtual void DeriveParameters(DbCommand dbCommand)
        {
            Type type = DbCommandBuilder.GetType();
            ReflectionHelper.CallMethod(type, "DeriveParameters", new object[] { dbCommand }, Type.EmptyTypes);
        }

        /// <summary>
        /// Gets the cached parameters of procedure
        /// </summary>
        /// <param name="procedureName">The procedure name.</param>
        /// <returns>The cached parameters</returns>
        public DbParameter[] GetCachedParameters(string procedureName)
        {
            string cacheKey = string.Format("{0}:{1}", this.ConnectionString, procedureName).ToLower();

            DbParameter[] parameters;
            if (!ParametersCacheTable.TryGetValue(cacheKey, out parameters))
            {
                lock (LockKey)
                {
                    if (!ParametersCacheTable.TryGetValue(cacheKey, out parameters))
                    {
                        parameters = this.DeriveParameters(procedureName, true);
                        ParametersCacheTable[cacheKey] = parameters;
                    }
                }
            }

            return CloneParameters(parameters);
        }
        private static object LockKey = new object();

        /// <summary>
        /// Clone the parameters
        /// </summary>
        /// <param name="parameters">The parameters to clone</param>
        /// <returns>The cloned parameters</returns>
        public DbParameter[] CloneParameters(params DbParameter[] parameters)
        {
            if (parameters == null)
                return null;

            DbParameter[] clonedParameters = new DbParameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                object oldValue = parameters[i].Value;
                parameters[i].Value = DBNull.Value;
                clonedParameters[i] = ((ICloneable)parameters[i]).Clone() as DbParameter;
                parameters[i].Value = oldValue;
            }
            return clonedParameters;
        }

        private Dictionary<string, DbParameter[]> ParametersCacheTable = new Dictionary<string, DbParameter[]>();
        #endregion

        #region Assign & Pickup ParameterValues
        /// <summary>
        /// Assign values to DbCommand.Parameters.
        /// </summary>
        /// <param name="parameterValues">Values list.</param>
        public void AssignParameterValues(object[] parameterValues)
        {
            if (parameterValues == null || parameterValues.Length == 0 || DbCommand.Parameters.Count == 0)
                return;

            int parameterIndexShift = (DbCommand.Parameters[0].Direction == ParameterDirection.ReturnValue ? 1 : 0);
            int count = parameterValues.Length;
            if (DbCommand.Parameters.Count != count + parameterIndexShift)
            {
                throw new ApplicationException("The number of parameters doesn't match the number of parameterValues.");
            }
            
            for (int i = 0; i < count; i++)
            {
                DbCommand.Parameters[i + parameterIndexShift].Value = parameterValues[i] ?? DBNull.Value;
            }
        }

        /// <summary>
        /// Assign values to DbCommand.Parameters.
        /// </summary>
        /// <param name="accessParameters">A DbAccessInformation object.</param>
        public void AssignParameterValues(DbAccessParameterCollection  accessParameters)
        {
            if (accessParameters == null)
                return;
            
            foreach (DbAccessParameter accParameter in accessParameters)
            {
                DbParameter dbParameter = DbCommand.Parameters[BuildParameterName(accParameter.ParameterName)];
                if (dbParameter != null)
                {
                    dbParameter.Value = accParameter.Value ?? DBNull.Value;
                }
            }
        }

        /// <summary>
        /// Pick up parameter values to accessInfo.
        /// </summary>
        /// <param name="accessInfo">The DbAccessInformation object.</param>
        public void PickupParameteValues(DbAccessInformation accessInfo)
        {
            foreach (DbParameter parameter in DbCommand.Parameters)
            {
                if (parameter.Direction != ParameterDirection.Input)
                {
                    DbAccessParameter accParameter = accessInfo.Parameters[parameter.ParameterName];
                    if (accParameter == null && ParameterPrefix != default(char))
                    {
                        if (parameter.ParameterName[0] == ParameterPrefix)
                            accParameter = accessInfo.Parameters[parameter.ParameterName.Substring(1)];
                        else
                            accParameter = accessInfo.Parameters[ParameterPrefix + parameter.ParameterName];
                    }

                    if (accParameter == null)
                        accessInfo.AddParameter(parameter.ParameterName, parameter.Value);
                    else
                        accParameter.Value = parameter.Value;
                }
            }          
        }

        /// <summary>
        /// Get the value of the ReturnValue parameter.
        /// </summary>
        /// <returns>The return value.</returns>
        public object GetReturnValue()
        {
            DbParameter parameter = GetReturnParameter(DbCommand.Parameters);
            return parameter == null ? null : parameter.Value;
        }

        /// <summary>
        /// Get the value of the return parameter.
        /// </summary>
        /// <param name="parameters">DbParameterCollection</param>
        /// <returns>The return parameter</returns>
        protected DbParameter GetReturnParameter(DbParameterCollection parameters)
        {
            foreach (DbParameter parameter in parameters)
            {
                if (parameter.Direction == ParameterDirection.ReturnValue)
                    return parameter;
            }
            return null;
        }
        #endregion

        #region Prepare & Cancel
        /// <summary>
        /// Attempts to cancels the execution of a DbCommand. 
        /// </summary>
        public void Cancel()
        {
            DbCommand.Cancel();
        }

        /// <summary>
        /// Creates a prepared (or compiled) version of the command on the data source. 
        /// </summary>
        public void Prepare()
        {
            DbCommand.Prepare();
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// Adds or refreshes rows in a specified range in the DataSet to match those in the data source using the DataSet, DataTable, and IDataReader names. 
        /// </summary>
        /// <param name="dataTable">The name of the DataTable to use for table mapping.</param>
        /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
        public int ExecuteDataTable(DataTable dataTable)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteDataTable";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataTable", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataTable == null), this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataTable == null)
                    dataTable = new DataTable();
                                
                int result = DbDataAdapter.Fill(dataTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the DataSet to match those in the data source using the DataSet, DataTable, and IDataReader names. 
        /// </summary>        
        /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
        public DataTable ExecuteDataTable()
        {
            DataTable dataTable = new DataTable();
            ExecuteDataTable(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Adds or refreshes rows in a DataTable to match those in the data source using the DataTable name, the specified SQL SELECT statement, and CommandBehavior. 
        /// </summary>
        /// <param name="dataTable">A DataTable to fill with records and, if necessary, schema.</param>
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrieve.</param>
        /// <returns>The number of rows successfully added to or refreshed in the DataTable. This does not include rows affected by statements that do not return rows.</returns>
        public int ExecuteDataTable(DataTable dataTable, int startRecord, int maxRecords)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteDataTable";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataTable", "startRecord", "maxRecords", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataTable == null), startRecord, maxRecords, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataTable == null)
                    dataTable = new DataTable();
                                
                int result = DbDataAdapter.Fill(startRecord, maxRecords, dataTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Create a DataTable to match those in the data source using the DataTable name, the specified SQL SELECT statement, and CommandBehavior. 
        /// </summary>        
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrieve.</param>
        /// <returns>A DataTable to fill with records</returns>
        public DataTable ExecuteDataTable(int startRecord, int maxRecords)
        {
            DataTable dataTable = new DataTable();
            ExecuteDataTable(dataTable, startRecord, maxRecords);
            return dataTable;
        }
        #endregion

        #region ExecuteDataRow
        /// <summary>
        /// Create a DataRow to match those in the data source, the specified SQL SELECT statement, and CommandBehavior. 
        /// </summary>
        /// <param name="dataTable">A DataTable to fill with records and, if necessary, schema.</param>
        /// <returns>A DataRow to fill with the first record</returns>
        public DataRow ExecuteDataRow(DataTable dataTable)
        {
            if (dataTable == null)
                dataTable = new DataTable();

            ExecuteDataTable(dataTable, 0, 1);
            if (dataTable == null || dataTable.Rows == null || dataTable.Rows.Count == 0)
                return null;
            else
                return dataTable.Rows[0];
        }

        /// <summary>
        /// Create a DataRow to match those in the data source, the specified SQL SELECT statement, and CommandBehavior. 
        /// </summary>
        /// <returns>A DataRow to fill with the first record</returns>
        public DataRow ExecuteDataRow()
        {
            return ExecuteDataRow(null);
        }
        #endregion

        #region ExecuteTableSchema
        /// <summary>
        /// Configures the schema of the specified DataTable based on the specified SchemaType.
        /// </summary>
        /// <param name="dataTable">The DataTable to be filled with the schema from the data source.</param>
        /// <param name="schemaType">One of the SchemaType values.</param>
        /// <returns>A DataTable that contains schema information returned from the data source.</returns>
        public DataTable ExecuteTableSchema(DataTable dataTable, SchemaType schemaType)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteTableSchema";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataTable", "schemaType", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataTable == null), schemaType, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataTable == null)
                    dataTable = new DataTable();
                                
                DataTable result = DbDataAdapter.FillSchema(dataTable, schemaType);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "result is DataTable", GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Create the schema of new DataTable based on the specified SchemaType of SchemaType.Source.
        /// </summary>        
        /// <returns>A DataTable that contains schema information returned from the data source.</returns>
        public DataTable ExecuteTableSchema()
        {
            return ExecuteTableSchema(new DataTable(), SchemaType.Source);
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table." 
        /// </summary>
        /// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
        /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
        public int ExecuteDataSet(DataSet dataSet)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteDataSet";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataSet", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataSet == null), this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataSet == null)
                    dataSet = new DataSet();
                                
                int result = DbDataAdapter.Fill(dataSet);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Create a DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table." 
        /// </summary>        
        /// <returns>A DataSet to fill with records</returns>
        public DataSet ExecuteDataSet()
        {
            DataSet dataSet = new DataSet();
            ExecuteDataSet(dataSet);
            return dataSet;
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet and DataTable names. 
        /// </summary>
        /// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
        /// <param name="srcTable">The name of the source table to use for table mapping.</param>
        /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
        public int ExecuteDataSet(DataSet dataSet, string srcTable)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteDataSet";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataSet", "srcTable", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataSet == null), srcTable, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataSet == null)
                    dataSet = new DataSet();
                if (string.IsNullOrEmpty(srcTable))
                    srcTable = "Table";
                                
                int result = DbDataAdapter.Fill(dataSet, srcTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Adds or refreshes rows in a specified range in the DataSet to match those in the data source using the DataSet and DataTable names.
        /// </summary>
        /// <param name="dataSet">A DataSet to fill with records and, if necessary, schema.</param>
        /// <param name="startRecord">The zero-based record number to start with.</param>
        /// <param name="maxRecords">The maximum number of records to retrieve.</param>
        /// <param name="srcTable">The name of the source table to use for table mapping.</param>
        /// <returns>The number of rows successfully added to or refreshed in the DataSet. This does not include rows affected by statements that do not return rows.</returns>
        public int ExecuteDataSet(DataSet dataSet, int startRecord, int maxRecords, string srcTable)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteDataSet";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataSet", "startRecord", "maxRecords", "srcTable", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataSet == null), startRecord, maxRecords, srcTable, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataSet == null)
                    dataSet = new DataSet();
                if (string.IsNullOrEmpty(srcTable))
                    srcTable = "Table";
                                
                int result = DbDataAdapter.Fill(dataSet, startRecord, maxRecords, srcTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region ExecuteSetSchema
        /// <summary>
        /// Adds a DataTable named "Table" to the specified DataSet and configures the schema to match that in the data source based on the specified SchemaType.
        /// </summary>
        /// <param name="dataSet">A DataSet to insert the schema in.</param>
        /// <param name="schemaType">One of the SchemaType values that specify how to insert the schema.</param>
        /// <returns>A reference to a collection of DataTable objects that were added to the DataSet.</returns>
        public DataTable[] ExecuteSetSchema(DataSet dataSet, SchemaType schemaType)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteSetSchema";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataSet", "schemaType", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataSet == null), schemaType, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataSet == null)
                    dataSet = new DataSet();
                                
                DataTable[] tables = DbDataAdapter.FillSchema(dataSet, schemaType);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "Null:" + (tables == null), GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return tables;
            }
            finally
            {
                context.FinallyCall();
            }
        }

        /// <summary>
        /// Adds a DataTable named "Table" to the specified DataSet and configures the schema to match that in the data source based on the specified SchemaType of SchemaType.Source.
        /// </summary>        
        /// <returns>A reference to a collection of DataTable objects that were added to the DataSet.</returns>
        public DataTable[] ExecuteSetSchema()
        {
            return ExecuteSetSchema(null, SchemaType.Source);
        }

        /// <summary>
        /// Adds a DataTable to the specified DataSet and configures the schema to match that in the data source based upon the specified SchemaType and DataTable.
        /// </summary>
        /// <param name="dataSet">A DataSet to insert the schema in.</param>
        /// <param name="schemaType">One of the SchemaType values that specify how to insert the schema.</param>
        /// <param name="srcTable">The name of the source table to use for table mapping.</param>
        /// <returns>A reference to a collection of DataTable objects that were added to the DataSet.</returns>
        public DataTable[] ExecuteSetSchema(DataSet dataSet, SchemaType schemaType, string srcTable)
        {
            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "ExecuteSetSchema";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "dataSet", "schemaType", "srcTable", "CommandText", "Parameters" },
                        new object[] { "Null:" + (dataSet == null), schemaType, srcTable, this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                if (dataSet == null)
                    dataSet = new DataSet();
                if (string.IsNullOrEmpty(srcTable))
                    srcTable = "Table";
                                
                DataTable[] tables = DbDataAdapter.FillSchema(dataSet, schemaType, srcTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { "Null:" + (tables == null), GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return tables;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region UpdateDataTable
        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified DataTable. 
        /// </summary>
        /// <param name="dataTable">The DataTable used to update the data source.</param>
        /// <returns>The number of rows successfully updated from the DataTable.</returns>
        public int UpdateDataTable(DataTable dataTable)
        {
            return UpdateDataTable(dataTable, null, null, null);
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified DataTable. 
        /// </summary>
        /// <param name="dataTable">The DataTable used to update the data source.</param>
        /// <param name="insertInfo"></param>
        /// <param name="updateInfo"></param>
        /// <param name="deleteInfo"></param>
        /// <returns>The number of rows successfully updated from the DataTable.</returns>
        public virtual int UpdateDataTable(DataTable dataTable, DbAccessInformation insertInfo, DbAccessInformation updateInfo, DbAccessInformation deleteInfo)
        {
            if (dataTable == null)
                return -1;

            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "UpdateDataTable";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                DbCommandBuilder.SetAllValues = false;                
                if (insertInfo != null)
                    DbDataAdapter.InsertCommand = InnerAttachAccessInfo(insertInfo, this.Connection.CreateCommand());
                if (updateInfo != null)
                    DbDataAdapter.UpdateCommand = InnerAttachAccessInfo(updateInfo, this.Connection.CreateCommand());
                if (deleteInfo != null)
                    DbDataAdapter.DeleteCommand = InnerAttachAccessInfo(deleteInfo, this.Connection.CreateCommand());
                int result = DbDataAdapter.Update(dataTable);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region UpdateDataRow
        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified array of DataRow objects.
        /// </summary>
        /// <param name="dataRows">An array of DataRow objects used to update the data source. </param>        
        /// <returns>The number of rows successfully updated from the array of DataRow.</returns>
        public int UpdateDataRow(params DataRow[] dataRows)
        {
            return UpdateDataRow(dataRows, null, null, null);
        }

        /// <summary>
        /// Calls the respective INSERT, UPDATE, or DELETE statements for each inserted, updated, or deleted row in the specified array of DataRow objects.
        /// </summary>
        /// <param name="dataRows">An array of DataRow objects used to update the data source. </param>
        /// <param name="insertInfo"></param>
        /// <param name="updateInfo"></param>
        /// <param name="deleteInfo"></param>
        /// <returns>The number of rows successfully updated from the array of DataRow.</returns>
        public virtual int UpdateDataRow(DataRow[] dataRows, DbAccessInformation insertInfo, DbAccessInformation updateInfo, DbAccessInformation deleteInfo)
        {
            if (dataRows == null)
                return -1;

            ExecuteContext context = new ExecuteContext(this);
            try
            {
                #region TraceInfo
                const string TRACE_METHOD = "UpdateDataRow";
                if (IsTraced)
                {
                    Tracer.Instance.EnterFunction(TraceClass, TRACE_METHOD,
                        new string[] { "CommandText", "Parameters" },
                        new object[] { this.CommandText, GetTraceParameters(true) });
                }
                #endregion

                DbCommandBuilder.SetAllValues = false;                
                if (insertInfo != null)
                    DbDataAdapter.InsertCommand = InnerAttachAccessInfo(insertInfo, this.Connection.CreateCommand());
                if (updateInfo != null)
                    DbDataAdapter.UpdateCommand = InnerAttachAccessInfo(updateInfo, this.Connection.CreateCommand());
                if (deleteInfo != null)
                    DbDataAdapter.DeleteCommand = InnerAttachAccessInfo(deleteInfo, this.Connection.CreateCommand());
                int result = DbDataAdapter.Update(dataRows);

                #region TraceInfo
                if (IsTraced)
                {
                    Tracer.Instance.LeaveFunction(TraceClass, TRACE_METHOD,
                        new string[] { "result", "Parameters" },
                        new object[] { result, GetTraceParameters(false) });
                }
                #endregion

                context.SuccessfullyCall();
                return result;
            }
            finally
            {
                context.FinallyCall();
            }
        }
        #endregion

        #region AsyncExecuteNonQuery
        /// <summary>
        /// AsyncExecuteNonQuery
        /// </summary>
        protected Func<int> AsyncExecuteNonQuery;        

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteNonQuery.</returns>
        public virtual IAsyncResult BeginExecuteNonQuery(AsyncCallback callback, Object state)
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
                        
            AsyncExecuteNonQuery = ExecuteNonQuery;
            
            return AsyncExecuteNonQuery.BeginInvoke(callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand.
        /// </summary>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteNonQuery.</returns>
        public IAsyncResult BeginExecuteNonQuery()
        {
            return BeginExecuteNonQuery(null, null);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to BeginExecuteNonQuery.</param>
        /// <returns>The number of rows affected (the same behavior as ExecuteNonQuery).</returns>
        public virtual int EndExecuteNonQuery(IAsyncResult asyncResult)
        {
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteNonQuery", "AsyncExecuteNonQuery", AsyncExecuteNonQuery);
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteNonQuery", "asyncResult", asyncResult);
            
            int result = AsyncExecuteNonQuery.EndInvoke(asyncResult);
            AsyncExecuteNonQuery = null;

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
        #endregion

        #region AsyncExecuteReader
        /// <summary>
        /// AsyncExecuteReader
        /// </summary>
        protected Func<CommandBehavior,DbDataReader> AsyncExecuteReader;        

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <param name="behavior">One of the CommandBehavior values, indicating options for statement execution and data retrieval.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteReader.</returns>        
        public virtual IAsyncResult BeginExecuteReader(AsyncCallback callback, Object state, CommandBehavior behavior)
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

            AsyncExecuteReader = DbCommand.ExecuteReader;
            IAsyncResult asyncResult = AsyncExecuteReader.BeginInvoke(behavior, callback, state);

            return asyncResult;
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand.
        /// </summary>
        /// <param name="behavior">One of the CommandBehavior values, indicating options for statement execution and data retrieval.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteReader.</returns>        
        public IAsyncResult BeginExecuteReader(CommandBehavior behavior)
        {
            return BeginExecuteReader(null, null, behavior);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand.
        /// </summary>        
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteReader.</returns>        
        public IAsyncResult BeginExecuteReader()
        {
            CommandBehavior commandBehavior = DbCommand.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
            return BeginExecuteReader(commandBehavior);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>        
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteReader.</returns>        
        public IAsyncResult BeginExecuteReader(AsyncCallback callback, Object state)
        {
            CommandBehavior commandBehavior = DbCommand.Transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default;
            return BeginExecuteReader(callback, state, commandBehavior);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to EndExecuteReader.</param>
        /// <returns>A DbDataReader object that can be used to retrieve the requested rows (the same behavior as ExecuteReader).</returns>
        public virtual DbDataReader EndExecuteReader(IAsyncResult asyncResult)
        {            
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteReader", "AsyncExecuteReader", AsyncExecuteReader);
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteReader", "asyncResult", asyncResult);

            DbDataReader result = AsyncExecuteReader.EndInvoke(asyncResult);
            AsyncExecuteReader = null;

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
        #endregion

        #region AsyncExecuteXmlReader
        /// <summary>
        /// AsyncExecuteXmlReader
        /// </summary>
        protected Func<System.Xml.XmlReader> AsyncExecuteXmlReader;

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand, given a callback procedure and state information.
        /// </summary>
        /// <param name="callback">An AsyncCallback delegate that is invoked when the command's execution has completed. Pass null (Nothing in Microsoft Visual Basic) to indicate that no callback is required.</param>
        /// <param name="state">A user-defined state object that is passed to the callback procedure. Retrieve this object from within the callback procedure using the AsyncState property.</param>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteXmlReader.</returns>        
        public virtual IAsyncResult BeginExecuteXmlReader(AsyncCallback callback, Object state)
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

            AsyncExecuteXmlReader = ExecuteXmlReader;
            return AsyncExecuteXmlReader.BeginInvoke(callback, state);
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Transact-SQL statement or stored procedure that is described by this DBCommand.
        /// </summary>
        /// <returns>An IAsyncResult that can be used to poll or wait for results, or both; this value is also needed when invoking EndExecuteXmlReader.</returns>                
        public IAsyncResult BeginExecuteXmlReader()
        {
            return BeginExecuteXmlReader(null, null);
        }

        /// <summary>
        /// Finishes asynchronous execution of a Transact-SQL statement.
        /// </summary>
        /// <param name="asyncResult">The IAsyncResult returned by the call to EndExecuteReader.</param>
        /// <returns>An XmlReader object that can be used to fetch the resulting XML data (the same behavior as ExecuteXmlReader).</returns>
        public virtual System.Xml.XmlReader EndExecuteXmlReader(IAsyncResult asyncResult)
        {
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteXmlReader", "AsyncExecuteXmlReader", AsyncExecuteXmlReader);
            ParameterChecker.CheckNull("DbAccessCommand.EndExecuteXmlReader", "asyncResult", asyncResult);

            System.Xml.XmlReader result = AsyncExecuteXmlReader.EndInvoke(asyncResult);
            AsyncExecuteXmlReader = null;

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
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources. 
        /// </summary>
        public void Dispose()
        {
            this.Close();            

            if (_DbConnection != null)
            {
                _DbConnection.Dispose();
                _DbConnection = null;
            }

            if (_DbCommand != null)
            {
                _DbCommand.Dispose();
                _DbCommand = null;
            }

            if (_DbDataAdapter != null)
            {
                _DbDataAdapter.Dispose();
                _DbDataAdapter = null;
            }

            if (_DbCommandBuilder != null)
            {
                _DbCommandBuilder.Dispose();
                _DbCommandBuilder = null;
            }
        }

        #endregion

        #region TraceInfo
        /// <summary>
        /// GetTraceParameters
        /// </summary>
        /// <param name="isBegin"></param>
        /// <returns></returns>
        protected SortedList GetTraceParameters(bool isBegin)
        {
            SortedList parameters = new SortedList();            
            foreach (DbParameter parameter in DbCommand.Parameters)
            {                
                if(isBegin)
                    parameters.Add(parameter.ParameterName, parameter.Value);
                else if(parameter.Direction != ParameterDirection.Input)
                    parameters.Add(parameter.ParameterName, parameter.Value);
            }
            return parameters;
        }
        #endregion

        #region ExecuteContext
        /// <summary>
        /// ExecuteContext
        /// </summary>
        protected struct ExecuteContext
        {
            private bool NeedClose;
            private bool IsSuccessful;
            private DbAccessCommand DbCommand;            

            /// <summary>
            /// ExecuteContext
            /// </summary>
            /// <param name="dbCommand"></param>
            public ExecuteContext(DbAccessCommand dbCommand)
            {
                this.NeedClose = false;
                this.IsSuccessful = false;
                this.DbCommand = dbCommand;

                if (this.DbCommand.AutoOpenClose)
                {
                    if (this.DbCommand.Connection.State != ConnectionState.Open)
                    {
                        this.DbCommand.Open();
                        this.NeedClose = true;
                    }
                }
            }

            /// <summary>
            /// SuccessfullyCall
            /// </summary>
            public void SuccessfullyCall()
            {
                IsSuccessful = true;
            }

            /// <summary>
            /// FinallyCall
            /// </summary>
            public void FinallyCall()
            {
                if (this.DbCommand.AutoOpenClose)
                {
                    if (!this.IsSuccessful)
                        this.DbCommand.RollbackTransaction();

                    if (this.NeedClose || !this.IsSuccessful)
                        this.DbCommand.Close();
                }
            }
        }
        #endregion

        #region IDbCommand Members
        IDbDataParameter IDbCommand.CreateParameter()
        {
            return DbProviderFactory.CreateParameter();
        }

        IDbTransaction IDbCommand.Transaction
        {
            get
            {
                return this.Transaction;
            }
            set
            {
                this.Transaction = value as DbTransaction;
            }
        }

        IDbConnection IDbCommand.Connection
        {
            get
            {
                return this.Connection;
            }
            set
            {
                this.Connection = value as DbConnection;
            }
        }

        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
        {
            return DbCommand.ExecuteReader(behavior);
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            return DbCommand.ExecuteReader();
        }

        IDataParameterCollection IDbCommand.Parameters
        {
            get { return this.Parameters; }
        }

        #endregion
    }
}
