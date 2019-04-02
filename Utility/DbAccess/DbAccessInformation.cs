using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Utility.DataAccess
{
    /// <summary>
    /// A DbAccessInformation object relevant to a DbAccessCommand. 
    /// </summary>
    [Serializable]
    public sealed class DbAccessInformation : MarshalByRefObject
    {
        static DbAccessInformation()
        {
        }

        /// <summary>
        /// Gets or sets the Transact-SQL statement, table name or stored procedure to execute at the data source. 
        /// </summary>
        public string CommandText 
        {
            set { _CommandText = value; } 
            get { return _CommandText; } 
        }
        private string _CommandText = null;

        /// <summary>
        /// Gets or sets a value indicating how the CommandText property is to be interpreted. 
        /// </summary>
        public CommandType CommandType 
        {
            set { _CommandType = value; } 
            get { return _CommandType; } 
        }
        private CommandType _CommandType = CommandType.Text;

        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to execute a command and generating an error. 
        /// </summary>
        public int CommandTimeout 
        {
            set { _CommandTimeout = value; } 
            get { return _CommandTimeout; } 
        }
        private int _CommandTimeout = 30;

        /// <summary>
        /// Gets or sets the DbAccessParameterCollection.
        /// </summary>
        public DbAccessParameterCollection Parameters
        {
            set { _Parameters = value; }
            get { return _Parameters; }
        }
        private DbAccessParameterCollection _Parameters = new DbAccessParameterCollection();

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        public DbAccessInformation()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement, table name or stored procedure to execute at the data source.</param>
        /// <param name="commandType">A value indicating how the CommandText property is to be interpreted.</param>
        /// <param name="commandTimeout">The wait time before terminating the attempt to execute a command and generating an error. </param>
        public DbAccessInformation(string commandText, CommandType commandType, int commandTimeout)
            : this()
        {
            ParameterChecker.CheckNullOrEmpty("DbAccessInformation", "commandText", commandText);

            this._CommandText = commandText;
            this._CommandType = commandType;
            if(commandTimeout > 0)
                this._CommandTimeout = commandTimeout;            
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement, table name or stored procedure to execute at the data source.</param>
        /// <param name="commandType">A value indicating how the CommandText property is to be interpreted.</param>        
        public DbAccessInformation(string commandText, CommandType commandType)
            : this(commandText, commandType, 30)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="commandText">The Transact-SQL statement, table name or stored procedure to execute at the data source.</param>        
        public DbAccessInformation(string commandText)
            : this(commandText, CommandType.Text)
        {
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameter">The DbAccessParameter to add to the collection.</param>
        public void AddParameter(DbAccessParameter parameter)
        {
            ParameterChecker.CheckNull("DbAccessInformation.AddParameter", "parameter", parameter);
            
            _Parameters.Add(parameter);
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameter">The DbParameter to add to the collection. </param>
        public void AddParameter(DbParameter parameter)
        {
            _Parameters.Add(new DbAccessParameter(parameter));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to add to the collection.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter to add to the collection.</param>
        public void AddParameter(string parameterName, object parameterValue)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, parameterValue));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to add to the collection.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter to add to the collection.</param>
        public void AddParameter(string parameterName, DbType dbType)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, dbType));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to add to the collection.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter to add to the collection.</param>
        /// <param name="size">The size of the DbAccessParameter to add to the collection.</param>
        public void AddParameter(string parameterName, DbType dbType, int size)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, dbType, size));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to add to the collection.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter to add to the collection.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter to add to the collection.</param>
        /// <param name="parameterDirection">The direction of the DbAccessParameter to add to the collection.</param>
        public void AddParameter(string parameterName, object parameterValue, DbType dbType, ParameterDirection parameterDirection)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, parameterValue, dbType, parameterDirection));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to add to the collection.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter to add to the collection.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter to add to the collection.</param>
        /// <param name="size">The size of the DbAccessParameter to add to the collection.</param>
        /// <param name="parameterDirection">The direction of the DbAccessParameter to add to the collection.</param>
        public void AddParameter(string parameterName, object parameterValue, DbType dbType, int size, ParameterDirection parameterDirection)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, parameterValue, dbType, size, parameterDirection));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
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
        public void AddParameter(string parameterName, int providerType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, Object value)
        {
            _Parameters.Add(new DbAccessParameter(parameterName, providerType, size, direction, isNullable, precision, scale, srcColumn, srcVersion, value));
        }

        /// <summary>
        /// Adds the specified DbAccessParameter object to the Parameters. 
        /// </summary>
        /// <param name="entity"></param>
        public void AddParameters(object entity)
        {
            if (entity == null)
                return;

            var properties = entity.GetType().GetProperties().Where(p => p.CanRead && (p.GetIndexParameters().Length == 0));
            foreach (var pi in properties)
            {
                this.AddParameter(pi.Name, ReflectionHelper.GetProperty(entity, pi));
            }
        }

        /// <summary>
        /// Find the value of the DbAccessParameter by the name of the DbAccessParameter.
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <returns>The value of the DbAccessParameter to return.</returns>
        public object GetParameterValue(string parameterName)
        {
            DbAccessParameter parameter = _Parameters[parameterName];

            return (parameter == null ? null : parameter.Value);
        }

        /// <summary>
        /// Find the value of the DbAccessParameter by the index of the Parameters.
        /// </summary>
        /// <param name="parameterIndex">The index of the Parameters.</param>
        /// <returns>The value of the DbAccessParameter to return.</returns>
        public object GetParameterValue(int parameterIndex)
        {
            DbAccessParameter parameter = _Parameters[parameterIndex];

            return (parameter == null ? null : parameter.Value);
        }

        /// <summary>
        /// Find the ReturnValue.
        /// </summary>
        /// <returns>The value of the DbAccessParameter to return.</returns>
        public object GetReturnValue()
        {
            foreach (DbAccessParameter parameter in this.Parameters)
            {
                if (parameter.Direction == ParameterDirection.ReturnValue)
                    return parameter.Value;
            }
            return null;
        }
    }
}
