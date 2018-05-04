using Xunit;

namespace FraudDomain.Model
{
    public class FraudulentAddressTests
    {
        [Fact]
        public void ShouldBeEqualWhenItMakesSense()
        {
            var address = new FraudulentAddress
            {
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            var address2 = new FraudulentAddress
            {
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            Assert.Equal(address, address2);

            address2.Street = "Maple Avenue";
            Assert.NotEqual(address, address2);
        }
    }
}
