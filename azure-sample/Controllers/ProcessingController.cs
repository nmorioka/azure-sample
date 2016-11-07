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
        public ImageProcessJobEntity Get(string id)
        {
            // TODO check job status..
            return ImageProcessJobModel.Get(id);
        }

        // POST api/processing
        public async Task<JobOrderDTO> Post([FromBody]ProcessOrderDTO processOrderDTO)
        {
            InputImageEntity imageEntity = InputImageModel.Get(processOrderDTO.ImageId);

            if (imageEntity == null)
            {
                // return this.Request.CreateResponse(HttpStatusCode.NotFound);
                return null;
            }

            Guid g = System.Guid.NewGuid();
            string orderId = g.ToString("N").Substring(0, 15);

            ImageProcessJobModel.Crate(orderId, imageEntity.RowKey);

            // send job
            QueueModel.SendMessage(orderId, processOrderDTO.ImageId, "default");

            return new JobOrderDTO() { OrderId = orderId};
        }

    }
}
