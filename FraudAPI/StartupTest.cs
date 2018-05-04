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
    public class StartupTest
    {
        public StartupTest(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<FraudulentAddressContext>(options => options.UseInMemoryDatabase("testDb"));
            services.AddTransient<FraudulentAddressService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            var fraudulentAddressContext = app.ApplicationServices.GetService<FraudulentAddressContext>();
            AddreessDataInitializer.Initialize(fraudulentAddressContext);
        }
    }
}
