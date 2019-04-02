using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common; 

namespace Utility.DataAccess
{
    /// <summary>
    /// Represents a parameter to a DbAccessCommand
    /// </summary>
    [Serializable]
    public sealed class DbAccessParameter : MarshalByRefObject
    {
        static DbAccessParameter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        public DbAccessParameter()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName)
            : this()
        {
            ParameterChecker.CheckNullOrEmpty("DbAccessParameter", "parameterName", parameterName);
            
            this.ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameter">Another DbParameter object.</param>
        public DbAccessParameter(DbParameter parameter)
            : this()
        {
            ParameterChecker.CheckNull("DbAccessParameter", "parameter", parameter);
            ParameterChecker.CheckNullOrEmpty("DbAccessParameter", "parameter.ParameterName", parameter.ParameterName);            
            
            this.ParameterName = parameter.ParameterName;
            this.DbType = parameter.DbType;
            this.Size = parameter.Size;
            this.Direction = parameter.Direction;
            this.IsNullable = parameter.IsNullable;
            this.Precision = ((IDbDataParameter)parameter).Precision;
            this.Scale = ((IDbDataParameter)parameter).Scale;
            this.SourceColumn = parameter.SourceColumn;
            this.SourceVersion = parameter.SourceVersion;
            this.Value = parameter.Value;
            this.SourceColumnNullMapping = parameter.SourceColumnNullMapping;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName, object parameterValue)
            : this(parameterName)
        {
            this.Value = parameterValue;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName, DbType dbType)
            : this(parameterName)
        {
            this.DbType = dbType;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter.</param>
        /// <param name="size">The size of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName, DbType dbType, int size)
            : this(parameterName, dbType)
        {
            if(size > 0)
                this.Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter.</param>
        /// <param name="parameterDirection">The Direction of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName, object parameterValue, DbType dbType, ParameterDirection parameterDirection)
            : this(parameterName, dbType)
        {
            this.Value = parameterValue;
            this.Direction = parameterDirection;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter.</param>
        /// <param name="parameterValue">The value of the DbAccessParameter.</param>
        /// <param name="dbType">The DbType of the DbAccessParameter.</param>
        /// <param name="size">The size of the DbAccessParameter.</param>
        /// <param name="parameterDirection">The direction of the DbAccessParameter.</param>
        public DbAccessParameter(string parameterName, object parameterValue, DbType dbType, int size, ParameterDirection parameterDirection)
            : this(parameterName, dbType, size)
        {
            this.Value = parameterValue;
            this.Direction = parameterDirection;
        }

        /// <summary>
        /// Initializes a new instance of the DbAccessParameter class. 
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
        public DbAccessParameter(string parameterName, int providerType, int size, ParameterDirection direction, bool isNullable, byte precision, byte scale, string srcColumn, DataRowVersion srcVersion, Object value)
            : this(parameterName)
        {
            this.ProvideType = providerType;
            this.Size = size;
            this.Direction = direction;
            this.IsNullable = isNullable;
            this.Precision = precision;
            this.Scale = scale;
            this.SourceColumn = srcColumn;
            this.SourceVersion = srcVersion;
            this.Value = value;
        }
        
        /// <summary>
        /// Gets or sets the DbType of the parameter.
        /// </summary>
        public DbType DbType
        {
            get
            {
                return _DbType;
            }
            set
            {
                _DbType = value;
            }
        }
        private DbType _DbType = DbType.String;

        /// <summary>
        /// Gets or sets the ProvideType of the parameter.
        /// </summary>
        public int ProvideType
        {
            get
            {
                return _ProvideType;
            }
            set
            {
                _ProvideType = value;
            }
        }
        private int _ProvideType = -1;

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// </summary>
        public ParameterDirection Direction
        {
            get
            {
                return _Direction;
            }
            set
            {
                _Direction = value;
            }
        }
        private ParameterDirection _Direction = ParameterDirection.Input;

        /// <summary>
        /// Gets or sets the name of the DbAccessParameter.
        /// </summary>
        public string ParameterName
        {
            get
            {
                return _ParameterName;         
            }
            set
            {
                _ParameterName = value;
            }
        }
        private string _ParameterName = string.Empty;

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        public int Size
        {
            get
            {
                return _Size;
            }
            set
            {
                _Size = value;
            }
        }
        private int _Size = 0;

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = (value ?? DBNull.Value);                
                    
            }
        }
        private object _Value = DBNull.Value;

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter accepts null values.
        /// </summary>
        public bool IsNullable
        {
            get
            {
                return _IsNullable;
            }
            set
            {
                _IsNullable = value;
            }
        }
        private bool _IsNullable = false;

        /// <summary>
        /// Gets or sets the name of the source column mapped to the DataSet and used for loading or returning the Value.
        /// </summary>
        public string SourceColumn
        {
            get
            {
                return _SourceColumn;
            }
            set
            {
                _SourceColumn = value;
            }
        }
        private string _SourceColumn = string.Empty;

        /// <summary>
        /// Sets or gets a value which indicates whether the source column is nullable. This allows DbCommandBuilder to correctly generate Update statements for nullable columns.
        /// </summary>
        public bool SourceColumnNullMapping
        {
            get
            {
                return _SourceColumnNullMapping;
            }
            set
            {
                _SourceColumnNullMapping = value;
            }
        }
        private bool _SourceColumnNullMapping = false;

        /// <summary>
        /// Gets or sets the DataRowVersion to use when you load Value.
        /// </summary>
        public DataRowVersion SourceVersion
        {
            get
            {
                return _SourceVersion;
            }
            set
            {
                _SourceVersion = value;
            }
        }
        private DataRowVersion _SourceVersion = DataRowVersion.Current;

        /// <summary>
        /// Gets or sets the maximum number of digits used to represent the Value property. 
        /// </summary>
        public byte Precision
        {
            get
            {
                return _Precision;
            }
            set
            {
                _Precision = value;
            }
        }
        private byte _Precision = byte.MinValue;

        /// <summary>
        /// Gets or sets the number of decimal places to which Value is resolved. 
        /// </summary>
        public byte Scale
        {
            get
            {
                return _Scale;
            }
            set
            {
                _Scale = value;
            }
        }
        private byte _Scale = byte.MinValue;

        /// <summary>
        /// Resets the DbType property to its original settings. 
        /// </summary>
        public void ResetDbType()
        {
            this.DbType = DbType.String;                
        }
    }
}
