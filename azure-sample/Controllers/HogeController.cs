using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace azure_sample.Controllers
{
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Models;

    public class HogeController : ApiController
    {
        // GET api/hoge
        public IEnumerable<string> Get()
        {
            const string ImageToUpload = "c:\\hoge.png";

            BlobModel blobModel = new BlobModel();
            blobModel.upload(ImageToUpload, ImageToUpload);

            return new string[] { "hoge1", "hoge2" };
        }

        // GET api/hoge/5
        public string Get(int id)
        {
            return "hoge";
        }

        // POST api/hoge
        public async Task<HttpResponseMessage> Post(bool overwrite = false)
        {
            Console.WriteLine("write...");

            var tempPath = Path.GetTempPath();
            var provider = new MultipartFormDataStreamProvider(tempPath);

            await this.Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.FileData)
            {
                // アップロードファイル名の取得
                // var fileName = file.Headers.ContentDisposition.FileName;
                // fileName = fileName.StartsWith("\"") || fileName.StartsWith("'") ? fileName.Substring(1, fileName.Length - 1) : fileName;
                // fileName = fileName.EndsWith("\"") || fileName.EndsWith("'") ? fileName.Substring(0, fileName.Length - 1) : fileName;
                // fileName = Path.GetFileName(fileName);

                // ファイル名は乱数を入れておく
                Guid g = System.Guid.NewGuid();
                string pass = g.ToString("N").Substring(0, 15);

                // ファイルの移動
                // File.Move(file.LocalFileName, Path.Combine("C:\\temp\\", fileName));
                BlobModel blobModel = new BlobModel();
                blobModel.upload(pass, file.LocalFileName);

                InputImageModel inputImageModel = new InputImageModel();
                inputImageModel.CrateAsync(pass);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        // PUT api/hoge/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/hoges/5
        public void Delete(int id)
        {
        }

    }
}
