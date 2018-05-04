using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using NuGet.Frameworks;
using Xunit;

namespace FraudDomain.Controller
{
    public class VisaApplicationControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public VisaApplicationControllerTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<StartupTest>());
            _client = _server.CreateClient();
            
        }

        [Fact]
        public async  Task ShouldValidateAnApplicationAsync()
        {
            var response = await _client.GetAsync("api/FraudulentAddress");
            response.EnsureSuccessStatusCode();

            IEnumerable<FraudulentAddress> addresses = await response.Content.ReadAsAsync<IEnumerable<FraudulentAddress>>();
            var address = addresses.FirstOrDefault();

            var request = new VisaApplicationRequest
            {
                Id = "1234",
                Address = new Address
                {
                    Street = $"{address.StreetNumber} {address.Street}",
                    City = address.City,
                    State = address.State,
                    ZIP = address.ZIP
                }
            };

            response = await _client.PostAsJsonAsync("api/VisaApplication", request);
            response.EnsureSuccessStatusCode();

            VisaFraudResponse expectedResponse = new VisaFraudResponse
            {
                CaseId = address.CaseId,
                FraudStatus = FraudStatus.Matched,
                MatchingField = FraudulentAddressService.MatchingFieldAddress,
                ApplicationId = request.Id
            };

            VisaFraudResponse visaFraudResponse = await response.Content.ReadAsAsync<VisaFraudResponse>();

            Assert.Equal(expectedResponse, visaFraudResponse);
        }
    }
}
