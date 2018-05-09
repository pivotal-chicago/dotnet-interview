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

        [Fact]
        public void ShouldFindAMatch()
        {
            string street = "Main Street", streetnumber = "123", city = "Chicago", state = "IL", zip = "60001";

            var address = new FraudulentAddress { Street = street, StreetNumber = streetnumber, City = city, State = state, ZIP = zip };
            Assert.True(address.Match(street,streetnumber,city,state,zip));
            Assert.False(address.Match("<unkown street>",streetnumber,city,state,zip));
            Assert.False(address.Match(street,"<unkown streetnumber>",city,state,zip));
            Assert.False(address.Match(street,streetnumber,"<unkown city>",state,zip));
            Assert.False(address.Match(street,streetnumber,city,"<unkown state>",zip));
            Assert.False(address.Match(street,streetnumber,city,state,"<unkown zip>"));
        }

    }
}
