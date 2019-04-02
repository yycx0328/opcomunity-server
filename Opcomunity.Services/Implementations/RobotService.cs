using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Data.Entities;
using System.Data.Entity;

namespace Opcomunity.Services.Implementations
{
    public class RobotService : ServiceBase, IRobotService
    {
        public List<TB_NeteaseMessageSend> GetWaitingSendList(DateTime time)
        {
            using (var context = base.NewContext())
            {
                var query = from s in context.TB_NeteaseMessageSend
                            where DbFunctions.DiffSeconds(s.WaitingSendTime, time) == 0
                            select s;
                return query.ToList();
            }
        }
    }
}
