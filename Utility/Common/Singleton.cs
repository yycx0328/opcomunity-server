using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Utility
{
    /// <summary>
    /// Singleton
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class Singleton<T>
    {
        /// <summary>
        /// LockKey
        /// </summary>
        private static object LockKey = new object();

        /// <summary>
        /// ÊµÀý
        /// </summary>
        private static T _Instance;

        /// <summary>
        /// Get an instance of T
        /// </summary>
        /// <returns></returns>
        public static T GetInstance()
        {
            return GetInstance(null);
        }

        /// <summary>
        /// Get an instance of T
        /// </summary>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetInstance(Func<T> onCreateInstance)
        {
            if (_Instance == null)
            {
                lock (LockKey)
                {
                    if (_Instance == null)
                    {
                        _Instance = TryGetInstance(onCreateInstance);
                    }
                }
            }
            return _Instance;
        }

        /// <summary>
        /// Get an instance of T and set to instance
        /// </summary>        
        /// <param name="instance">The instance.</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetInstance(T instance, Func<T> onCreateInstance)
        {
            if (instance == null)
            {                
                lock (LockKey)
                {
                    if (instance == null)
                    {
                        instance = TryGetInstance(onCreateInstance);
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Get an instance of T
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="onCreateInstance">The on create instance.</param>
        /// <returns></returns>
        public static T GetInstance(Dictionary<string, T> dictionary, string key, Func<T> onCreateInstance)
        {
            if (dictionary == null)
                dictionary = new Dictionary<string, T>();

            T instance;
            if (dictionary.TryGetValue(key, out instance))
                return instance;

            lock (LockKey)
            {
                if (dictionary.TryGetValue(key, out instance))
                    return instance;

                instance = TryGetInstance(onCreateInstance);

                dictionary[key] = instance;
                return instance;
            }
        }

        /// <summary>
        /// Release the instance of T
        /// </summary>
        public static void ReleaseInstance()
        {
            lock (LockKey)
            {
                IDisposable id = _Instance as IDisposable;
                if (id != null)
                    id.Dispose();

                _Instance = default(T);
            }
        }

        private static T TryGetInstance(Func<T> onCreateInstance)
        {
            try
            {
                if (onCreateInstance == null)
                    return Activator.CreateInstance<T>();
                else
                    return onCreateInstance();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
