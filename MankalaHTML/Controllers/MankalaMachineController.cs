using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MankalaHTML.Controllers
{
    public class MankalaMachineController : ApiController
    {
        public static IList<MankalaMachine> MankalaMachines = new List<MankalaMachine>();

        // GET: api/MankalaMachine
        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET: api/MankalaMachine/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MankalaMachine
        public async Task<HttpResponseMessage> PostAsync([FromBody]string value)
        {
            var machine = new MankalaMachine();
            MankalaMachines.Add(machine);
            var machineId = MankalaMachines.IndexOf(machine);
            await machine.StartAsync(Request.RequestUri, machineId);
            var response = Request.CreateResponse(HttpStatusCode.Created, $"{{ 'ID' = {machineId}}}", "application/json");
            response.Headers.Location = new Uri(Request.RequestUri, $"MankalaMachine/{machineId}");
            return response;
        }

        // PUT: api/MankalaMachine/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MankalaMachine/5
        public void Delete(int id)
        {
            var machine = MankalaMachines[id];
            machine.Stop();
            MankalaMachines.Remove(machine);
        }
    }
}
