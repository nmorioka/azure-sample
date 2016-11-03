using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using WorkerEnvironment;

namespace Utils
{

    public class Storage
    {
        private static CloudStorageAccount cloudStorageAccount;

        public static void Init(CloudStorageAccount account)
        {
            cloudStorageAccount = account;

            try
            {
                Directory.CreateDirectory(ProcessorPath.srcDirPath);
                Directory.CreateDirectory(ProcessorPath.dstDirPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

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
        public static bool DownLoadContents()
        {
            try
            {
                // クライアントの作成
                CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(BlobName.BIN_CONTAINER_NAME);

                Directory.CreateDirectory(ProcessorPath.binPath);
                DownLoadDirectory(container.ListBlobs(), ProcessorPath.binPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public static Stream DownloadFileToStream(string fileName)
        {
            // クライアントの作成
            CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(BlobName.IMAGE_CONTAINER_NAME);

            MemoryStream ms = new MemoryStream();
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.DownloadToStream(ms);

            return ms;
        }

        public static void UploadFileToStream(string localFileName, string uploadFileName)
        {
            CloudBlobClient client = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(BlobName.RESULT_CONTAINER_NAME);

            CloudBlockBlob blockBlob = container.GetBlockBlobReference(uploadFileName);
            blockBlob.UploadFromFile(localFileName);
        }

    }
}
