using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Xml;
namespace Utility.DataAccess
{
    /// <summary>
    /// DbAccessor
    /// </summary>
    public sealed class DbAccessor : IDisposable
    {

        #region Properties
        private DbAccessCommand DbCommand { get; set; }       

        /// <summary>
        /// Gets or sets the string used to open the connection.
        /// </summary>
        public string ConnectionString
        {
            get { return DbCommand.ConnectionString; }
            set { DbCommand.ConnectionString = value; }
        }
        #endregion        
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbCommand"></param>
        public DbAccessor(DbAccessCommand dbCommand)
        {
            this.DbCommand = dbCommand;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="providerName">The providerName of connectionStrings section</param>
        /// <param name="connectionString">The connectionString of connectionStrings section</param>
        public DbAccessor(string providerName, string connectionString)
            : this(new DefaultAccessCommand(providerName, connectionString))
        {            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connSettings"></param>
        public DbAccessor(System.Configuration.ConnectionStringSettings connSettings)
            : this(new DefaultAccessCommand(connSettings))
        {
        }

        #region DB opration
        /// <summary>
        /// Open
        /// </summary>
        public void Open()
        {
            DbCommand.Open();
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Close()
        {
            DbCommand.Close();
        }

        /// <summary>
        /// BeginTransaction
        /// </summary>
        public void BeginTransaction()
        {
            DbCommand.BeginTransaction();
        }

        /// <summary>
        /// CommitTransaction
        /// </summary>
        public void CommitTransaction()
        {
            DbCommand.CommitTransaction();
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return -1;

            int result = 0;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteNonQuery();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }        
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// ExecuteScalar
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public object ExecuteScalar(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            object result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteScalar();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteXmlReader
        /// <summary>
        /// ExecuteXmlReader
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public System.Xml.XmlReader ExecuteXmlReader(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            System.Xml.XmlReader result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteXmlReader();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="readerHandler"></param>
        /// <returns></returns>
        public object ExecuteReader(DbAccessInformation accessInfo, Func<DbDataReader, object> readerHandler)
        {
            if (accessInfo == null || readerHandler == null)
                return null;
            
            DbCommand.AttachAccessInfo(accessInfo);
            object result = DbCommand.ExecuteReader(readerHandler);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteReader
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="readerHandler"></param>
        /// <param name="startRecord"></param>
        /// <returns></returns>
        public object ExecuteReader(DbAccessInformation accessInfo, Func<DbDataReader, object> readerHandler, int startRecord)
        {
            if (accessInfo == null || readerHandler == null)
                return null;
                        
            DbCommand.AttachAccessInfo(accessInfo);
            object result = DbCommand.ExecuteReader(readerHandler, startRecord);            
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteSchemaTable
        /// <summary>
        /// ExecuteSchemaTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataTable ExecuteSchemaTable(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataTable result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteSchemaTable();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteDataTable
        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataTable result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataTable();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbAccessInformation accessInfo, int startRecord, int maxRecords)
        {
            if (accessInfo == null)
                return null;

            DataTable result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataTable(startRecord, maxRecords);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int ExecuteDataTable(DbAccessInformation accessInfo, DataTable dataTable)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataTable(dataTable);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataTable"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <returns></returns>
        public int ExecuteDataTable(DbAccessInformation accessInfo, DataTable dataTable, int startRecord, int maxRecords)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataTable(dataTable, startRecord, maxRecords);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteDataRow
        /// <summary>
        /// ExecuteDataRow
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataRow ExecuteDataRow(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataRow result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataRow();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataRow
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public DataRow ExecuteDataRow(DbAccessInformation accessInfo, DataTable dataTable)
        {
            if (accessInfo == null)
                return null;

            DataRow result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataRow(dataTable);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteTableSchema
        /// <summary>
        /// ExecuteTableSchema
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataTable ExecuteTableSchema(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataTable result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteTableSchema();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataSet result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataSet();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataSet"></param>
        /// <param name="startRecord"></param>
        /// <param name="maxRecords"></param>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        public int ExecuteDataSet(DbAccessInformation accessInfo, DataSet dataSet, int startRecord, int maxRecords, string srcTable)
        {
            if (accessInfo == null)
                return 0;

            int result = 0;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataSet(dataSet, startRecord, maxRecords, srcTable);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataSet"></param>
        /// <returns></returns>
        public int ExecuteDataSet(DbAccessInformation accessInfo, DataSet dataSet)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataSet(dataSet);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }

        /// <summary>
        /// ExecuteDataSet
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataSet"></param>
        /// <param name="srcTable"></param>
        /// <returns></returns>
        public int ExecuteDataSet(DbAccessInformation accessInfo, DataSet dataSet, string srcTable)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteDataSet(dataSet, srcTable);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteSetSchema
        /// <summary>
        /// ExecuteSetSchema
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public DataTable[] ExecuteSetSchema(DbAccessInformation accessInfo)
        {
            if (accessInfo == null)
                return null;

            DataTable[] result = null;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.ExecuteSetSchema();
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region ExecuteCommand
        /// <summary>
        /// ExecuteCommand
        /// </summary>
        /// <param name="commandHandler"></param>
        /// <returns></returns>
        public object ExecuteCommand(Func<DbAccessCommand, object> commandHandler)
        {
            if (commandHandler == null)
                return null;

            object result = null;

            if (commandHandler != null)
                result = commandHandler(DbCommand);

            return result;
        }
        #endregion

        #region ExecuteEntitySet
        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="accessInfo"></param>
        /// <param name="mappings"></param>
        /// <param name="startRecord"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>(DbAccessInformation accessInfo, INameMappings mappings, int startRecord) where TEntity : new()
        {
            if (accessInfo == null)
                return null;

            DbCommand.AttachAccessInfo(accessInfo);
            IEnumerable<TEntity> result = DbCommand.ExecuteEntitySet<TEntity>(mappings, startRecord);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }        

        /// <summary>
        /// ExecuteEntitySet
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="accessInfo"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteEntitySet<TEntity>(DbAccessInformation accessInfo) where TEntity : new()
        {
            return ExecuteEntitySet<TEntity>(accessInfo, null, 0);
        }
        #endregion

        #region UpdateDataTable
        /// <summary>
        /// UpdateDataTable
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public int UpdateDataTable(DbAccessInformation accessInfo, DataTable dataTable)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.UpdateDataTable(dataTable);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region UpdateDataRow
        /// <summary>
        /// UpdateDataRow
        /// </summary>
        /// <param name="accessInfo"></param>
        /// <param name="dataRows"></param>
        /// <returns></returns>
        public int UpdateDataRow(DbAccessInformation accessInfo, params DataRow[] dataRows)
        {
            if (accessInfo == null)
                return -1;

            int result = -1;

            DbCommand.AttachAccessInfo(accessInfo);
            result = DbCommand.UpdateDataRow(dataRows);
            DbCommand.PickupParameteValues(accessInfo);

            return result;
        }
        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (DbCommand != null)
                DbCommand.Dispose();
        }

        #endregion
    }
}
