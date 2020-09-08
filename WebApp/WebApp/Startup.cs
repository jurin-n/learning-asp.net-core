using System;
using System.IO;
using System.Text.Json;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp.Models;

namespace WebApp
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(1);
            });

            var config = new AppConfig();
            if (_env.IsDevelopment() || _env.IsStaging())
            {
                System.Diagnostics.Debug.WriteLine("Dev or Staging");
                config.ConnectionString = Configuration["DevConfig:ConnectionString"];
                //config.ConnectionString = Configuration.GetConnectionString("dev");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Prod");
                //TODO::AWS ÇÃ Secret ManagerÇ©ÇÁÇ∆ÇÈÉçÉWÉbÉNé¿ëï
                String secret = getConnectionStringFromAWS();
                config = JsonSerializer.Deserialize<AppConfig>(secret);
            }
            services.AddSingleton(config);

            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = config.ConnectionString;
                options.SchemaName = "dbo";
                options.TableName = "AppCache";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public String getConnectionStringFromAWS() 
        {
            string secretName = "rds/sql-servver-express2";
            string region = "ap-northeast-1";
            string secret = "";

            MemoryStream memoryStream = new MemoryStream();

            IAmazonSecretsManager client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

            GetSecretValueRequest request = new GetSecretValueRequest();
            request.SecretId = secretName;
            request.VersionStage = "AWSCURRENT"; // VersionStage defaults to AWSCURRENT if unspecified.

            GetSecretValueResponse response = client.GetSecretValueAsync(request).Result;
            if (response.SecretString != null)
            {
                secret = response.SecretString;
            }
            return secret;
        }
    }
}
