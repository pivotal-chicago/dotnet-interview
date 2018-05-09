using System;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<FraudulentAddressContext>(options => options.UseInMemoryDatabase("FraudDb"));
            services.AddTransient<FraudulentAddressService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
//            var repository = app.ApplicationServices.GetService<FraudulentAddressContext>();
//            AddressDataInitializer.Initialize(repository);
            app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}
