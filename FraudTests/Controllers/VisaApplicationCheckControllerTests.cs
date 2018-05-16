using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudAPI.Controllers;
using FraudDomain.Dto;
using FraudDomain.Model;
using FraudDomain.Service;
using Moq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace FraudDomain.Controllers
{
    public class VisaApplicationCheckControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private Mock<IVisaApplicationCheckerService> mockService;

        const string ENDPOINT = "/api/visa-application/check";

        public VisaApplicationCheckControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            mockService = new Mock<IVisaApplicationCheckerService>();
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IVisaApplicationCheckerService>(mockService.Object);
                }));
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task EndpointShouldRespond()
        {
            var response = await _client.PostAsync(ENDPOINT, DefaultContent());
            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task EndpointShouldAcceptApplication()
        {
            mockService.Setup(service => service.validate(It.IsAny<VisaApplication>())).Returns(new MatchResult());

            var response = await _client.PostAsync(ENDPOINT, DefaultContent());
            Assert.True(response.IsSuccessStatusCode);

            mockService.Verify(service => service.validate(It.IsAny<VisaApplication>()), Times.Exactly(1));
        }

        private static StringContent DefaultContent(VisaApplication app = null)
        {
            var application = app ?? DefaultApplication();
            string jsonApplication = JsonConvert.SerializeObject(application);
            var content = new StringContent(jsonApplication, Encoding.UTF8, "application/json");
            return content;
        }

        private static VisaApplication DefaultApplication()
        {
            return new VisaApplication {Id = "3", Address = new Address {City = "Chicago", ZIP = "60001"}};
        }

        [Fact]
        // this is bad name
        public async Task FraudulentApplicationReturnsFraudulentMatchResult()
        {
            var expectedResult = new MatchResult
            {
                ApplicationId = "3",
                FraudStatus = FraudStatus.Matched,
                MatchingField = Fields.Address,
                CaseId = "13"
            };

            var application = DefaultApplication();
            mockService.Setup(service => service.validate(It.IsAny<VisaApplication>())).Returns(expectedResult);

            HttpResponseMessage response = await _client.PostAsJsonAsync(ENDPOINT, DefaultContent(application));
            var matchResult = JsonConvert.DeserializeObject<MatchResult>(await response.Content.ReadAsStringAsync());
            Assert.Equal(expectedResult, matchResult);
        }
    }
}