using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Models
{

    public class InputImageModel
    {

        private const string TableName = "inputImage";
        private const string AccountId = "Account1";

        private static CloudStorageAccount cloudStorageAccount;


        private InputImageModel() { }

        public static void Init(CloudStorageAccount account)
        {
            cloudStorageAccount = account;
        }

        /// <summary>
        /// Demonstrate basic Table CRUD operations. 
        /// </summary>
        /// <param name="table">The sample table</param>
        public static void Create(string fileName)
        {
            CloudTable table = CreateTable();

            InputImageEntity inputImage = new InputImageEntity(AccountId, fileName)
            {
                Valid = true,
                CreateTime = DateTime.Now
            };

            inputImage = InsertOrMergeEntity(table, inputImage);
        }

        public static InputImageEntity Get(string fileName)
        {
            CloudTable table = CreateTable();

            TableOperation retrieveOperation = TableOperation.Retrieve<InputImageEntity>(AccountId, fileName);
            TableResult result = table.Execute(retrieveOperation);
            InputImageEntity inputImage = result.Result as InputImageEntity;

            return inputImage;
        }

        /// <summary>
        /// Create a table for the sample application to process messages in. 
        /// </summary>
        /// <returns>A CloudTable object</returns>
        private static CloudTable CreateTable()
        {
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient();

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
        /// The Table Service supports two main types of insert operations. 
        ///  1. Insert - insert a new entity. If an entity already exists with the same PK + RK an exception will be thrown.
        ///  2. Replace - replace an existing entity. Replace an existing entity with a new entity. 
        ///  3. Insert or Replace - insert the entity if the entity does not exist, or if the entity exists, replace the existing one.
        ///  4. Insert or Merge - insert the entity if the entity does not exist or, if the entity exists, merges the provided entity properties with the already existing ones.
        /// </summary>
        /// <param name="table">The sample table name</param>
        /// <param name="entity">The entity to insert or merge</param>
        /// <returns></returns>
        private static InputImageEntity InsertOrMergeEntity(CloudTable table, InputImageEntity entity)
        {
            // Create the InsertOrReplace  TableOperation
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);

            // Execute the operation.
            TableResult result = table.Execute(insertOrMergeOperation);
            Console.WriteLine(result);
            InputImageEntity insertedCustomer = result.Result as InputImageEntity;
            return insertedCustomer;
        }

    }
}
