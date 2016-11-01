//----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//----------------------------------------------------------------------------------

using System.IO;
using Microsoft.Azure.WebJobs;
using Dto;
using WorkerEnvironment;

namespace azure_sample_worker
{
    public class Functions
    {
        /// <summary>
        /// This function will be invoked when a message ends up in the poison queue
        /// </summary>
        public static void BindToPoisonQueue([QueueTrigger(QueueName.JOB_QUEUE_NAME)] Order order, TextWriter log)
        {
            // 1. read job

            // 2 download image
//            System.IO.Stream stream = Utils.Storage.DownloadFileToStream("hoge.jpg");

            // 3. execute and uplaod image
//            Utils.ImageProcessor.execute(stream);

            // 4. record job

            log.Write("This message couldn't be processed by the original function: " + order.Name);
        }
    }
}
