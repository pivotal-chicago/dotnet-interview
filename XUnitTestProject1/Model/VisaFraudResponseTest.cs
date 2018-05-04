using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FraudDomain.Model
{
    public class VisaFraudResponseTest
    {


        [Fact]
        public void ReturnTrueIfEqual()
        {
            VisaFraudResponse response1 = new VisaFraudResponse
            {
                CaseId = "1",
                MatchingField = "A",
                FraudStatus = FraudStatus.Clear,
                ApplicationId = "123"
            };

            VisaFraudResponse response2 = new VisaFraudResponse
            {
                CaseId = "1",
                MatchingField = "A",
                FraudStatus = FraudStatus.Clear,
                ApplicationId = "123"
            };

            Assert.Equal(response1, response2);
        }

        [Fact]
        public void ReturnFalseIfNotEqual()
        {
            VisaFraudResponse response1 = new VisaFraudResponse
            {
                CaseId = "1",
                MatchingField = "A",
                FraudStatus = FraudStatus.Clear,
                ApplicationId = "123"
            };

            VisaFraudResponse response2 = new VisaFraudResponse
            {
                CaseId = "2",
                MatchingField = "A",
                FraudStatus = FraudStatus.Clear,
                ApplicationId = "123"
            };

            Assert.NotEqual(response1, response2);
        }

    }
}
