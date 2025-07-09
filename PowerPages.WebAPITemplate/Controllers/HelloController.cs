using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

/////////////
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using PowerPages.WebAPITemplate.API.Utils;
using System.Text;
//using PowerPages.WebAPITemplate.Utils;
using PowerPages.WebAPITemplate.API.Models.Dataverse;

using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Runtime;
using System.Text.RegularExpressions;
using Azure;

using PowerPages.WebAPITemplate.API.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PowerPages.WebAPITemplate.APIControllers
{
    [Route("[controller]")]

    //[ApiController]
    public class HelloController : ControllerBase
    {
        private IConfiguration Configuration;
       // private IKeyVaultWorker KeyVault;
     //   private String FileAPI_APIMKey;
              private DataverseService dv;

        public HelloController(IConfiguration _configuration)//, IKeyVaultWorker _kvWorker
        {
            Configuration = _configuration;
            //KeyVault = _kvWorker;
           // AllowedFileExtensions = Configuration["PowerPages:AllowedFileUploadExtensions"] ?? "pdf";
            //FileAPI_APIMKey = this.KeyVault.GetSecretAsync("sharepointapi-apimkey").Result;
            //FileAPIUrl = this.Configuration["PowerPages:FileAPIUrl"];
            //FileAPIUrl = FileAPIUrl.EndsWith("/") ? FileAPIUrl.Substring(FileAPIUrl.Length - 1) : FileAPIUrl;
         //   dv = new DataverseService(_configuration, _kvWorker);
        }

        private String GetCurrentUserEmail()
        {
            var user = HttpContext.User;

            if (!(user is ClaimsPrincipal claim)) return "";

            var claimInfo = claim.FindFirst("lp_sdes");
            var email = claim.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            return email ?? "";
        }                     
       

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("/api/Hello"), Authorize]
        public async Task<IActionResult> Hello()
        {
            var output = new { email = GetCurrentUserEmail() };
            return new OkObjectResult(output);
        }      
   
    }
}
