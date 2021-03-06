﻿using FraudAPI.Database;
using FraudDomain.Model;
using FraudDomain.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FraudAPI
{
    public class TestStartup
    {
        public static FraudulentAddressContext FraudulentAddressContext;

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddDbContext<FraudulentAddressContext>(options => options.UseSqlite(connection));
//            services.AddDbContext<FraudulentAddressContext>(options => options.UseInMemoryDatabase("TestDB"));
            services.AddTransient<FraudulentAddressService>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseMvc();
            var context = app.ApplicationServices.GetService<FraudulentAddressContext>();
            AddreessDataInitializer.Initialize(context);
            FraudulentAddressContext = context;
        }
    }
}