using System;
using System.IO;
using System.Drawing;
using Microsoft.WindowsAzure.Storage;
using System.Drawing.Imaging;
using WorkerEnvironment;

namespace Utils
{

    public class ImageProcessor
    {
        private static int num = 0;

        public static void execute(Stream input)
        {
            string srcFilePath = "";
            string dstFilePath = "";

//            lock (thisLock)
//            {

                srcFilePath = ProcessorPath.srcDirPath + num + ".bmp";
                dstFilePath = ProcessorPath.dstDirPath + num + ".png";
                ++num;

            //            }

            Console.WriteLine("debug A");
            // inputからテンポラリファイルを生成
            SaveSrcFile(input, srcFilePath);
            
            // exeの実行
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = false;
            // WorkingDirectoryは実行パスと同じじゃないとダメ
            psi.WorkingDirectory = ProcessorPath.binPath;
            psi.FileName = ProcessorPath.binPath + "image.exe";
            psi.Arguments = String.Format("{0} {1} {2}",
                ProcessorPath.binPath + "exe.xml",
                dstFilePath,
                srcFilePath
                );

            // from_image.png -resize 50% to_image.png
            /*
            psi.FileName = root + "Bin" + @"\" + "convert.exe";
            psi.Arguments = String.Format("{0} {1} {2} {3}",
                srcFilePath,
                "-resize",
                "50%",
                dstFilePath
                );
              */              

            System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi);
            p.WaitForExit();

            Storage.UploadFileToStream(dstFilePath, "fugafuga.png");

            System.IO.File.Delete(srcFilePath);
            System.IO.File.Delete(dstFilePath);
        }

        private static void SaveSrcFile(Stream input, string fileName)
        {
            using (Bitmap bmp = new Bitmap(input))
            {
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
                Bitmap resizeBmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(resizeBmp);

                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
                g.DrawImage(bmp, 0, 0, width, height);

                Console.WriteLine("bmp save");
                resizeBmp.Save(fileName, ImageFormat.Bmp);
                bmp.Save(fileName);

                g.Dispose();
                bmp.Dispose();
                resizeBmp.Dispose();

            }
        }

    }
}
