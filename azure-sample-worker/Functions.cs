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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace azure_sample_worker
{
    public class Order
    {
        public string Name { get; set; }

        public string OrderId { get; set; }
    }

    public class Functions
    {
        /// <summary>
        /// This function will be invoked when a message ends up in the poison queue
        /// </summary>
        public static void BindToPoisonQueue([QueueTrigger("initialorder")] Order order, TextWriter log)
        {
            log.Write("This message couldn't be processed by the original function: " + order.Name);
        }
    }
}
