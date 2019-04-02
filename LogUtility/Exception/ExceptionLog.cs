using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

partial class ExceptionLog
{
    public long LogID { get; set; }
    public string MachineName { get; set; }
    public string AssemblyName { get; set; }
    public string AppDomainName { get; set; }
    public string ThreadId { get; set; }
    public string WindowsIdentity { get; set; }
    public int EventID { get; set; }
    public int EventPriority { get; set; }
    public string EventSeverity { get; set; }
    public string CategoryName { get; set; }
    public string Title { get; set; }
    public string ExceptionSource { get; set; }
    public string ExceptionType { get; set; }
    public string HelpLink { get; set; }
    public string TargetSite { get; set; }
    public string Message { get; set; }
    public string FormattedMessage { get; set; }
    public DateTime LogTime { get; set; }
}
