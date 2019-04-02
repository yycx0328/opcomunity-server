using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Passport.Services.Interface
{
    public interface IAccountService
    {
        bool IsPhoneRegist(string phoneNo);
    }
}
