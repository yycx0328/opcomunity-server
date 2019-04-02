using Opcomunity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IRobotService
    {
        List<TB_NeteaseMessageSend> GetWaitingSendList(DateTime time);
    }
}
