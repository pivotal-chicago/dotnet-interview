using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FraudDomain.Controllers
{
    public class VisaCheckControllerTests
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        readonly IVisaApplicationChecker mockVisaApplicationChecker;
        private readonly MatchResponse expectMatchResponse;
        private Mock<IVisaApplicationChecker> mockBuilder;

        public VisaCheckControllerTests()
        {
            mockBuilder = new Mock<IVisaApplicationChecker>();

            expectMatchResponse = new MatchResponse
            {
                applicationId = "applicationId",
                caseId = "newCaseId",
                status = FraudStatus.Matched,
                matchingField = "Address"
            };
            mockBuilder.Setup(thing => thing.ValidateApplication(It.IsAny<VisaApplication>()))
                .Returns(expectMatchResponse);

            mockVisaApplicationChecker = mockBuilder.Object;

            server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>()
                .ConfigureServices(services => { services.AddSingleton(mockVisaApplicationChecker); }));
            client = server.CreateClient();
        }

        [Fact]
        public async Task ShouldSaveAnAddress()
        {
            var requestJson = @"
            {
                ""id"": ""applicationId"",
                ""address"": {
                    ""street"": ""1 Blah Ave."",
                    ""city"": ""Chicago"",
                    ""state"": ""IL"",
                    ""zip"": ""60654""
                }
            }
            ";
            var expectedResponseJson = @"
            {
                ""application-id"": ""applicationId"",
                ""fraud-status"": ""Matched"",
                ""matching-field"": ""Address"",
                ""case-id"": ""newCaseId""
            }
            ";

            var expectedJson = JObject.Parse(expectedResponseJson);

            var response = await client.PostAsync("/api/VisaCheck",
                new StringContent(requestJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var responseJson = JObject.Parse(responseString);

            Assert.Equal(expectedJson, responseJson);
            mockBuilder.Verify(service => service.ValidateApplication(It.IsAny<VisaApplication>()));
        }
    }
}