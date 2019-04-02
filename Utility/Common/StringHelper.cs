using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


/// <summary>
/// the helper class for string
/// </summary>
public static class StringHelper
{
    //static int counter = 0; 
    private static readonly string sLetters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ#";

    /// <summary>
    /// 是否含有ASCII码小于等于31的字符
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static bool IsEspecial(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            try
            {
                int val = (int)Convert.ToChar(input.Substring(i, 1));
                if (val <= 31)
                    return true;
            }
            catch { }
        }
        return false;
    }

    /// <summary>
    /// 去除ASCII码小于等于31的字符
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ReplaceStr(string input)
    {
        string rtn = string.Empty;
        for (int i = 0; i < input.Length; i++)
        {
            try
            {
                int val = (int)Convert.ToChar(input.Substring(i, 1));
                if (val > 31)
                    rtn += input.Substring(i, 1);
            }
            catch { }
        }
        return rtn;
    }
    /// <summary>
    /// 是否符合社区昵称要求,不以数字开头，2-5个汉字长度，4-14个字符长度
    /// </summary>
    /// <param name="input"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool IsLegalNickName(string input, ref string message)
    {
        message = "昵称不能以数字或下划线开头,允许英文中文和数字";
        if (string.IsNullOrEmpty(input)) return false;
        input = input.Trim();
        if (TryRegex(input, RegularType.NickName))
        {
            int length = 0;
            foreach (var item in input.ToCharArray())
            {
                if (Convert.ToInt32(item) < 255)
                {
                    length++;
                }
                else
                    length += 2;
            }
            if (length >= 4 && length <= 14)
                return true;
            else
            {
                message = "昵称只能4-14个字符(一个汉字为2个字符)";
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 指定的字符是否符合指定长度范围
    /// </summary>
    /// <param name="input"></param>
    /// <param name="minLength"></param>
    /// <param name="maxLength"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static bool IsValidString(string input, int minLength, int maxLength, ref string message)
    {

        int length = 0;
        foreach (var item in input.ToCharArray())
        {
            if (Convert.ToInt32(item) < 255)
            {
                length++;
            }
            else
                length += 2;
        }
        if (length >= minLength && length <= maxLength)
            return true;
        else
        {
            message = string.Format("只能{0}-{1}个字符(一个汉字为2个字符)", minLength, maxLength);
            return false;
        }

    }

    #region 正则匹配字符串
    /// <summary>
    /// 正则匹配字符串
    /// </summary>
    /// <param name="input"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool TryRegex(string input, RegularType type)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        string regularExpression = string.Empty;

        switch (type)
        {
            case RegularType.Int:
                regularExpression = @"^\d+$";
                break;
            case RegularType.PositiveInt:
                regularExpression = @"^\+?[1-9][0-9]*$";
                break;
            case RegularType.Money:
                regularExpression = @"^[0-9]+(.[0-9]{2})?$";
                break;
            case RegularType.Mail:
                regularExpression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                break;
            case RegularType.Postalcode:
                regularExpression = @"^[0-9]\d{5}$";
                break;
            case RegularType.Phone:
                regularExpression = @"^(\d{3,4}|\d{3,4}-)?\d{7,8}$";
                break;
            case RegularType.Mobile:
                regularExpression = @"^0?1[3|4|5|7|8|9][0-9]\d{8}$";
                break;
            case RegularType.InternetUrl:
                regularExpression = @"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$";
                break;
            case RegularType.IdCard: //身份证号(15位或18位数字)
                regularExpression = @"^((1[1-5])|(2[1-3])|(3[1-7])|(4[1-6])|(5[0-4])|(6[1-5])|71|(8[12])|91)\d{4}((19\d{2}(0[13-9]|1[012])(0[1-9]|[12]\d|30))|(19\d{2}(0[13578]|1[02])31)|(19\d{2}02(0[1-9]|1\d|2[0-8]))|(19([13579][26]|[2468][048]|0[48])0229))\d{3}(\d|X|x)?$";
                break;
            case RegularType.Date:   //日期范围:1900-2099;简单验证1-12月,1-31日
                regularExpression = @"^(19|20)\d{2}-(0?\d|1[012])-(0?\d|[12]\d|3[01])$";
                break;
            case RegularType.Ip:
                regularExpression = @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
                break;
            case RegularType.QQ:
                regularExpression = @"^[1-9]\d{5,12}$";
                break;
            case RegularType.ChineseName:
                regularExpression = @"[\u4e00-\u9fa5]{2,4}";
                break;
            case RegularType.NickName:
                regularExpression = @"^[\u4E00-\u9FA5|a-zA-Z][\u4E00-\u9FA5|0-9a-zA-Z]*$";
                break;
            case RegularType.Account:
                regularExpression = @"^[0-9a-zA-Z]{6,16}$";
                break;
            case RegularType.SystemAccount:
                regularExpression = @"^(v|V)[0-9]+$";
                break;
            case RegularType.RockAccount:
                regularExpression = @"^[a-zA-Z]+\w{5,15}";
                break;
            default:
                break;
        }
        Regex regex = new Regex(regularExpression);
        return regex.Match(input).Success;
    } 
    #endregion

    #region 身份证号码验证 
    /// <summary>
    /// 验证身份证号码
    /// </summary>
    /// <param name="Id">身份证号码</param>
    /// <returns>验证成功为True，否则为False</returns>
    public static bool CheckIDCard(string Id, out DateTime Birthday)
    {
        Birthday = new DateTime();
        if (Id.Length == 18)
        {
            bool check = CheckIDCard18(Id, out Birthday);
            return check;
        }
        else if (Id.Length == 15)
        {
            bool check = CheckIDCard15(Id, out Birthday);
            return check;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 验证18位身份证号
    /// </summary>
    /// <param name="Id">身份证号</param>
    /// <returns>验证成功为True，否则为False</returns>
    private static bool CheckIDCard18(string Id, out DateTime Birthday)
    {
        Birthday = new DateTime();
        long n = 0;
        if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }
        string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        Birthday = new DateTime();
        if (DateTime.TryParse(birth, out Birthday) == false)
        {
            return false;//生日验证
        }
        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = Id.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        Math.DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
        {
            return false;//校验码验证
        }
        return true;//符合GB11643-1999标准
    }

    /// <summary>
    /// 验证15位身份证号
    /// </summary>
    /// <param name="Id">身份证号</param>
    /// <returns>验证成功为True，否则为False</returns>
    private static bool CheckIDCard15(string Id, out DateTime Birthday)
    {
        Birthday = new DateTime();
        long n = 0;
        if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }
        string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        Birthday = new DateTime();
        if (DateTime.TryParse(birth, out Birthday) == false)
        {
            return false;//生日验证
        }
        return true;//符合15位身份证标准
    }
    #endregion

    #region 全角和半角互相转换函数
    /// <summary>
    /// 转全角的函数(SBC case)
    /// </summary>
    /// <param name="input">任意字符串</param>
    /// <returns>全角字符串</returns>
    /// <remarks>
    /// 全角空格为12288，半角空格为32
    /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
    /// </remarks>
    public static string ToSBC(string input)
    {
        //半角转全角：
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 32)
            {
                c[i] = (char)12288;
                continue;
            }
            if (c[i] < 127)
                c[i] = (char)(c[i] + 65248);
        }
        return new string(c);
    }

    /// <summary> 转半角的函数(DBC case) </summary>
    /// <param name="input">任意字符串</param>
    /// <returns>半角字符串</returns>
    /// <remarks>
    /// 全角空格为12288，半角空格为32
    /// 其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
    /// </remarks>
    public static string ToDBC(string input)
    {
        char[] c = input.ToCharArray();
        for (int i = 0; i < c.Length; i++)
        {
            if (c[i] == 12288)
            {
                c[i] = (char)32;
                continue;
            }
            if (c[i] > 65280 && c[i] < 65375)
                c[i] = (char)(c[i] - 65248);
        }
        return new string(c);
    } 
    #endregion

    static Regex reg = new Regex("<[^>]+>", RegexOptions.None);
    public static string RemoveHtmlEntity(string s)
    {
        return reg.Replace(s, string.Empty);
    }

    public static string SafeHtmlString(string s)
    {
        return s.Replace("<", "&lt;").Replace(">", "&gt;").Replace("\n", "<br />").Replace("\r\n", "<br />").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;");
    }

    public static string Addslashes(string s)
    {
        return s.Replace("'", "\\'").Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
    
    public static string UbbToHtml(string s)
    { 
        s = s.Replace(" ", "&nbsp;");//空格
        s = s.Replace("\n", "<br/>");//回车
        Regex reg = new Regex(@"(\[IMG\])(.[^\[]*)(\[\/IMG\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<a href=""$2"" target=_blank><IMG SRC=""$2"" border=0 alt=按此在新窗口浏览图片 onload=""javascript:if(this.width>screen.width-333)this.width=screen.width-333""></a>");

        reg = new Regex(@"(\[URL\])(http:\/\/.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<A HREF=""$2"" TARGET=_blank>$2</A>");

        reg = new Regex(@"(\[URL\])(.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<A HREF=""http://$2"" TARGET=_blank>$2</A>");

        reg = new Regex(@"(\[URL=(http:\/\/.[^\[]*)\])(.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<A HREF=""$2"" TARGET=_blank>$3</A>");

        reg = new Regex(@"(\[URL=(.[^\[]*)\])(.[^\[]*)(\[\/URL\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<A HREF=""http://$2"" TARGET=_blank>$3</A>");
         
        reg = new Regex(@"^(HTTP://[A-Za-z0-9\./=\?%\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<a target=_blank href=$1>$1</a>");

        reg = new Regex(@"(HTTP://[A-Za-z0-9\./=\?%\-&_~`@':+!]+)$", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<a target=_blank href=$1>$1</a>");

        reg = new Regex(@"[^>=""](HTTP://[A-Za-z0-9\./=\?%\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<a target=_blank href=$1>$1</a>");
         
        reg = new Regex(@"[^>=""](FTP://[A-Za-z0-9\.\/=\?%\-&_~`@':+!]+)", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<a target=_blank href=$1>$1</a>");

        reg = new Regex(@"(\[I\])(.[^\[]*)(\[\/I\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<i>$2</i>");

        reg = new Regex(@"(\[B\])(.[^\[]*)(\[\/U\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<u>$2</u>");

        reg = new Regex(@"(\[B\])(.[^\[]*)(\[\/B\])", RegexOptions.IgnoreCase);
        s = reg.Replace(s, @"<b>$2</b>");
        return s;
    }

    /// <summary>
    /// 获取字符串非数字的第一个位置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static int NoNumFirstIndex(string input)
    {
        int index = 0;
        for (int i = 0; i < input.Length; i++)
        {
            try
            {
                int val = (int)Convert.ToChar(input.Substring(i, 1));
                if (!(val > 47 && val <= 57))
                {
                    index = i;
                    break;
                }
            }
            catch { }
        }
        return index;
    }

    public static string StringReplace(string input, string chars = @"'',""&<>()#$?*")
    {
        for (int i = 0; i < chars.Length; i++)
        {
            input = input.Replace(chars[i].ToString(), "");
        }
        return input;
    } 
}

/// <summary>
/// RegularType
/// </summary>
public enum RegularType
{
    Int = 1,
    PositiveInt = 2,
    Money = 3,
    Mail = 4,
    Postalcode = 5,
    Phone = 6,
    Mobile = 7,
    InternetUrl = 8,
    IdCard = 9,
    Date = 10,
    Ip = 11,
    QQ = 12,
    ChineseName = 13,
    /// <summary>
    /// 不以数字开头的昵称,长度为2-16位
    /// </summary>
    NickName = 14,
    /// <summary>
    /// 账号（6-16位英文和字母）
    /// </summary>
    Account = 15,
    /// <summary>
    /// 系统产生的Account
    /// </summary>
    SystemAccount = 16,
    /// <summary>
    /// 疯狂摇帐号
    /// </summary>
    RockAccount = 17
}