using System.Collections.Generic;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/FraudulentAddress")]
    public class FraudulentAddressController : Controller
    {
        private readonly FraudulentAddressService service;

        public FraudulentAddressController(FraudulentAddressService service)
        {
            this.service = service;
        }

        // GET: api/FraudulentAddress
        [HttpGet]
        public IEnumerable<FraudulentAddress> Get()
        {
            return service.All();
        }

        // GET: api/FraudulentAddress/5
        [HttpGet("{id}", Name = "Get")]
        public FraudulentAddress Get(int id)
        {
            return service.Find(id);
        }
        
        // POST: api/FraudulentAddress
        [HttpPost]
        public void Post([FromBody]FraudulentAddress value)
        {
            service.Save(value);
        }
        
        // PUT: api/FraudulentAddress/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]FraudulentAddress value)
        {
            service.Save(value);
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.Delete(id);
        }
    }
}
