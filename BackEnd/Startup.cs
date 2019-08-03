using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using BackEnd.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        //public all can access
        //get = read only
        //standard in core. in C# Type first. This method for webhosting environmental variables
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // Void called by runtime during start to add services to the container software architecture
        public void ConfigureServices(IServiceCollection services)
        {


           

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                //single file DB widely compatible
                options.UseSqlite("Data Source=conferences.db");

                // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                // {
                //     options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                // }
                // else
                // {
                //     options.UseSqlite("Data Source=conferences.db");
                // }
            });
            //adds MVC user interface framework for everything to be used 
            // sets compatibility with asp.net version 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(options =>
                options.SwaggerDoc("v1", new Info { Title = "Conference Planner API", Version = "v1" })
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // network communication framework get request address, S is for security prevent changes
        // pipeline using blocks 
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                //for dev 
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // only ssl communication 
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Conference Planner API v1")
            );

            app.UseHttpsRedirection();
            app.UseMvc();
            app.Run(context =>
            {
                context.Response.Redirect("/swagger");
                return Task.CompletedTask;
            });
        }
    }
}
