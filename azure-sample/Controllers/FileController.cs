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
    public class InputImageDTO
    {
        public string ImageId { get; set; }
    }

    public class FileController : ApiController
    {
        // GET api/file
        public IEnumerable<string> Get()
        {
            return new string[] { "hoge1", "hoge2" };
        }

        // GET api/file/{id}
        public HttpResponseMessage Get(string id)
        {
            HttpResponseMessage response = Request.CreateResponse();

            // TODO check table
            InputImageModel imageModel = new InputImageModel();
            InputImageEntity imageEntry = imageModel.Get(id);

            if (imageEntry == null)
            {
                return this.Request.CreateResponse(HttpStatusCode.NotFound);
            }

            MemoryStream ms = new MemoryStream();
            BlobModel.Download(id, ms);

            response.Content = new ByteArrayContent(ms.ToArray());
            // only jpeg
            response.Content.Headers.TryAddWithoutValidation("Content-Type", "image/jpeg");

            return response;
        }

        // POST api/file
        public async Task<IEnumerable<InputImageDTO>> Post()
        {
            var tempPath = Path.GetTempPath();
            var provider = new MultipartFormDataStreamProvider(tempPath);

            await this.Request.Content.ReadAsMultipartAsync(provider);

            var list = new List<InputImageDTO>();

            foreach (var file in provider.FileData)
            {
                // アップロードファイル名の取得
                // var fileName = file.Headers.ContentDisposition.FileName;
                // fileName = fileName.StartsWith("\"") || fileName.StartsWith("'") ? fileName.Substring(1, fileName.Length - 1) : fileName;
                // fileName = fileName.EndsWith("\"") || fileName.EndsWith("'") ? fileName.Substring(0, fileName.Length - 1) : fileName;
                // fileName = Path.GetFileName(fileName);

                // ファイル名は乱数を入れておく
                Guid g = System.Guid.NewGuid();
                string imageId = g.ToString("N").Substring(0, 15);

                // ファイルの移動
                // File.Move(file.LocalFileName, Path.Combine("C:\\temp\\", fileName));
                BlobModel.Upload(imageId, file.LocalFileName);

                InputImageModel inputImageModel = new InputImageModel();
                inputImageModel.Create(imageId);

                list.Add(new InputImageDTO()
                    {
                        ImageId = imageId
                    });
            }

            return list;
        }

        // PUT api/file/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/file/5
        public void Delete(int id)
        {
        }

    }
}
