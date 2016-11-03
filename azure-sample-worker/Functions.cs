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
using Models;
using System;
using Utils;

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
            ImageProcessJobEntity jobEntity = ImageProcessJobModel.Get(order.OrderId);

            if (jobEntity == null)
            {
                Console.WriteLine("order is null");
                return;
            }
            if (jobEntity.Status != "REQUEST")
            {
                Console.WriteLine("order is already process");
                return;
            }

            jobEntity.Status = "PROCESSING";
            jobEntity.UpdateTime = DateTime.Now;
            ImageProcessJobModel.Update(jobEntity);

            // 2 download image
            System.IO.Stream stream = Storage.DownloadFileToStream(jobEntity.ImageId);

            // 3. execute and uplaod image
            Utils.ImageProcessor.execute(jobEntity.RowKey ,stream);

            // 4. record job
            jobEntity.Status = "DONE";
            jobEntity.UpdateTime = DateTime.Now;
            ImageProcessJobModel.Update(jobEntity);

            log.Write("This message couldn't be processed by the original function: " + order.OrderId);
        }
    }
}
