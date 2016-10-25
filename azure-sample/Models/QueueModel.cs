using System;
using System.Collections.Generic;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using System.Threading;

namespace azure_sample.Models
{
    public class QueueModel
    {

        private static QueueClient queueClient;
        private static string QueueName = "JobQueue";
        const Int16 maxTrials = 4;

        public QueueModel() { }


        public static void Initialize()
        {
            var namespaceManager = NamespaceManager.Create();

            if (namespaceManager.QueueExists(QueueName) == false)
            {
                namespaceManager.CreateQueue(QueueName);
            }

            queueClient = QueueClient.Create(QueueName);
        }

        public static void SendMessage(string serializedMessage)
        {
            // List<BrokeredMessage> messageList = new List<BrokeredMessage>();

            // messageList.Add(CreateSampleMessage("1", "First message information"));
            // messageList.Add(CreateSampleMessage("2", "Second message information"));
            // messageList.Add(CreateSampleMessage("3", "Third message information"));

            var message = CreateSampleMessage("1", serializedMessage);

            while (true)
            {
                try
                {
                    queueClient.Send(message);
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

        private static BrokeredMessage CreateSampleMessage(string messageId, string messageBody)
        {
            BrokeredMessage message = new BrokeredMessage(messageBody);
            message.MessageId = messageId;
            return message;
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
