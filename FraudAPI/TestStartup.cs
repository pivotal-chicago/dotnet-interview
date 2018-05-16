using FraudAPI.Controllers;
using FraudAPI.Database;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FraudAPI
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<FraudulentAddressContext>(options => options.UseInMemoryDatabase("TestDB"));
            services.AddTransient<FraudulentAddressService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc();
            var context = app.ApplicationServices.GetService<FraudulentAddressContext>();
            AddreessDataInitializer.Initialize(context);
        }
    }
}