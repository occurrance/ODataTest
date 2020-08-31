using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODataTest.Controllers
{
    /// <summary>
    /// Controller class for UserProfileCollection type.
    /// This class will not be overwritten by CSLA Generator.
    /// Use this class to override defaults.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[ResponseCache(CacheProfileName = "DefaultWebApiGet")]
    public class WebApiDebugController : Controller    {

        private const string CI_BUILD_VERSION = "[CI_BUILD_VERSION]";

        //private ICoreAppService _appService; ApiControllerBase
        //private IConfigManger _configManager;
        private IHostingEnvironment _env;
        
        /// <summary>
        /// Check if Web API is up and running
        /// </summary>
        /// <returns>A JSON of true with OK (200) response.</returns>
        [HttpGet("/api/GetWebApiIsAvailable")]
        //[SwaggerResponse(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, Description = "Successful fetch (OK response)", Type = typeof(Boolean))]
        public virtual IActionResult GetWebApiIsAvailable()
        {
            return this.Ok(true);
        }


        /// <summary>
        /// Retrieve App Service's Current App Manifest
        /// </summary>
        /// <returns>A JSON of the App Manifest object with OK (200) response.</returns>
        [HttpGet("")]
        //[SwaggerResponse(Microsoft.AspNetCore.Http.StatusCodes.Status200OK, Description = "Successful fetch (OK response)", Type = typeof(WebApiDebugInfo))]
        public virtual IActionResult GetWebApiDebugInfo()
        {
            try
            {
                var retVal = new WebApiDebugInfo();

                try
                {

                    retVal.Exception = string.Empty;
                    retVal.WebServerName = System.Environment.MachineName;
                    retVal.ServiceAccountUser = System.Environment.UserDomainName + "\\" + System.Environment.UserName;
                    retVal.DotNetCoreEnvironment = _env.EnvironmentName;
                    retVal.BuildVersion = CI_BUILD_VERSION;

                }
                catch (Exception ex)
                {
                    retVal.Exception = ex.ToString();
                }

                return this.Ok(retVal);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Class to hold Web API Debugging Information
        /// </summary>
        public class WebApiDebugInfo
        {
            /// <summary>
            /// Service account Web-Service process is running under
            /// </summary>
            public string ServiceAccountUser { get; set; }
            /// <summary>
            /// Computer name web-server is running on
            /// </summary>
            public string WebServerName { get; set; }
            /// <summary>
            /// SQL Server Instance Configured in Config File
            /// </summary>
            public string DatabaseInstance { get; set; }
            /// <summary>
            /// SQL Database Name configured in Config File
            /// </summary>
            public string DatabaseName { get; set; }

            /// <summary>
            /// Environment Name returned from SQL Instance
            /// </summary>
            public string SQLEnvironmentName { get; set; }

            /// <summary>
            /// Value of Environment Variable for .Net Core Environment
            /// </summary>
            public string DotNetCoreEnvironment { get; set; }


            /// <summary>
            /// SQL Instance Name returned from SQL Server
            /// </summary>
            public string SQLInstanceName { get; set; }
            /// <summary>
            /// Database Name returned from SQL Server
            /// </summary>
            public string SQLDatabaseName { get; set; }

            /// <summary>
            /// Build Version Stamped by Build Process
            /// </summary>
            public string BuildVersion { get; set; }


            /// <summary>
            /// Exception Detail returned by this process
            /// </summary>
            public string Exception { get; set; }
        }

    }
}
