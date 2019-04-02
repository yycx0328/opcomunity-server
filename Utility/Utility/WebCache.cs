using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// WebCache
/// </summary>
public static class WebCache
{
    /// <summary>
    /// 一般配置Chche过期时间，秒钟计
    /// </summary>
    /// <value>The common cache time out.</value>
    public static int CommonCacheTimeOut
    {
        get
        {
            if (_CommonCacheTimeOut == null)
            {
                string settings = ConfigurationManager.AppSettings["CommonCacheTimeOut"];
                int timeOut;
                if (!int.TryParse(settings, out timeOut))
                    timeOut = 1200;   //默认20分钟过期
                _CommonCacheTimeOut = timeOut;
            }

            return _CommonCacheTimeOut.Value;
        }
    }
    private static int? _CommonCacheTimeOut;

    /// <summary>
    /// 查询配置Chche过期时间，秒钟计
    /// </summary>
    /// <value>The query cache time out.</value>
    public static int QueryCacheTimeOut
    {
        get
        {
            if (_QueryCacheTimeOut == null)
            {
                string settings = ConfigurationManager.AppSettings["QueryCacheTimeOut"];
                int timeOut;
                if (!int.TryParse(settings, out timeOut))
                    timeOut = 10;   //默认10秒钟过期
                _QueryCacheTimeOut = timeOut;
            }

            return _QueryCacheTimeOut.Value;
        }
    }
    private static int? _QueryCacheTimeOut;

    /// <summary>
    /// Sql数据库缓存依赖项
    /// </summary>
    private static List<string> SqlCacheDependencyItems = new List<string>();

    /// <summary>
    /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="timeOutSeconds">单位：秒</param>
    /// <param name="onCreateInstance">The on create instance.</param>
    /// <returns></returns>
    public static T GetCachedObject<T>(string key, int timeOutSeconds, Func<T> onCreateInstance)
    {
        return GetCachedObject<T>(key, null, timeOutSeconds, onCreateInstance);
    }

    /// <summary>
    /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="onCreateInstance">The on create instance.</param>
    /// <returns></returns>
    public static T GetCachedObject<T>(string key, Func<T> onCreateInstance)
    {
        return GetCachedObject<T>(key, null, CommonCacheTimeOut, onCreateInstance);
    }

    /// <summary>
    /// 获取二级缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key1">第一级缓存健</param>
    /// <param name="key2">第二级缓存健</param>
    /// <param name="onCreateInstance"></param>
    /// <returns></returns>
    public static T GetCachedObject<T>(string key1, string key2, Func<T> onCreateInstance) where T : class, new()
    {
        Dictionary<string, T> dictionary = GetCachedObject<Dictionary<string, T>>(key1, null, CommonCacheTimeOut,
            delegate
            {
                return new Dictionary<string, T>();
            });

        T instance = null;
        if (!dictionary.TryGetValue(key2, out instance))
        {
            if (onCreateInstance == null)
                instance = new T();
            else
                instance = onCreateInstance();

            dictionary[key2] = instance;
        }

        return instance;
    }

    /// <summary>
    /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="databaseEntryName"></param>
    /// <param name="tableName"></param>
    /// <param name="onCreateInstance"></param>
    /// <returns></returns>
    public static T GetCachedObject<T>(string key, string databaseEntryName, string tableName, Func<T> onCreateInstance)
    {
        if (!SqlCacheDependencyItems.Contains(databaseEntryName))
        {
            System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(GetConnectionString(databaseEntryName));
            SqlCacheDependencyItems.Add(databaseEntryName);
            SqlCacheDependencyItems.Sort();
        }
        if (!SqlCacheDependencyItems.Contains(databaseEntryName + '#' + tableName))
        {
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(GetConnectionString(databaseEntryName), tableName);
            SqlCacheDependencyItems.Add(databaseEntryName + '#' + tableName);
            SqlCacheDependencyItems.Sort();
        }

        System.Web.Caching.SqlCacheDependency dependency = new System.Web.Caching.SqlCacheDependency(databaseEntryName, tableName);
        return GetCachedObject<T>(key, dependency, 0, onCreateInstance);
    }

    /// <summary>
    /// 获取缓存的查询。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="onCreateInstance">The on create instance.</param>
    /// <returns></returns>
    public static T GetCachedQuery<T>(string key, Func<T> onCreateInstance)
    {
        return GetCachedObject<T>(key, null, QueryCacheTimeOut, onCreateInstance);
    }

    /// <summary>
    /// 移除缓存的对象
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static object Remove(string key)
    {
        //当前Cache对象
        System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        return webCache.Remove(key);
    }

    /// <summary>
    /// 清除所有缓存
    /// </summary>
    public static void RemoveAll()
    {
        //当前Cache对象           
        var cache = System.Web.HttpRuntime.Cache;
        foreach (DictionaryEntry de in cache.Cast<DictionaryEntry>())
        {
            string key = de.Key as string;
            cache.Remove(key);
        }
    }

    /// <summary>
    /// 获取缓存的对象。当没有缓存的时候，自动创建对象并进行缓存。只支持引用类型的缓存。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="dependency"></param>
    /// <param name="timeOutSeconds">单位：秒</param>
    /// <param name="onCreateInstance"></param>
    /// <returns></returns>
    private static T GetCachedObject<T>(string key, System.Web.Caching.CacheDependency dependency, int timeOutSeconds, Func<T> onCreateInstance)
    {
        if (timeOutSeconds > 0 || dependency != null)
        {
            //当前Cache对象
            System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;
            if (webCache == null)
                return onCreateInstance();

            //获取缓存的对象
            T cachedObject = (T)webCache.Get(key);

            if (cachedObject == null)
            {
                //创建新的对象
                cachedObject = onCreateInstance();

                //将创建的对象进行缓存
                if (cachedObject != null)
                {
                    webCache.Insert(key, cachedObject, dependency, DateTime.Now.AddSeconds(timeOutSeconds), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            return cachedObject;
        }
        else
        {
            //不设置缓存，则创建新的对象
            return onCreateInstance();
        }
    }

    /// <summary>
    /// 获取数据库连接字符串
    /// </summary>
    /// <param name="connectionEntryName"></param>
    /// <returns></returns>
    private static string GetConnectionString(string connectionEntryName)
    {
        string connString = System.Configuration.ConfigurationManager.AppSettings[connectionEntryName];
        if (!string.IsNullOrEmpty(connString))
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[connString] != null)
                return System.Configuration.ConfigurationManager.ConnectionStrings[connString].ConnectionString;
            else
                return connString;
        }
        else
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings[connectionEntryName].ConnectionString;
        }
    }
}
