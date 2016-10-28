using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;


namespace Utils
{
    using System;
    using System.Runtime.InteropServices;
    using System.IO;
    using System.Collections.Generic;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Auth;
    using Microsoft.WindowsAzure.Storage.Blob;
    using Microsoft.WindowsAzure.Storage.RetryPolicies;
    using Microsoft.WindowsAzure.ServiceRuntime;

    public class ImageProcessor
    {
        private static int num = 0;
        private static void execute(Stream input)
        {
            string root = RoleEnvironment.GetLocalResource("LocalStorage").RootPath;
            string srcFilePath = "";
            string dstFilePath = "";

//            lock (thisLock)
//            {

                srcFilePath = root + @"Src\" + num + ".bmp";
                dstFilePath = root + @"Dst\" + num + ".png";
                ++num;

            //            }

            /*
             ファイルの上限値はWeb.configの「maxRequestLength」で調整します。単位はKB
            <system.web>
                <compilation debug="true" targetFramework="4.5" />
                <httpRuntime targetFramework="4.5" maxRequestLength="10240" />
            </system.web>
            */

            if (input != null)
            {
                Bitmap bmp = new Bitmap(input);

                int width = bmp.Width;
                int height = bmp.Height;

                if (width > height)
                {
                    double aspect = (double)width / height;

                    height = 1080;
                    width = (int)(1080 * aspect);

                    if (width % 2 != 0)
                    {
                        --width;
                    }
                }
                else
                {
                    double aspect = (double)height / width;

                    width = 1080;
                    height = (int)(1080 * aspect);

                    if (height % 2 != 0)
                    {
                        --height;
                    }
                }
                Bitmap resizeBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Graphics g = Graphics.FromImage(resizeBmp);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(bmp, 0, 0, width, height);

                resizeBmp.Save(srcFilePath);

//                ViewData["before_image1"] = ImageToBase64String(resizeBmp);
//                ViewData["before_image1_width"] = resizeBmp.Width / 2;
//                ViewData["before_image1_height"] = resizeBmp.Height / 2;

                g.Dispose();
                bmp.Dispose();
                resizeBmp.Dispose();
            }
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();

            psi.UseShellExecute = false;
            psi.WorkingDirectory = root;
            psi.FileName = root + "image.exe";
            psi.Arguments = String.Format("{0} {1} {2}",
                root + "exe.xml",
                dstFilePath,
                srcFilePath
                );
            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            p.WaitForExit();

            if (input != null)
            {
//                FileStream file = new FileStream(dstFilePath, FileMode.Open);
//                Bitmap bmp = new Bitmap(file);

//                ViewData["image1"] = ImageToBase64String(bmp);
//                ViewData["image1_width"] = bmp.Width / 2;
//                ViewData["image1_height"] = bmp.Height / 2;

//                bmp.Dispose();
//                file.Close();
            }

            System.IO.File.Delete(srcFilePath);
            System.IO.File.Delete(dstFilePath);
        }
    }
}
