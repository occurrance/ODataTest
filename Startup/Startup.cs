using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Linq;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNetCore.Mvc;
using ODataTest.Models;
using Microsoft.OData.Edm;
using Microsoft.EntityFrameworkCore;

namespace OdataTest
{
    /// <summary>
    /// OData Test Startup Class
    /// </summary>
    public class Startup
    {

        /// <summary>
        /// Startup Function
        /// </summary>
        /// <param name="env">Dependency Injection of IHostingEnvironment</param>
        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
            
        }
        /// <summary>
        /// Configuration Data
        /// </summary>
        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {                       
            services.AddMemoryCache();
        
            
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var manager = new ApplicationPartManager();
            manager.ApplicationParts.Add(new AssemblyPart(typeof(Startup).Assembly));
            services.AddSingleton(manager);


            // Add framework services.
            services.AddDbContext<BookStoreContext>(opt => opt.UseInMemoryDatabase("BookLists"));
            services.AddOData();
            var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_0); ;

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="prov"></param>        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider prov)
        {
            
            var logger = loggerFactory.CreateLogger("Configure");
            var startMsg = $"Configuring OdataTest for '{ env.EnvironmentName }' ";

            logger.LogInformation(startMsg);

            var odataBuilder = new ODataConventionModelBuilder(app.ApplicationServices);
                      

            app.UseMvc(routebuilder =>
            {
                //Enable avaiable OData Features
                routebuilder.Select().Filter().OrderBy().Expand().Count().MaxTop(null);

                //Workaround: https://github.com/OData/WebApi/issues/1175
                routebuilder.EnableDependencyInjection();

                //Set Odata Base Route
                routebuilder.MapODataServiceRoute("odata", "odata", GetEdmModel(odataBuilder));
                

            });


        }

        private static IEdmModel GetEdmModel(ODataConventionModelBuilder builder)
        {
            builder.EntitySet<Book>("Books");
            builder.EntitySet<Press>("Presses");
            return builder.GetEdmModel();
        }
    }
}
