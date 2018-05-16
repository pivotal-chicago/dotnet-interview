using Xunit;

namespace FraudDomain.Model
{
    public class FraudulentAddressTests
    {
        [Fact]
        public void ShouldBeEqualIFFStreetIsEqual()
        {
            var address = new FraudulentAddress
            {
                Id = 3,
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            var address2 = new FraudulentAddress
            {
                Id = 3,
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

        [Fact]
        public void ShouldBeEqualIFFIdIsEqual()
        {
            var address = new FraudulentAddress
            {
                Id = 3,
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            var address2 = new FraudulentAddress
            {
                Id = 3,
                Street = "Main Street",
                StreetNumber = "123",
                City = "Chicago",
                State = "IL",
                ZIP = "60001"
            };

            Assert.Equal(address, address2);

            address2.Id = 4;
            Assert.NotEqual(address, address2);
        }
    }
}