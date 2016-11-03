using System;
using Microsoft.ServiceBus.Messaging;
using System.Threading;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Models
{
    public class Order
    {
        public string ImageId { get; set; }

        public string OrderId { get; set; }

        public string ProcessId { get; set; }
    }

    public class QueueModel
    {

        private static CloudQueueClient queueClient;
        private static CloudQueue queue;
        const Int16 maxTrials = 4;

        public QueueModel() { }


        public static void Initialize()
        {

            // Retrieve storage account information from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            queueClient = storageAccount.CreateCloudQueueClient();
            queue = queueClient.GetQueueReference("orders");

            queue.CreateIfNotExists();
        }

        public static void SendMessage(string orderId, string imageId, string processId)
        {
            var message = new Order()
            {
                OrderId = orderId,
                ImageId = imageId,
                ProcessId = processId
            };

            while (true)
            {
                try
                {
                    queue.AddMessage(new CloudQueueMessage(JsonConvert.SerializeObject(message)));
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                    else
                    {
                        HandleTransientErrors(e);
                    }
                }
                break;
            }
        }

        private static void HandleTransientErrors(MessagingException e)
        {
            //If transient error/exception, let's back-off for 2 seconds and retry
            Console.WriteLine(e.Message);
            Console.WriteLine("Will retry sending the message in 2 seconds");
            Thread.Sleep(2000);
        }
    }
}
