using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;

using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace azure_sample.Controllers
{
    using Models;

    public class ProcessingController : ApiController
    {
        // GET api/processing/{id}
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response = Request.CreateResponse();

            MemoryStream ms = new MemoryStream();
            BlobModel.Download(id, ms);

            response.Content = new ByteArrayContent(ms.ToArray());
            // only jpeg
            response.Content.Headers.TryAddWithoutValidation("Content-Type", "image/jpeg");

            return response;
        }

        // POST api/processing
        public async Task<HttpResponseMessage> Post(string imageId = "", int processId = 0)
        {
            InputImageModel inputImageModel = new InputImageModel();
            InputImageEntity imageEntity = inputImageModel.Get(imageId);

            if (imageEntity == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            // send job
            QueueModel.SendMessage("hoge");

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

    }
}
