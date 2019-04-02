using log4net;
using Opcomunity.Services.Helpers;
using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Web;
using Utility.Common;

namespace WebSite.Common
{
    public class FileUploadCore
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        //private static readonly string FfmpegPath = ConfigHelper.GetValue("FfmpegPath");
        private static readonly int THUMBNAIL_WIDTH = 240;
        // 图片的尺寸如:240*180
        private static readonly string WHVideoImage = "360*640";
        // 开始截取的时间如:"1"
        private static readonly string CutTimeFrame = "1";
        //private static readonly int THUMBNAIL_HEIGHT = 240;

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static UploadImageCheckResult SaveImageFile(HttpPostedFileBase file, string directory, string fileName, out string fileExtension)
        {
            try
            {
                string allowExt = ".gif.png.jpg.jpeg";
                FileInfo fileInfo = new FileInfo(file.FileName);
                fileExtension = fileInfo.Extension.ToLower();
                Image img = Image.FromStream(file.InputStream);
                if (img == null)
                    return UploadImageCheckResult.FileFormatterErr;
                if (allowExt.IndexOf(fileExtension) == -1)
                    return UploadImageCheckResult.FileExtensionErr;
                // 如果目录不存在，则重建
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                string orgFilePhysicalPath = string.Format("{0}/{1}{2}", directory, fileName,fileExtension);
                // 如果文件已经存在，则删除
                if(File.Exists(orgFilePhysicalPath))
                {
                    File.Delete(orgFilePhysicalPath);
                }
                file.SaveAs(orgFilePhysicalPath);

                // 生成缩略图
                string thumbnailImagePhysicalPath = string.Format("{0}/{1}S{2}", directory, fileName, fileExtension);
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream))
                {
                    int height = (int)(image.Height / (image.Width*1.0 / THUMBNAIL_WIDTH));
                    using (System.Drawing.Image newImage = image.GetThumbnailImage(THUMBNAIL_WIDTH, height, null, new IntPtr()))
                    {
                        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(newImage))
                        {
                            g.DrawImage(image, 0, 0, THUMBNAIL_WIDTH, height);
                            newImage.Save(thumbnailImagePhysicalPath);
                        }
                    }
                }
                 
                return UploadImageCheckResult.Success;
            }
            catch (Exception ex)
            {
                fileExtension = "";
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return UploadImageCheckResult.Excption;
            }
        }

        public static UploadVideoCheckResult SaveVideo(HttpPostedFileBase file, string directory, string fileName,string imgExtension, out string fileExtension)
        {
            try
            {
                string allowExt = ".mp4";
                FileInfo fileInfo = new FileInfo(file.FileName);
                fileExtension = fileInfo.Extension.ToLower();
                if (allowExt.IndexOf(fileExtension) == -1)
                    return UploadVideoCheckResult.FileExtensionErr;
                // 如果目录不存在，则重建
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                Log4NetHelper.Info(log, "3、上传文件开始");
                string orgFilePhysicalPath = string.Format("{0}/{1}{2}", directory, fileName, fileExtension);
                using (var stream = file.InputStream)
                {
                    // 如果文件已经存在，则删除
                    if (File.Exists(orgFilePhysicalPath))
                    {
                        File.Delete(orgFilePhysicalPath);
                    }
                    byte[] bytes = new byte[(int)stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        using (FileStream fs = new FileStream(orgFilePhysicalPath, FileMode.Create))
                        {
                            ms.WriteTo(fs);
                        }
                    }
                }

                Log4NetHelper.Info(log, "4、上传文件结束");
                string imgPath = string.Format("{0}/{1}{2}", directory, fileName, imgExtension);
                if (!GetPicFromVideo(orgFilePhysicalPath, imgPath))
                    return UploadVideoCheckResult.VideoImageCatchErr;

                Log4NetHelper.Info(log, "5、截取封面完成");
                return UploadVideoCheckResult.Success;
            }
            catch (Exception ex)
            {
                fileExtension = "";
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