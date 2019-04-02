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
    /// A collection of parameters relevant to a DbAccessCommand. 
    /// </summary>
    [Serializable]
    public sealed class DbAccessParameterCollection : MarshalByRefObject, IDataParameterCollection
    {
        static DbAccessParameterCollection()
        {   
        }

        private List<DbAccessParameter> Parameters;

        /// <summary>
        /// Initializes a new instance of the DbAccessParameterCollection class.
        /// </summary>
        public DbAccessParameterCollection()
            : base()
        {
            Parameters = new List<DbAccessParameter>();
        }

        /// <summary>
        /// Adds a DbAccessParameter item with the specified value to the DbAccessParameterCollection.
        /// </summary>
        /// <param name="value">The Value of the DbAccessParameter to add to the collection.</param>        
        public void Add(DbAccessParameter value)
        {
            Parameters.Add(value);
        }

        /// <summary>
        /// Adds an array of items with the specified values to the DbAccessParameterCollection.
        /// </summary>
        /// <param name="values">An array of values of type DbAccessParameter to add to the collection.</param>
        public void AddRange(params DbAccessParameter[] values)
        {
            if (values != null && values.Length > 0)
                Parameters.AddRange(values);
        }

        /// <summary>
        /// Indicates whether a DbAccessParameter with the specified Value is contained in the collection. 
        /// </summary>
        /// <param name="value">The Value of the DbAccessParameter to look for in the collection.</param>
        /// <returns>true if the DbAccessParameter is in the collection; otherwise false.</returns>
        public bool Contains(DbAccessParameter value)
        {
            return Parameters.Contains(value);
        }

        /// <summary>
        /// Indicates whether a DbAccessParameter with the specified name exists in the collection.
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter to look for in the collection.</param>
        /// <returns>true if the DbAccessParameter is in the collection; otherwise false.</returns>
        public bool Contains(string parameterName)
        {            
            return (IndexOf(parameterName) >= 0);
        }

        /// <summary>
        /// Returns the index of the specified DbAccessParameter object.
        /// </summary>
        /// <param name="value">The DbAccessParameter object in the collection.</param>
        /// <returns>The index of the specified DbAccessParameter object.</returns>
        public int IndexOf(DbAccessParameter value)
        {
            return Parameters.IndexOf(value);
        }

        /// <summary>
        /// Returns the index of the DbAccessParameter object with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter object in the collection.</param>
        /// <returns>The index of the DbAccessParameter object with the specified name.</returns>
        public int IndexOf(string parameterName)
        {            
            for (int i = 0, j = Parameters.Count; i < j; i++)
            {
                if (string.Equals(parameterName, Parameters[i].ParameterName, StringComparison.InvariantCultureIgnoreCase))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Inserts the specified the index of the DbAccessParameter object with the specified name into the collection at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the DbAccessParameter object.</param>
        /// <param name="value">The DbAccessParameter object to insert into the collection.</param>
        public void Insert(int index, DbAccessParameter value)
        {
            Parameters.Insert(index, value);
        }

        /// <summary>
        /// Removes the specified DbAccessParameter object from the collection.
        /// </summary>
        /// <param name="value">The DbAccessParameter object to remove.</param>
        public void Remove(DbAccessParameter value)
        {
            Parameters.Remove(value);
        }

        /// <summary>
        /// Removes the DbParameter object at the specified from the collection. 
        /// </summary>
        /// <param name="index">The index where the DbParameter object is located.</param>
        public void RemoveAt(int index)
        {
            Parameters.RemoveAt(index);
        }

        /// <summary>
        /// Removes the DbAccessParameter object with the specified name from the collection.
        /// </summary>
        /// <param name="parameterName">The name of the DbAccessParameter object to remove.</param>
        public void RemoveAt(string parameterName)
        {
            int index = IndexOf(parameterName);
            if (index >= 0)
                Parameters.RemoveAt(index);
        }

        /// <summary>
        /// Copies an array of items to the collection starting.
        /// </summary>
        /// <returns>An array of items to the collection.</returns>
        public DbAccessParameter[] ToArray()
        {
            return Parameters.ToArray();
        }

        /// <summary>
        /// Gets and sets the DbParameter at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the parameter.</param>
        /// <returns>The DbParameter at the specified index.</returns>
        public DbAccessParameter this[int index]
        {
            get { return Parameters[index]; }
            set { Parameters[index] = value; }
        }

        /// <summary>
        /// Gets and sets the DbParameter with the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>The DbParameter with the specified name, or a null reference if the parameter is not found.</returns>
        public DbAccessParameter this[string parameterName]
        {
            get
            {
                int index = IndexOf(parameterName);
                return (index >= 0 ? this[index] : null);
            }
            set
            {
                int index = IndexOf(parameterName);
                if (index >= 0)
                    Parameters[index] = value;
                else
                    Parameters.Add(value);
            }
        }

        /// <summary>
        /// Removes all DbParameter values from the DbParameterCollection.
        /// </summary>
        public void Clear()
        {
            Parameters.Clear();
        }

        /// <summary>
        /// Specifies the number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return Parameters.Count; }
        }

        /// <summary>
        /// Exposes the GetEnumerator method, which supports a simple iteration over a collection by a .NET Framework data provider. 
        /// </summary>
        /// <returns>An IEnumerator that can be used to iterate through the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }
        
        #region IDataParameterCollection Members

        bool IDataParameterCollection.Contains(string parameterName)
        {
            return this.Contains(parameterName);
        }

        int IDataParameterCollection.IndexOf(string parameterName)
        {
            return this.IndexOf(parameterName);
        }

        void IDataParameterCollection.RemoveAt(string parameterName)
        {
            this.RemoveAt(parameterName);
        }

        object IDataParameterCollection.this[string parameterName]
        {
            get
            {
                return this[parameterName];
            }
            set
            {
                this[parameterName] = value as DbAccessParameter;
            }
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            this.Add(value as DbAccessParameter);
            return this.Count;
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            return this.Contains(value as DbAccessParameter);
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf(value as DbAccessParameter);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, value as DbAccessParameter);
        }

        bool IList.IsFixedSize
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        bool IList.IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        void IList.Remove(object value)
        {
            this.Remove(value as DbAccessParameter);
        }

        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this[index] = value as DbAccessParameter;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            this.Parameters.CopyTo((DbAccessParameter[])array, index);
        }

        int ICollection.Count
        {
            get { return this.Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        object ICollection.SyncRoot
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}
