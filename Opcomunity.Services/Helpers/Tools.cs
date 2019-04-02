using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Helpers
{
    public class Tools
    {
        public static string ReplacePhoneAsNickName(string phoneNo)
        {
            if (!string.IsNullOrEmpty(phoneNo)
                && StringHelper.TryRegex(phoneNo, RegularType.Mobile))
            {
                return phoneNo.Replace(phoneNo.Substring(3, 4), "****");
            }
            return string.Empty;
        }

        public static string GetDefaultAvatar()
        {
            var defaultAvatar = "http://st.opcomunity.com/images/avatar/default.png";
            try
            { 
                var avatar = ConfigHelper.GetValue("DefaultAvatar");
                if (string.IsNullOrEmpty(avatar))
                    return defaultAvatar;
                return avatar;
            }
            catch
            {
                return defaultAvatar;
            }
        }

        public static string GetDefaultThumbnailAvatar()
        {
            var defaultAvatar = "http://st.opcomunity.com/images/avatar/defaults.png";
            try
            { 
                var avatar = ConfigHelper.GetValue("DefaultThumbnailAvatar");
                if (string.IsNullOrEmpty(avatar))
                    return defaultAvatar;
                return avatar;
            }
            catch
            {
                return defaultAvatar;
            }
        }

        public static string GetDefaultNickName(string prefix, int numLength)
        {
            var result = new StringBuilder();
            for (var i = 0; i < numLength; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return string.Format("{0}{1}", prefix, result.ToString());
        }
    }
}
