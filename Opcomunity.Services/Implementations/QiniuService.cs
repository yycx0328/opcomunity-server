using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Data.Entities;

namespace Opcomunity.Services.Implementations
{
    public class QiniuService : ServiceBase, IQiniuService
    {
        public string GetUploadToken()
        {
            using (var context = base.NewContext())
            {
                var token = context.TB_QiniuUploadToken.FirstOrDefault(p => p.ExpiresTime > DateTime.Now);
                if (token != null)
                    return token.Token;
                return string.Empty;
            }
        }

        public void SaveUploadToken(TB_QiniuUploadToken model)
        {
            using (var context = base.NewContext())
            {
                context.TB_QiniuUploadToken.Add(model);
                context.SaveChanges();
            }
        }
    }
}
