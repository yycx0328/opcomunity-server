using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Tracer
    /// </summary>
    public sealed class Tracer
    {
        /// <summary>
        /// A static instance of Tracer
        /// </summary>
        public static Tracer Instance
        {
            get
            {
                return Singleton<Tracer>.GetInstance();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Tracer()
        {
            TraceItems = new Dictionary<string, bool>();
        }

        /// <summary>
        /// TraceObjectConverter
        /// </summary>
        public Converter<TraceObject, SortedList> TraceObjectConverter { get; set; }
        /// <summary>
        /// TracerProxy
        /// </summary>
        public DataProcessProxy<SortedList> TracerProxy { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// TraceItems
        /// </summary>
        public Dictionary<string, bool> TraceItems { get; private set; }

        /// <summary>
        /// AddTraceItems
        /// </summary>
        /// <param name="types"></param>
        public void AddTraceItems(params Type[] types)
        {
            foreach (Type type in types)
            {
                TraceItems[type.FullName] = true;
            }
        }

        /// <summary>
        /// AddTraceItems
        /// </summary>
        /// <param name="types"></param>
        public void AddTraceItems(params string[] types)
        {
            foreach (string type in types)
            {
                TraceItems[type] = true;
            }
        }   

        /// <summary>
        /// IsTraced
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsTraced(Type type)
        {
            while (type != null)
            {
                bool isTraced;
                if (TraceItems.TryGetValue(type.FullName, out isTraced))
                    return isTraced;
                else
                    type = type.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceData"></param>
        public void Write(TraceObject traceData)
        {
            var value = (TraceObjectConverter == null ? DefaultConvertTraceObject(traceData) : TraceObjectConverter(traceData));
            if (TracerProxy == null)
                TraceWrite(value);
            else
                TracerProxy.Process(value);          
        }

        private void TraceWrite(object entry)
        {
            if (entry == null)
                return;

            try
            {
                System.Xml.XmlNode node = XmlSerializerEx.XmlSerialize(entry, false);
                if (string.IsNullOrEmpty(Category))
                    Trace.WriteLine(node.OuterXml);
                else
                    Trace.WriteLine(node.OuterXml, Category);
                Trace.Flush();
            }
            catch
            {
                //ignore log exception
            }
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="message"></param>
        public void Write(object instance, string message)
        {
            if (instance == null)
                return;

            Type type = instance.GetType();
            if (IsTraced(type))
                this.Write(new TraceObject(type.Name, string.Empty, message));
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="message"></param>
        public void Write(string traceClass, string traceMethod, string message)
        {
            TraceObject traceObj = new TraceObject(traceClass, traceMethod, message);
            traceObj.Level = TraceLevel.Info;
            this.Write(traceObj);
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="ex"></param>
        public void Write(string traceClass, string traceMethod, Exception ex)
        {
            this.Write(traceClass, traceMethod, ex, int.MinValue);
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="ex"></param>
        /// <param name="errorCode"></param>
        public void Write(string traceClass, string traceMethod, Exception ex, int errorCode)
        {
            TraceObject traceObj;
            if (errorCode > int.MinValue)
                traceObj = new TraceObject(traceClass, traceMethod, "Trace exception", ex.ToString());
            else
                traceObj = new TraceObject(traceClass, traceMethod, "Trace exception", "ErrorCode=" + errorCode + ":" + ex.ToString());
            traceObj.Level = TraceLevel.Error;
            this.Write(traceObj);
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="value"></param>
        public void Write(string traceClass, string traceMethod, object value)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "Trace value", SerializeHelper.XmlSerialize(value).OuterXml));
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        public void Write(string traceClass, string traceMethod, string[] names, object[] values)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "Trace name-values", CreateTraceDetail(names, values)));
        }

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="node"></param>
        public void Write(string traceClass, string traceMethod, System.Xml.XmlNode node)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "Trace XmlNode", node.OuterXml));
        }

        /// <summary>
        /// EnterFunction
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="message"></param>
        public void EnterFunction(string traceClass, string traceMethod, string message)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "EnterFunction", message));
        }

        /// <summary>
        /// EnterFunction
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        public void EnterFunction(string traceClass, string traceMethod, string[] names, object[] values)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "EnterFunction", CreateTraceDetail(names, values)));
        }

        /// <summary>
        /// LeaveFunction
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="message"></param>
        public void LeaveFunction(string traceClass, string traceMethod, string message)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "LeaveFunction", message));
        }

        /// <summary>
        /// LeaveFunction
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="names"></param>
        /// <param name="values"></param>
        public void LeaveFunction(string traceClass, string traceMethod, string[] names, object[] values)
        {
            this.Write(new TraceObject(traceClass, traceMethod, "LeaveFunction", CreateTraceDetail(names, values)));
        }
        
        private string CreateTraceDetail(string[] names, object[] values)
        {
            SortedList parameters = new SortedList();
            for (int i = 0; i < names.Length; i++)
            {
                parameters.Add(names[i], values[i]);
            }
            return XmlSerializerEx.XmlSerialize(parameters, false).OuterXml;
        }

        private SortedList DefaultConvertTraceObject(TraceObject traceObj)
        {
            SortedList parameters = new SortedList();
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            if (context != null)
            {
                parameters.Add("RequestType", context.Request.RequestType);
                parameters.Add("HttpMethod", context.Request.HttpMethod);
                if(context.Request.Url != null)
                    parameters.Add("Url", context.Request.Url.ToString());
                if (context.Request.UrlReferrer != null)
                    parameters.Add("UrlReferrer", context.Request.UrlReferrer.ToString());
                parameters.Add("LogonUser", context.Request.LogonUserIdentity.Name);
                if (context.Request.QueryString.Count > 0)
                    parameters.Add("QueryString", context.Request.QueryString.ToString());
                if (context.Request.Form.Count > 0)
                    parameters.Add("Form", context.Request.Form.ToString());
                if (context.Request.Cookies.Count > 0)
                {
                    NameValueCollection cookies = new NameValueCollection();
                    for (int i = 0; i < context.Request.Cookies.Count; i++)
                    {
                        System.Web.HttpCookie cookie = context.Request.Cookies[i];
                        cookies.Add(cookie.Name, cookie.Value);
                    }
                    parameters.Add("Cookies", cookies);
                }
                
                const string TraceContext = "TraceContext";
                if (context.Items.Contains(TraceContext))
                    parameters.Add(TraceContext, context.Items[TraceContext]);
            }
            parameters.Add("TraceObject", traceObj);
            return parameters;
        }        
    }
}
