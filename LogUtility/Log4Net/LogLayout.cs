using log4net.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class LogLayout : PatternLayout
{
    public LogLayout()
    {
        this.AddConverter("Ip", typeof(IpPatternConverter));
    }
}
