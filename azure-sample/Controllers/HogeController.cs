using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace azure_sample.Controllers
{
    public class HogeController : ApiController
    {
        // GET api/hoge
        public IEnumerable<string> Get()
        {
            return new string[] { "hoge1", "hoge2" };
        }

        // GET api/hoge/5
        public string Get(int id)
        {
            return "hoge";
        }

        // POST api/hoge
        public void Post([FromBody]string value)
        {
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
