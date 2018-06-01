using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FraudAPI;
using FraudDomain.DTOs;
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
            //output.WriteLine("HUZZAHH!!!");
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
                ZIP = "60201"
            };
            var json = JsonConvert.SerializeObject(fraudulentAddress);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

            await _client.PostAsync("/api/FraudulentAddress", requestBody);

            var fromDb = TestStartup.FraudulentAddressContext.Addresses.First(addr => addr.StreetNumber == "1234ZZZ");
            fraudulentAddress.Id = fromDb.Id; // Hack to force Equality to work.
            Assert.Equal(fraudulentAddress, fromDb);
        }

        [Fact]
        public async Task IfAddressNoFoundShouldReturnAllClear()
        {
            //wiring up the http
            //serialization and deserialization


            //We need to make sure we are hitting the right endpoint> At least it should exist
            //The return response should be deserialized to our matching objects

            var visaApplication = new VisaApplicationDto()




            {
                Street = "123 Sherman Avenue",
                City = "Evanston",
                State = "IL",
                ZIP = "60201",
                Id = "SomeID"
            };
            var json = JsonConvert.SerializeObject(visaApplication);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/FraudulentAddress/Search", requestBody);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var body = await result.Content.ReadAsStringAsync();
            var applicationMatch = JsonConvert.DeserializeObject<ApplicationMatch>(body);
            Assert.NotNull(applicationMatch);
            Assert.Equal(eFraudStatus.CLEAR, applicationMatch.FraudStatus);
        }

        [Fact]
        public async Task IfAddressFoundShouldReturnAMatch()
        {
            //wiring up the http
            //serialization and deserialization


            //We need to make sure we are hitting the right endpoint> At least it should exist
            //The return response should be deserialized to our matching objects

            var visaApplication = new VisaApplicationDto()
            {
                Street = "2222 Main St",
                City = "Chicago",
                State = "IL",
                ZIP = "60602",
                Id = "testId"
            };
            var context = TestStartup.FraudulentAddressContext;
            context.Addresses.Add(new FraudulentAddress
            {
                CaseId = "caseid",
                City = visaApplication.City,
                State = visaApplication.State,
                StreetNumber = "2222",
                Street = "Main St",
                ZIP = "60602"
            });
            context.SaveChanges();

            var json = JsonConvert.SerializeObject(visaApplication);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await _client.PostAsync("/api/FraudulentAddress/Search", requestBody);

            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            var body = await result.Content.ReadAsStringAsync();
            Assert.Contains("MATCHED", body);
            var applicationMatch = JsonConvert.DeserializeObject<ApplicationMatch>(body);
            Assert.NotNull(applicationMatch);
            Assert.Equal(eFraudStatus.MATCHED, applicationMatch.FraudStatus);
        }
    }
}