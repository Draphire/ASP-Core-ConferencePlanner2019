using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FrontEnd.Services;
using System.Net.Http;
using Microsoft.IdentityModel.Protocols;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

namespace FrontEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private static bool ValidateRemoteCertificate(
  object sender,
  X509Certificate certificate,
  X509Chain chain,
  SslPolicyErrors policyErrors)
{
    // Logic to determine the validity of the certificate
    return true;
}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

        //       services.AddHttpClient("HttpClientWithSSLUntrusted").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        //   {
        //     ClientCertificateOptions = ClientCertificateOption.Manual,
        //     ServerCertificateCustomValidationCallback =
        //     (httpRequestMessage, cert, cetChain, policyErrors) =>
        //     {
        //         return true;
        //     }     });



        // ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(
        //         ValidateRemoteCertificate);
            //Http Client with base URL to point to BackEnd Apllication
            services.AddHttpClient<IApiClient, ApiClient>(client =>
            {
                // client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = 
                //     new X509ServiceCertificateAuthentication()
                //     {
                //         CertificateValidationMode = X509CertificateValidationMode.None,
                //         RevocationMode = X509RevocationMode.NoCheck
                //     };

                //               HttpClientHandler clientHandler = new HttpClientHandler();
                // clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

                // // Pass the handler to httpclient(from you are calling api)
                //  client = new HttpClient(clientHandler);
                


                client.BaseAddress = new Uri("https://localhost:44342/");
                //client.BaseAddress = new Uri("https://localhost:5001/");
                // client.BaseAddress = new Uri(Configuration["ServiceUrl"]);
        
         
         
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc();
        }
    }
}
