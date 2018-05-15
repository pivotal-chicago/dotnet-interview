using System.Collections.Generic;
using System.Net.Http;
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
    }

}
