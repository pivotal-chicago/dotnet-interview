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

        [HttpGet]
        public IEnumerable<FraudulentAddress> Get()
        {
            return service.All();
        }

        [HttpGet("{id}", Name = "Get")]
        public FraudulentAddress Get(int id)
        {
            return service.Find(id);
        }
        
        [HttpPost]
        public void Post([FromBody]FraudulentAddress value)
        {
            service.Save(value);
        }
        
        [HttpPost]
        public void Match([FromBody]Applicant value)
        {
            var address = value.address;
            service.MatchAddress(address.street, address.city, address.state, address.zip);
        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]FraudulentAddress value)
        {
            service.Save(value);
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.Delete(id);
        }
    }
}
