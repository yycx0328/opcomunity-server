using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Implementations
{
    public class ConfigService : ServiceBase, IConfigService
    {
        public string GetValue(string key)
        {
            string cacheKey = string.Format("Opcomunity_ConfigService_GetValue_{0}", key);
            return WebCache.GetCachedObject<string>(cacheKey, 7200, () =>
            {
                using (var context = base.NewContext())
                {
                    var query = from c in context.TB_Config
                                where c.IsAvailable && c.KeyId == key
                                select c.Value;
                    return query.SingleOrDefault();
                }
            });
        }
    }
}
