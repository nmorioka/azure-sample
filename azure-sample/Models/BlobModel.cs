using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;

namespace Models
{

    public class BlobModel
    {
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer uploadContainer;
        private static CloudBlobContainer resultContainer;


        public BlobModel()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static void Init(CloudStorageAccount storageAccount)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            uploadContainer = blobClient.GetContainerReference("upload-images");
            try
            {
                uploadContainer.CreateIfNotExists();
            }
            catch (StorageException)
            {
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                throw;
            }

            resultContainer = blobClient.GetContainerReference("result-images");
            try
            {
                resultContainer.CreateIfNotExists();
            }
            catch (StorageException)
            {
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                throw;
            }
        }

        public static void Upload(string fileName, string imagePathName)
        {

            // Upload a BlockBlob to the newly created container
            CloudBlockBlob blockBlob = uploadContainer.GetBlockBlobReference(fileName);
            blockBlob.UploadFromFile(imagePathName);
        }

        public static void Download(string fileName, Stream stream)
        {
            CloudBlockBlob blockBlob = uploadContainer.GetBlockBlobReference(fileName);
            blockBlob.DownloadToStream(stream);
        }

        
        public static void DownloadResult(string fileName, Stream stream)
        {
            CloudBlockBlob blockBlob = resultContainer.GetBlockBlobReference(fileName);
            blockBlob.DownloadToStream(stream);
        }

    }
}
