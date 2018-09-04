using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FraudDomain.Controllers
{
    public class FraudulentAddressControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public FraudulentAddressControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ShouldGetFraudulentAddresses()
        {
            var response = await _client.GetAsync("/api/FraudulentAddress");
            Assert.True(response.IsSuccessStatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var addresses = JsonConvert.DeserializeObject<List<FraudulentAddress>>(body);
            Assert.Equal(3, addresses.Count);
        }

        [Fact]
        public async Task ShouldGetOneFraudulentAddress()
        {
            var response = await _client.GetAsync("/api/FraudulentAddress/1");
            Assert.True(response.IsSuccessStatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var address = JsonConvert.DeserializeObject<FraudulentAddress>(body);
            Assert.Equal(1, address.Id);
        }

        [Fact]
        public async Task ShouldSaveaNewFraudulentAddress()
        {
            var fraudulentAddress = new FraudulentAddress
            {
                StreetNumber = "1234ZZZ",
                Street = "Sherman Avenue",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
//                CaseId = "CaseId1234"
            };
            var json = JsonConvert.SerializeObject(fraudulentAddress);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/FraudulentAddress", requestBody);

            var fromDb = TestStartup.FraudulentAddressContext.Addresses.First(addr => addr.StreetNumber.Equals("1234ZZZ"));
            fraudulentAddress.Id = fromDb.Id; // Hack to force Equality to work.
            Assert.Equal(fraudulentAddress, fromDb);
        }
    }
}
