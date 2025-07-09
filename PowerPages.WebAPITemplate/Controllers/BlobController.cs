using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using PowerPages.WebAPITemplate.API.Models;
using PowerPages.WebAPITemplate.API.Models.Dataverse;
using PowerPages.WebAPITemplate.API.Utils;
using PowerPages.WebAPITemplate.Models;
using System.Net;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PowerPages.WebAPITemplate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private IConfiguration Configuration;
        private BlobContainerClient BlobContainerClient;
        private IKeyVaultService KeyVault; 
        private DataverseService dv;
        String strBlobContainerPath;
        private String GetCurrentUserEmail()
        {
            var user = HttpContext.User;

            if (!(user is ClaimsPrincipal claim)) return "";

            var claimInfo = claim.FindFirst("lp_sdes");
            var email = claim.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;

            return email ?? "";
        }

        public BlobController(IConfiguration _configuration, IKeyVaultService keyVault)
        {
            Configuration = _configuration;
            
            strBlobContainerPath = Configuration["PowerPages:BlobAccountUri"] + "/powerpagesintegration";
            BlobContainerClient = new BlobContainerClient(keyVault.GetSecretAsync("blobstorageconnectionstring").Result, "powerpagesintegration");
            BlobContainerClient.CreateIfNotExistsAsync();
            KeyVault = keyVault;
            dv = new DataverseService(_configuration, KeyVault);
        }

        // GET: api/<BlobController>
        [HttpGet, Authorize]
        public async ValueTask<IActionResult> Get()
        {
            var blobs = BlobContainerClient.GetBlobsAsync(BlobTraits.All, BlobStates.All);
            var fileDataList = new List<BlobMetadata>();

            await foreach (var blob in blobs)
            {
                fileDataList.Add(new BlobMetadata(blob));
            }

            return fileDataList.Count == 0 ? NotFound() : Ok(fileDataList);
           
        }

        // GET api/<BlobController>/<id>
        [HttpGet("{id}"), Authorize]
        public async ValueTask<ActionResult> Get(string id)
        {
            var submissionData = dv.GetSubmission(new Guid(id));
            if (submissionData.PortalUserContact.EmailAddress != GetCurrentUserEmail())
            {
                return new UnauthorizedResult();
            }

            try
            {
                
                var blobPath = System.Web.HttpUtility.UrlDecode(submissionData.BlobURL.Replace(strBlobContainerPath, ""));
                var filename = Path.GetFileName(blobPath);
              
                var blobClient = BlobContainerClient.GetBlobClient(blobPath);
                var blob = (await blobClient.DownloadContentAsync()).Value.Content;                

                return new OkObjectResult(new { fileName=filename,fileContents = Convert.ToBase64String(blob.ToArray())});
            }
            catch (RequestFailedException e)
            {
                if (e.Status is 0 or 400 or 401) throw;

                return StatusCode(e.Status);
            }
        }

        // POST api/<BlobController>
        [HttpPost("{id}"),Authorize]
        public async ValueTask<IActionResult> Post([FromBody] FileUploadRequest fileData,string id)
        {
            var submissionData = dv.GetSubmission(new Guid(id));
            if (submissionData.PortalUserContact.EmailAddress != GetCurrentUserEmail())
            {
                return new UnauthorizedResult();
            }

            try
            {
                var bytes = Convert.FromBase64String(fileData.fileContents.Split("base64,")[1]);
                
                Stream s = new MemoryStream(bytes);
                var blobClient = BlobContainerClient.GetBlobClient($"{id}/{fileData.fileName}"); 
                await blobClient.UploadAsync(s);

                dv.UpdateFileURL(blobClient.Uri.ToString(), id);

                return new OkObjectResult(new { message = "File Created!"});
            }
            catch (RequestFailedException e)
            {
                if (e.Status is 0 or 400 or 401) throw;

                return StatusCode(e.Status);
            }
        }

             // DELETE api/<BlobController>/5
        [HttpDelete("{id}"), Authorize]
        public async ValueTask<IActionResult> Delete(string id)
        {
            try
            {
                var submissionData = dv.GetSubmission(new Guid(id));
                if (submissionData.PortalUserContact.EmailAddress != GetCurrentUserEmail())
                {
                    return new UnauthorizedResult();
                }
                var blobPath = System.Web.HttpUtility.UrlDecode(submissionData.BlobURL.Replace(strBlobContainerPath, ""));
                var blobClient = BlobContainerClient.GetBlobClient(blobPath);
                var blob = await blobClient.DeleteAsync();
                return NoContent();
            }
            catch (RequestFailedException e)
            {
                if (e.Status is 0 or 400 or 401) throw;

                return StatusCode(e.Status);
            }
        }
    }
}
