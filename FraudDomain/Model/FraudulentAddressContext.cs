using Microsoft.EntityFrameworkCore;

namespace FraudDomain.Model
{
    public class FraudulentAddressContext : DbContext
    {
        public FraudulentAddressContext(DbContextOptions<FraudulentAddressContext> options)
            : base(options)
        {
        }

        public DbSet<FraudulentAddress> Addresses { get; set; }
    }
}