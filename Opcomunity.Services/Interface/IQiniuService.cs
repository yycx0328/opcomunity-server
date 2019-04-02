using Opcomunity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IQiniuService
    {
        /// <summary>
        /// 获取已存储的上传凭证
        /// </summary>
        /// <returns></returns>
        string GetUploadToken();

        /// <summary>
        /// 保存上传凭证
        /// </summary>
        /// <param name="model"></param>
        void SaveUploadToken(TB_QiniuUploadToken model);
    }
}
