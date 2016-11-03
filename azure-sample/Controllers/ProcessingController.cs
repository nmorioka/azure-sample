using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Threading.Tasks;
using Models;
using System;

namespace azure_sample.Controllers
{
    public class ProcessOrderDTO
    {
        public string ImageId { get; set; }
        //public string ProcessId { get; set; }
    }

    public class JobOrderDTO
    {
        public string OrderId { get; set; }
    }

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
        public async Task<JobOrderDTO> Post([FromBody]ProcessOrderDTO processOrderDTO)
        {
            InputImageModel inputImageModel = new InputImageModel();
            InputImageEntity imageEntity = inputImageModel.Get(processOrderDTO.ImageId);

            if (imageEntity == null)
            {
                // return this.Request.CreateResponse(HttpStatusCode.NotFound);
                return null;
            }

            ImageProcessJobModel imageProcessJobModel = new ImageProcessJobModel();
            Guid g = System.Guid.NewGuid();
            string orderId = g.ToString("N").Substring(0, 15);

            imageProcessJobModel.Crate(orderId, imageEntity.RowKey);

            // send job
            QueueModel.SendMessage(orderId, processOrderDTO.ImageId, "default");

            return new JobOrderDTO() { OrderId = orderId};
        }

    }
}
