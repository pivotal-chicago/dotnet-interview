using System.Collections.Generic;
using FraudDomain.Model;
using Microsoft.AspNetCore.Mvc;

namespace FraudAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/FraudulentAddress")]
    public class FraudulentAddressController : Controller
    {
        private readonly FraudulentAddressContext context;

        public FraudulentAddressController(FraudulentAddressContext context)
        {
            this.context = context;
        }

        // GET: api/FraudulentAddress
        [HttpGet]
        public IEnumerable<FraudulentAddress> Get()
        {
            return context.Addresses;
        }

        // GET: api/FraudulentAddress/5
        [HttpGet("{id}", Name = "Get")]
        public FraudulentAddress Get(int id)
        {
            return context.Find<FraudulentAddress>(id);
        }
        
        // POST: api/FraudulentAddress
        [HttpPost]
        public void Post([FromBody]FraudulentAddress value)
        {
            context.Update(value);
            context.SaveChanges();
        }
        
        // PUT: api/FraudulentAddress/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]FraudulentAddress value)
        {
            context.Add(value);
            context.SaveChanges();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var address = context.Find(typeof(FraudulentAddress), id);
            context.Remove(address);
            context.SaveChanges();
        }
    }
}
