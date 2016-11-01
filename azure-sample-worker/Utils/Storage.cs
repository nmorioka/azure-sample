using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Utils
{

    public class Storage
    {
        private static bool DownLoadDirectory(IEnumerable<IListBlobItem> list, string DLDir)
        {
            try
            {
                Directory.CreateDirectory(DLDir);

                // コンテンツのダウンロード
                foreach (IListBlobItem item in list)
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;

                        Console.WriteLine("b : " + DLDir + Path.GetFileName(item.Uri.ToString()));
                        blob.DownloadToFile(DLDir + Path.GetFileName(item.Uri.ToString()), FileMode.Create);
                    }
                    else if (item.GetType() == typeof(CloudPageBlob))
                    {
                        CloudPageBlob pageBlob = (CloudPageBlob)item;
                    }
                    else if (item.GetType() == typeof(CloudBlobDirectory))
                    {
                        CloudBlobDirectory directory = (CloudBlobDirectory)item;
                        string prefix = directory.Prefix.Substring(0, directory.Prefix.Length - 1);

                        int index = prefix.LastIndexOf("/");

                        if (index >= 0)
                        {
                            prefix = prefix.Substring(index + 1);
                        }
                        DownLoadDirectory(directory.ListBlobs(), DLDir + prefix + @"\");
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        // 動画生成に必要なコンテンツをStorageからダウンロード
        public static bool DownLoadContents(CloudStorageAccount account)
        {
            try
            {
                // クライアントの作成
                CloudBlobClient client = account.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("bin");

                Directory.CreateDirectory(ProcessorPath.binPath);
                DownLoadDirectory(container.ListBlobs(), ProcessorPath.binPath);

                Directory.CreateDirectory(ProcessorPath.srcDirPath);
                Directory.CreateDirectory(ProcessorPath.dstDirPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static Stream DownloadFileToStream(CloudStorageAccount account, string fileName)
        {
            // クライアントの作成
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("sample");

            MemoryStream ms = new MemoryStream();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.DownloadToStream(ms);

            return ms;
        }

        public static Stream FileToStream(CloudStorageAccount account, string fileName)
        {
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("sample");

            MemoryStream ms = new MemoryStream();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.DownloadToStream(ms);

            return ms;
        }

        public static void UploadFileToStream(CloudStorageAccount account, string fileName)
        {
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("sample");

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("fuga2.png");
            blockBlob.UploadFromFile(fileName);
        }

    }
}
