using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aliyun.OSS;
using System.Reflection;
using log4net;
using System.IO;
using Utility.Common;
using System.Drawing;
using System.Configuration;
using Opcomunity.Services.Helpers;
using Opcomunity.Services;

namespace WebSite.Common
{
    public class OSSFileCore
    { 
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        //private static readonly string FfmpegPath = ConfigurationManager.AppSettings["FfmpegPath"];
        // 图片的尺寸如:240*180
        private static readonly string WHVideoImage = "360*640";
        // 开始截取的时间如:"1"
        private static readonly string CutTimeFrame = "1";

        private OSSFileCore() { }

        private static readonly OssClient client = new OssClient(OSSConfig.Endpoint, OSSConfig.AccessKeyId, OSSConfig.AccessKeySecret);

        private static void CreateBucketIfNotExist(string bucketName)
        {
            if(!client.DoesBucketExist(bucketName))
            {
                Bucket bucket = client.CreateBucket(bucketName);
                client.SetBucketAcl(bucket.Name, CannedAccessControlList.PublicRead);
                Log4NetHelper.Info(log, "Bucket创建成功，存储空间名称"+bucket.Name);
            }
        }

        public static string UploadFileStream(string buketName, string fileKey,Stream stream)
        {
            PutObjectResult request = client.PutObject(buketName, fileKey, stream);
            if(request.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return string.Format("http://{0}.{1}/{2}", buketName, OSSConfig.Host, fileKey);
            }
            return string.Empty;
        }

        public static UploadImageCheckResult UploadImageStream(HttpPostedFileBase file, string bucketName, string fileName, out string fileKey)
        {
            fileKey = string.Empty;
            try
            {
                CreateBucketIfNotExist(bucketName);
                string allowExt = ".gif.png.jpg.jpeg";
                FileInfo fileInfo = new FileInfo(file.FileName);
                var fileExtension = fileInfo.Extension.ToLower();
                Image img = Image.FromStream(file.InputStream);
                if (img == null)
                    return UploadImageCheckResult.FileFormatterErr;

                if (allowExt.IndexOf(fileExtension) == -1)
                    return UploadImageCheckResult.FileExtensionErr;

                fileKey = string.Format("{0}{1}", fileName, fileExtension);

                ImageConverter imgconv = new ImageConverter();
                byte[] bytes = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
                using (Stream stream = new MemoryStream(bytes))
                {
                    var result = client.PutObject(bucketName, fileKey, stream);
                    if (result.HttpStatusCode == System.Net.HttpStatusCode.OK)
                        return UploadImageCheckResult.Success;
                    return UploadImageCheckResult.Failed;
                }
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return UploadImageCheckResult.Excption;
            }
        }

        public static UploadVideoCheckResult UploadVideo(HttpPostedFileBase file,string directory, string bucketNameSuffix, string fileName)
        {
            try
            {
                string imgBucketName = string.Format("{0}-{1}", OSSConfig.ImageBucketPrefix, bucketNameSuffix);
                string videoBucketName = string.Format("{0}-{1}", OSSConfig.VideoBucketPrefix, bucketNameSuffix);
                CreateBucketIfNotExist(imgBucketName);
                CreateBucketIfNotExist(videoBucketName);
                string allowExt = ".mp4";
                FileInfo fileInfo = new FileInfo(file.FileName);
                var fileExtension = fileInfo.Extension.ToLower();
                if (allowExt.IndexOf(fileExtension) == -1)
                    return UploadVideoCheckResult.FileExtensionErr;

                var videoFileKey = string.Format("{0}.mp4", fileName);

                // 如果目录不存在，则重建
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                Log4NetHelper.Info(log, "1、上传文件开始");
                string videoPhysicalPath = string.Format("{0}/{1}", directory, videoFileKey);
                using (var stream = file.InputStream)
                {
                    // 如果文件已经存在，则删除
                    if (File.Exists(videoPhysicalPath))
                    {
                        File.Delete(videoPhysicalPath);
                    }
                    byte[] bytes = new byte[(int)stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Log4NetHelper.Info(log, "2、上传至OSS存储空间");
                        var upVideoResult = client.PutObject(videoBucketName, videoFileKey, ms);
                        if (upVideoResult.HttpStatusCode != System.Net.HttpStatusCode.OK)
                            return UploadVideoCheckResult.Failed;

                        Log4NetHelper.Info(log, "3、存储至本地");
                        using (FileStream fs = new FileStream(videoPhysicalPath, FileMode.Create))
                        {
                            ms.WriteTo(fs);
                        }
                    }
                }

                Log4NetHelper.Info(log, "4、上传文件结束");
                var imageFileKey = string.Format("{0}.jpg", fileName);
                string imgPath = string.Format("{0}/{1}", directory, imageFileKey);
                if (!GetPicFromVideo(videoPhysicalPath, imgPath))
                    return UploadVideoCheckResult.VideoImageCatchErr;
                Log4NetHelper.Info(log, "5、截取封面完成");

                // 如果文件已经存在，则删除
                if (File.Exists(videoPhysicalPath))
                {
                    File.Delete(videoPhysicalPath);
                }
                Log4NetHelper.Info(log, "6、移除本地视频文件");
                
                Log4NetHelper.Info(log, imgPath);
                var upImageResult = client.PutObject(imgBucketName, imageFileKey, imgPath);
                if (upImageResult.HttpStatusCode != System.Net.HttpStatusCode.OK)
                    return UploadVideoCheckResult.Failed;

                // 如果文件已经存在，则删除
                if (File.Exists(imgPath))
                {
                    File.Delete(imgPath);
                }
                Log4NetHelper.Info(log, "7、移除本地截图");

                return UploadVideoCheckResult.Success;
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return UploadVideoCheckResult.Excption;
            }
        }

        /// <param name="VideoName">视频文件pic/guiyu.mov</param>
        /// <param name="WidthAndHeight">图片的尺寸如:240*180</param>
        /// <param name="CutTimeFrame">开始截取的时间如:"1"</param>
        #region 从视频画面中截取一帧画面为图片
        public static bool GetPicFromVideo(string videoPath, string imgPath)
        {
            var FfmpegPath = ConfigHelper.GetValue("FfmpegPath");
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo(FfmpegPath);
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.Arguments = " -i " + videoPath + " -y -f image2 -ss " + CutTimeFrame + " -t 0.001 -s " + WHVideoImage + " " + imgPath;  //設定程式執行參數
            try
            {
                System.Diagnostics.Process.Start(startInfo);
                System.Threading.Thread.Sleep(500);
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return false;
            }
        }
        #endregion
    }
}