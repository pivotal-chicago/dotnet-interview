using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudAPI.Controllers;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace FraudDomain.Controllers
{
    public class FraudVisaApplicationControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private VisaApplication visaApplication;

        public FraudVisaApplicationControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
            visaApplication = new VisaApplication
            {
                Id = "123456",
                Address = new FraudulentAddress
                {
                    StreetNumber = "111",
                    Street = "Main",
                    City = "Evanston",
                    State = "IL",
                    ZIP = "60201"
                }
            };
        }
        
        [Fact]
        public async Task ShouldCheckFradulentVisaApplication()
        {
            var body = await SendRequestAndGetResponse();
            var result = JObject.Parse(body);
            Assert.Equal("123456", result["application-id"]);
            Assert.Equal("MATCHED", result["fraud-status"]);
            Assert.Equal("Address", result["matching-field"]);
            Assert.Equal("CaseId111", result["case-id"]);

        }

        [Fact]
        public async Task ShouldCheckNonFradulentVisaApplication()
        {
            visaApplication.Address.Street = "Some Other street";
            var body = await SendRequestAndGetResponse();
            var result = JObject.Parse(body);
            Assert.Equal("123456", result["application-id"]);
            Assert.Equal("CLEAR", result["fraud-status"]);
        }

        [Fact]
        public void ShouldValidateAddress()
        {
//            var mock = new Mock<FraudulentAddressService>();

//            var controller = new FraudVisaApplicationController(mock.Object);
//            VisaApplication application;
            //controller.Post(application);
        }

        private async Task<string> SendRequestAndGetResponse()
        {
            var json = JsonConvert.SerializeObject(visaApplication);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/CheckFradulentVisaApplication", requestBody);
            Assert.True(response.IsSuccessStatusCode);
            return await response.Content.ReadAsStringAsync();
        }


    }
}
