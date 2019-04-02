using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring.Context;
using Spring.Context.Support;

namespace DI
{
    public class SpringHelper
    {
        #region Sping.NET容器上下文
        /// <summary>
        /// Sping.NET容器上下文
        /// </summary>
        private static IApplicationContext SpringContext
        {
            get
            {
                return ContextRegistry.GetContext();
            }
        }
        #endregion

        #region 使用Sping.NET操作配置文件并转换成对象
        /// <summary>
        /// 使用Sping.NET操作配置文件并转换成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objName"></param>
        /// <returns></returns>
        public static T GetObject<T>(string objName) where T : class
        {
            T o = (T)SpringContext.GetObject(objName);
            return o;
        } 
        #endregion
    }
}
