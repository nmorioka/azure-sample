using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Models;

namespace azure_sample.Controllers
{
    public class ResultController : ApiController
    {

        // GET api/result/{id}
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response = Request.CreateResponse();

            MemoryStream ms = new MemoryStream();
            BlobModel.DownloadResult(id, ms);

            response.Content = new ByteArrayContent(ms.ToArray());
            // only jpeg
            response.Content.Headers.TryAddWithoutValidation("Content-Type", "image/png");

            return response;
        }

    }
}
