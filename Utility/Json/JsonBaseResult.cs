using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[Serializable]
[DataContract()]
public class JsonBase
{
    private object _data;
    public int state { get; set; }
    public string message { get; set; }
    public string remark { set; get; }
    public JsonBase()
    {
        state = -1000;
        message = "初始化异常";
    }

    public virtual object data
    {
        get
        {
            if (_data == null)
                _data = "";
            return _data;
        }
        set
        {
            _data = value;
        }
    }

    public virtual string ToJson()
    {
        return JSONSerializeUtil.ToJson(this);
    }
}

public class JsonResultData
{
    public string data { get; set; }
}