using FraudAPI;
using FraudDomain.Model;
using FraudDomain.Request.Model;
using FraudDomain.Response.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;
using Xunit.Abstractions;

namespace FraudDomain.Service
{
    public class VisaApplicantFraudDetectionControllerTest
    {
        private readonly ITestOutputHelper output;
        private readonly HttpClient _client;

        private const string ReqContentType = "application/json";
        private const string FraudValidationReqUri = "/api/VisaApplication/validateApplicantIsFraudClear";
        private const string ApplicationId = "application-id";
        private const string FraudStatus = "fraud-status";
        private const string MatchingField = "matching-field";
        private const string CaseId = "case-id";

        public VisaApplicantFraudDetectionControllerTest(ITestOutputHelper output)
        {
            this.output = output;
            var server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async Task ShouldAcceptAnApplication()
        {
            //constructing the VisaApplicant request
            var visaApplicantAddress = new VisaApplicantRequestAddress{};
            var id = Guid.NewGuid().ToString();
            var visaApplicantRequest = new VisaApplicantRequest
            {
                Address = visaApplicantAddress,
                Id = id
            };
            var json = JsonConvert.SerializeObject(visaApplicantRequest);
            var requestBody = new StringContent(json, Encoding.UTF8, ReqContentType);

            var response = await _client.PostAsync(FraudValidationReqUri, requestBody);
            Assert.True(response.IsSuccessStatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var visaApplicantResponse = JsonConvert.DeserializeObject<VisaApplicantRespsonse>(body);

            //validate the ID that is sent is the same thats being returned.
            Assert.Equal(id, visaApplicantResponse.ApplicationId);
        }


        [Fact]
        public async Task ShouldCaptureAsFraudApplicant()
        {

            //constructing the VisaApplicant request
            var id = Guid.NewGuid().ToString();
            var visaApplicantRequestAddress = new VisaApplicantRequestAddress { Street = "111 Main", City = "Evanston", State = USState.IL, Zip = "60201" };
            var visaApplicantRequest = new VisaApplicantRequest
            {
                Address = visaApplicantRequestAddress,
                Id = id
            };

            var expectedVisaApplicantResponse = new VisaApplicantRespsonse { ApplicationId = id, FraudStatus = FraudulentAddressService.FRAUD_MATCHED, MatchingField = FraudulentAddressService.FRAUD_FIELD, CaseId = "CaseId111" };
            var json = JsonConvert.SerializeObject(visaApplicantRequest);
            var requestBody = new StringContent(json, Encoding.UTF8, ReqContentType);
            var response = await _client.PostAsync(FraudValidationReqUri, requestBody);
            Assert.True(response.IsSuccessStatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var visaApplicantResponse = JsonConvert.DeserializeObject<VisaApplicantRespsonse>(body);

            //validate the ID that is sent is the same thats being returned.
            Assert.Equal(expectedVisaApplicantResponse, visaApplicantResponse);
        }

        [Fact]
        public async Task ShouldCaptureAsFraudApplicantCasesInsensitively()
        {

            //constructing the VisaApplicant request
            var id = Guid.NewGuid().ToString();
            var visaApplicantRequestAddress = new VisaApplicantRequestAddress { Street = "111 MAIN", City = "EVANSTON", State = USState.IL, Zip = "60201" };
            var visaApplicantRequest = new VisaApplicantRequest
            {
                Address = visaApplicantRequestAddress,
                Id = id
            };


            var json = JsonConvert.SerializeObject(visaApplicantRequest);
            var requestBody = new StringContent(json, Encoding.UTF8, ReqContentType);

            var response = await _client.PostAsync(FraudValidationReqUri, requestBody);
            Assert.True(response.IsSuccessStatusCode);
            var body = await response.Content.ReadAsStringAsync();
            var actual = JObject.Parse(body);

            // Asserting the ID
            Assert.Equal(id, actual.SelectToken(ApplicationId));
            //Asserting the fraud status
            Assert.Equal(FraudulentAddressService.FRAUD_MATCHED, actual.SelectToken(FraudStatus));
            // Asserting  Matching -field 
            Assert.Equal(FraudulentAddressService.FRAUD_FIELD, actual.SelectToken(MatchingField));
            //Asserting the case-id
            Assert.Equal("CaseId111", actual.SelectToken(CaseId));
        }

        [Fact]
        public async Task ShouldCaptureAsNonFraudApplicant()
        {
            //constructing the VisaApplicant request as a String

            var id = Guid.NewGuid().ToString();
            var visaApplicantRequest = new JObject
            {
                {"id", id},
                {"address", new JObject
                {
                    {"street", "111 MAIN"},
                    {"city", "EVANSTON"},
                    {"state", "IL"},
                    {"zip", "60202"}
                }}
            };

            var requestBody = new StringContent(visaApplicantRequest.ToString(), Encoding.UTF8,ReqContentType);
            var response = await _client.PostAsync(FraudValidationReqUri, requestBody);

            var body = await response.Content.ReadAsStringAsync();
            var actual = JObject.Parse(body);
            var fraudStatus = actual.SelectToken(FraudStatus);
            var applicationId = actual.SelectToken(ApplicationId);
            Assert.Equal(id, applicationId);
            Assert.Equal(FraudulentAddressService.FRAUD_NOT_MATCHED, fraudStatus);

            output.WriteLine(body);
            Assert.DoesNotContain(MatchingField, body);
            Assert.DoesNotContain(CaseId, body);
        }
    }
}
