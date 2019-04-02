using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Api验证提示
public enum ValidateTips : int
{
    [Remark("请求成功")]
    Success = 0,
    [Remark("失败")]
    Faild = 1,
    [Remark("请重新登录")]
    Error_UserAccount = 1000,
    [Remark("初始化异常")]
    Error_Init,
    [Remark("系统异常，请稍候再试")]
    Error_Exception,
    [Remark("未能查找到应用信息")]
    Error_Application,
    [Remark("非法请求")]
    Error_Sign,
    [Remark("基本参数不正确")]
    Error_BaseParams,
    [Remark("请求时间格式不正确")]
    Error_TimeStamp,
    [Remark("请求链接已失效")]
    Error_Url,
    [Remark("业务参数不正确")]
    Error_BusinessParams
} 
#endregion