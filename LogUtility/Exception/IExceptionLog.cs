// =========================================================
// Author	:   luyunhai
// Create Time  :   8/10/2015 5:01:23 PM
// =========================================================
// Copyright © USER-VFH583E7VU 2015 . All rights reserved.
// =========================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 异常日志
/// </summary>
interface IExceptionLog
{
    /// <summary>
    /// 应用程序域
    /// </summary>
    string AppDomainName { get; set; }
    /// <summary>
    /// 组件名
    /// </summary>
    string AssemblyName { get; set; }
    /// <summary>
    /// 类别
    /// </summary>
    string CategoryName { get; set; }
    /// <summary>
    /// 事件ID
    /// </summary>
    int EventID { get; set; }
    /// <summary>
    /// 事件优先级
    /// </summary>
    int EventPriority { get; set; }
    /// <summary>
    /// 事件重要性
    /// </summary>
    string EventSeverity { get; set; }
    /// <summary>
    /// 异常源
    /// </summary>
    string ExceptionSource { get; set; }
    /// <summary>
    /// 异常类型
    /// </summary>
    string ExceptionType { get; set; }
    /// <summary>
    /// 格式化后的异常信息
    /// </summary>
    string FormattedMessage { get; set; }
    /// <summary>
    /// 帮助连接
    /// </summary>
    string HelpLink { get; set; }
    /// <summary>
    /// 记录ID
    /// </summary>
    long LogID { get; set; }
    /// <summary>
    /// 记录时间
    /// </summary>
    DateTime LogTime { get; set; }
    /// <summary>
    /// 服务器名
    /// </summary>
    string MachineName { get; set; }
    /// <summary>
    /// 异常信息
    /// </summary>
    string Message { get; set; }
    /// <summary>
    /// 异常TargetSite
    /// </summary>
    string TargetSite { get; set; }
    /// <summary>
    /// 当前线程的Identity
    /// </summary>
    string ThreadId { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    string Title { get; set; }
    /// <summary>
    /// 当前WindowsIdentity
    /// </summary>
    string WindowsIdentity { get; set; }
}

/// <summary>
/// DefaultExceptionLog
/// </summary>
public class DefaultExceptionLog : IExceptionLog
{
    public string AppDomainName { get; set; }

    public string AssemblyName { get; set; }

    public string CategoryName { get; set; }

    public int EventID { get; set; }

    public int EventPriority { get; set; }

    public string EventSeverity { get; set; }

    public string ExceptionSource { get; set; }

    public string ExceptionType { get; set; }

    public string FormattedMessage { get; set; }

    public string HelpLink { get; set; }

    public long LogID { get; set; }

    public DateTime LogTime { get; set; }

    public string MachineName { get; set; }

    public string Message { get; set; }

    public string TargetSite { get; set; }

    public string ThreadId { get; set; }

    public string Title { get; set; }

    public string WindowsIdentity { get; set; }
    public string AppCoreName { get; set; }
}
