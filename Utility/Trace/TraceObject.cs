using System;

namespace Utility
{
    /// <summary>
    /// TraceObject
    /// </summary>
    [Serializable]
    public class TraceObject
    {
        /// <summary>
        /// The code structure or class writing the trace output.
        /// </summary>
        public string TraceClass
        {
            get { return _TraceClass; }
            set { _TraceClass = value; }
        }
        private string _TraceClass;

        /// <summary>
        /// The name of the method calling into trace.
        /// </summary>
        public string TraceMethod
        {
            get { return _TraceMethod; }
            set { _TraceMethod = value; }
        }
        private string _TraceMethod;

        /// <summary>
        /// The primary message of this trace.
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
        private string _Message;

        /// <summary>
        /// The supplementary description of this trace.
        /// </summary>
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _Description;

        /// <summary>
        /// The detail of this trace.
        /// </summary>
        public string Detail
        {
            get { return _Detail; }
            set { _Detail = value; }
        }
        private string _Detail;

        /// <summary>
        /// The level of the trace write.
        /// </summary>
        public System.Diagnostics.TraceLevel Level
        {
            get { return _Level; }
            set { _Level = value; }
        }
        private System.Diagnostics.TraceLevel _Level = System.Diagnostics.TraceLevel.Verbose;

        /// <summary>
        /// The time of the trace write.
        /// </summary>
        public DateTime TraceTime
        {
            set { _TraceTime = value; }
            get { return _TraceTime; }
        }
        private DateTime _TraceTime = DateTime.Now;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public TraceObject()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="message"></param>
        public TraceObject(string traceClass, string traceMethod, string message)
        {
            this.TraceClass = traceClass;
            this.TraceMethod = traceMethod;
            this.Message = message;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="traceClass"></param>
        /// <param name="traceMethod"></param>
        /// <param name="description"></param>
        /// <param name="detail"></param>
        public TraceObject(string traceClass, string traceMethod, string description, string detail)
        {
            this.TraceClass = traceClass;
            this.TraceMethod = traceMethod;
            this.Description = description;
            this.Detail = detail;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            AppendString(sb, "TraceClass", this.TraceClass);
            AppendString(sb, "TraceMethod", this.TraceMethod);
            AppendString(sb, "Message", this.Message);
            AppendString(sb, "Description", this.Description);
            AppendString(sb, "Detail", this.Detail);
            AppendString(sb, "Level", this.Level.ToString());
            AppendString(sb, "TraceTime", this.TraceTime.ToString());

            return sb.ToString();
        }

        private void AppendString(System.Text.StringBuilder sb, string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (sb.Length > 0)
                    sb.Append('&');
                sb.Append(name);
                sb.Append('=');
                sb.Append(System.Web.HttpUtility.UrlEncode(value));
            }
        }
    }
}
