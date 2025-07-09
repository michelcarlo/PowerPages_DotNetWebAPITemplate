using PowerPages.WebAPITemplate.API.Models.Dataverse;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.PowerPlatform.Dataverse.Client;
using PowerPages.WebAPITemplate.API.Utils;
using System.Collections;
using System.IO.Compression;
using Newtonsoft.Json;

using PowerPages.WebAPITemplate.API.Models;

namespace PowerPages.WebAPITemplate.API.Utils
{
    public class DataverseService
    {
        private ServiceClient serviceClient;
        private IConfiguration Configuration;
        private IKeyVaultService KeyVault;
      
        public DataverseService(IConfiguration configuration, IKeyVaultService kvw)
        {
            Configuration = configuration;
            KeyVault = kvw;
            string strAdminWebRoleId = Configuration["PowerPages:PortalAdminWebRoleId"] ?? "";
            string connectionString = $"AuthType=ClientSecret; url=https://{Configuration["PowerPages:DataverseEnvironment"]}.crm4.dynamics.com; ClientId={Configuration["PowerPages:DataverseAppId"]}; ClientSecret={kvw.GetSecretAsync("dataverse-appsecret").Result}";
           // adminWebRoleId = new Guid(strAdminWebRoleId);
            serviceClient =
              new(connectionString);
        }

        public DataverseService(String connectionString)
        {
            serviceClient =
              new(connectionString);
        }

        public void UpdateFileURL(String fileURL, String submissionID)
        {
            var supDoc = new Entity("pnp_submission");
            supDoc.Id = new Guid(submissionID);
            supDoc["pnp_bloburl"] = fileURL;
            serviceClient.Update(supDoc);
        }


        public Submission GetSubmissionData(Guid submissionID)
        {
            Entity o = serviceClient.Retrieve("pnp_submission", submissionID, new ColumnSet("pnp_portaluser", "pnp_bloburl"));
            //serviceClient.Retrieve()
            return new Submission(o);
        }


        public Contact GetContactData(String contactEmail)
        {
            ConditionExpression condition = new ConditionExpression() { AttributeName = "emailaddress1", Operator = ConditionOperator.Equal };
            condition.Values.Add(contactEmail);

            FilterExpression filter = new FilterExpression();
            filter.Conditions.Add(condition);

            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet.AddColumns("parentcustomerid", "firstname", "lastname", "emailaddress1");
            query.Criteria.AddFilter(filter);

            var e1 = serviceClient.RetrieveMultiple(query);

            return new Contact(e1.Entities[0]);
        }

        public Submission GetSubmission(Guid submissionID)
        {
            // Set Condition Values
            //var query_pnp_submissionid = "a2272a32-39b2-ee11-a569-000d3aa9a09b";

            // Instantiate QueryExpression query
            var query = new QueryExpression("pnp_submission");
            query.TopCount = 1;
            // Add columns to query.ColumnSet
            query.ColumnSet.AddColumns("pnp_bloburl", "pnp_portaluser");

            // Add conditions to query.Criteria
            query.Criteria.AddCondition("pnp_submissionid", ConditionOperator.Equal, submissionID);

            // Add link-entity contact
            var contact = query.AddLink("contact", "pnp_portaluser", "contactid");
            contact.EntityAlias = "contact";

            // Add columns to contact.Columns
            contact.Columns.AddColumns("contactid", "emailaddress1", "parentcustomerid", "firstname", "lastname");

            Object response = serviceClient.RetrieveMultiple(query)[0];

            return new Submission(serviceClient.RetrieveMultiple(query)[0]);
        }
    }
}
