using System;
using System.Collections.Generic;
using System.Text;
using FraudAPI.Controllers;
using FraudDomain.Service;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace FraudDomain.Controllers
{
    
    public class VisaApplicationControllerTests
    {
        [Fact]
        public void ShouldReturnMATCHEDWhenAddressMatchIsFound()
        {
            string caseId = "12354654";
            var validatorMock = new Mock<IVisaValidator>();
            
            JObject visaApplication = new JObject
            {
                {"id", "appId1234"},
                {
                    "address", new JObject()
                    {
                        {"street", "123 Main Street"},
                        {"city", "Chicago"},
                        {"state", "IL"},
                        {"zip", "60001"}
                    }
                }
            };

            JObject expectedResult = new JObject
            {
                { "application-id" , "appId1234" },
                { "fraud-status" , "MATCHED" },
                { "matching-field" , "ADDRESS" },
                { "case-id", caseId}
            };

            validatorMock.Setup((v => v.Validate(visaApplication))).Returns(caseId);
            IVisaValidator validator = validatorMock.Object;
            VisaApplicationController visaController = new VisaApplicationController(validator);

            var result = visaController.Verify(visaApplication);
            
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void ShouldReturnCLEARWhenAddressMatchIsNotFound()
        {
            string caseId = "";
            var validatorMock = new Mock<IVisaValidator>();
            
            JObject visaApplication = new JObject
            {
                {"id", "appId12345"},
                {
                    "address", new JObject()
                    {
                        {"street", "123 Main Street"},
                        {"city", "Chicago"},
                        {"state", "IL"},
                        {"zip", "60001"}
                    }
                }
            };

            JObject expectedResult = new JObject
            {
                { "application-id" , "appId12345" },
                { "fraud-status" , "CLEAR" }
            };

            validatorMock.Setup((v => v.Validate(visaApplication))).Returns(caseId);
            IVisaValidator validator = validatorMock.Object;
            VisaApplicationController visaController = new VisaApplicationController(validator);

            var result = visaController.Verify(visaApplication);
            
            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void ThisIsReallyDangerous()
        {
            string caseId = "";
            var validatorMock = new Mock<IVisaValidator>();
            
            JObject matchingApplication = new JObject
            {
                {"id", "appId12345"},
                {
                    "address", new JObject()
                    {
                        {"street", "123 Main Street"},
                        {"city", "Chicago"},
                        {"state", "IL"},
                        {"zip", "60001"}
                    }
                }
            };

            JObject passingApplication = new JObject
            {
                {"id", "appId12345"},
                {
                    "address", new JObject()
                    {
                        {"street", "123 Main Street"},
                        {"city", "Chicago"},
                        {"state", "IL"},
                        {"zip", "60001"}
                    }
                }
            };

            validatorMock.Setup((v => v.Validate(matchingApplication))).Returns("Ma5hingCaseId");
            IVisaValidator validator = validatorMock.Object;
            VisaApplicationController visaController = new VisaApplicationController(validator);
            visaController.Verify(matchingApplication);
            var passing = visaController.Verify(passingApplication);
            Assert.DoesNotContain("matching-field", passing.ToString());
        }
    }
}
