using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FraudDomain.Controller
{
    public class FraudulentAddressControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        private readonly ITestOutputHelper output;


        public FraudulentAddressControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ShouldMatchApplication()
        {
            var address = new FraudulentAddress
            {
                StreetNumber = "111",
                Street = "Main",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
                CaseId = "SomeCaseId1"
            };
            var content = new StringContent(JsonConvert.SerializeObject(address), Encoding.UTF8, "application/json");

            var postResult = await _client.PostAsync("/api/FraudulentAddress", content);

            var result = _client.GetAsync("/api/FraudulentAddress").Result;
            Assert.Equal(actual: result.StatusCode, expected: HttpStatusCode.OK);
            output.WriteLine(result.Content.ReadAsStringAsync().Result);
        }
    }
}
