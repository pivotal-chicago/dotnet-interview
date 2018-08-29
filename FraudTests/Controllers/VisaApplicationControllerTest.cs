using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FraudDomain.Controllers
{
    public class VisaApplicationControllerTest
    {
        private readonly ITestOutputHelper _output;
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public VisaApplicationControllerTest(ITestOutputHelper outputHelper)
        {
            _output = outputHelper;
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task ShouldValidateApplicationWhenAddressIsValid()
        {
            var requestJson = new StringContent(@"{  
  ""Id"":1234,
   ""Address"":{  
      ""Street"":""123 Good Street"",
      ""City"":""Chicago"",
      ""State"":""IL"",
      ""Zipcode"":""60001""
	}
}
", Encoding.UTF8, "application/json");

            var json = JsonConvert.SerializeObject(new VisaApplication
            {
                Id = 1234,
                Address = new ApplicationAddress
                {
                    City = "Evanston",
                    State = "IL",
                    Street = "111 Main",
                    Zipcode = "60201"
                }
            });

            var response = await _client.PostAsync("api/validate", new StringContent(json, Encoding.UTF8, "application/json"));
            Assert.True(response.IsSuccessStatusCode);

            var actualValidationResponse = await response.Content.ReadAsStringAsync();
            var expectedValidationResponse = "\"something\"";

            Assert.Equal(expectedValidationResponse, actualValidationResponse);
        }
    }
}