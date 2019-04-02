using Passport.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Common;

namespace Passport.Services.Implementations
{
    public class AccountService : ServiceBase, IAccountService
    {
        public bool IsPhoneRegist(string phoneNo)
        {
            using (var context = base.NewContext())
            {
                return context.TB_UserAuth.SingleOrDefault(p=> 
                    p.IdentityType == UserAuthIdentityType.phone.ToString()
                    && p.Identifier == phoneNo) != null;
            }
        }
    }
}
