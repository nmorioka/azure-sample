using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;


namespace Models
{

    public class ImageProcessJobModel
    {

        private const string TableName = "imageProcessJob";
        private const string AccountId = "Account1";

        /// <summary>
        /// Demonstrate basic Table CRUD operations. 
        /// </summary>
        /// <param name="table">The sample table</param>
        public void Crate(string orderId, string imageId)
        {
            // Create or reference an existing table
            CloudTable table = CreateTable();

            ImageProcessJobEntity job = new ImageProcessJobEntity(AccountId, orderId)
            {
                Status = "REQUEST",
                ImageId = imageId,
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            job = InsertOrMergeEntity(table, job);
        }

        public ImageProcessJobEntity Get(string orderId)
        {
            CloudTable table = CreateTable();

            TableOperation retrieveOperation = TableOperation.Retrieve<ImageProcessJobEntity>(AccountId, orderId);
            TableResult result = table.Execute(retrieveOperation);
            ImageProcessJobEntity job = result.Result as ImageProcessJobEntity;

            return job;
        }

        public void Update(ImageProcessJobEntity entity)
        {
            CloudTable table = CreateTable();
            InsertOrMergeEntity(table, entity);
        }

        /// <summary>
        /// Create a table for the sample application to process messages in. 
        /// </summary>
        /// <returns>A CloudTable object</returns>
        private CloudTable CreateTable()
        {
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            CloudTable table = tableClient.GetTableReference(TableName);
            try
            {
                if (table.CreateIfNotExists())
                {
                    Console.WriteLine("Created Table named: {0}", TableName);
                }
                else
                {
                    Console.WriteLine("Table {0} already exists", TableName);
                }
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("If you are running with the default configuration please make sure you have started the storage emulator. Press the Windows key and type Azure Storage to select and run it from the list of applications - then restart the sample.");
                throw;
            }

            return table;
        }

        /// <summary>
        /// Validate the connection string information in app.config and throws an exception if it looks like 
        /// the user hasn't updated this to valid values. 
        /// </summary>
        /// <param name="storageConnectionString">Connection string for the storage service or the emulator</param>
        /// <returns>CloudStorageAccount object</returns>
        private CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the application.");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }

        /// <summary>
        /// The Table Service supports two main types of insert operations. 
        ///  1. Insert - insert a new entity. If an entity already exists with the same PK + RK an exception will be thrown.
        ///  2. Replace - replace an existing entity. Replace an existing entity with a new entity. 
        ///  3. Insert or Replace - insert the entity if the entity does not exist, or if the entity exists, replace the existing one.
        ///  4. Insert or Merge - insert the entity if the entity does not exist or, if the entity exists, merges the provided entity properties with the already existing ones.
        /// </summary>
        /// <param name="table">The sample table name</param>
        /// <param name="entity">The entity to insert or merge</param>
        /// <returns></returns>
        private ImageProcessJobEntity InsertOrMergeEntity(CloudTable table, ImageProcessJobEntity entity)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

            TableResult result = table.Execute(insertOrMergeOperation);
            ImageProcessJobEntity inserted = result.Result as ImageProcessJobEntity;
            return inserted;
        }

    }
}
