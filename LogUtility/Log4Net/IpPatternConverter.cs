using log4net.Core;
using log4net.Layout.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class IpPatternConverter : PatternLayoutConverter
{
    protected override void Convert(System.IO.TextWriter writer, LoggingEvent loggingEvent)
    {
        var property = loggingEvent.MessageObject as CustomLogProperty;
        if (property != null)
        {
            writer.Write(property.Ip);
        }
    }
}
