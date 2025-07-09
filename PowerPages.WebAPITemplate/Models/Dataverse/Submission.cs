using Microsoft.Xrm.Sdk;

namespace PowerPages.WebAPITemplate.API.Models.Dataverse
{
    public class Submission
    {
        public Submission(Entity e)
        {

            Id = e.Id;            
            PortalUser = e.GetAttributeValue<EntityReference>("pnp_portaluser");
            Object o = e.GetAttributeValue<AliasedValue>("contact.emailaddress1");

            PortalUserContact = new Contact() {
                Id = e.GetAttributeValue<EntityReference>("pnp_portaluser").Id,
                EmailAddress = e.GetAttributeValue<AliasedValue>("contact.emailaddress1")?.Value?.ToString(),
                FirstName = e.GetAttributeValue<AliasedValue>("contact.firstname")?.Value?.ToString(),
                LastName = e.GetAttributeValue<AliasedValue>("contact.lastname")?.Value?.ToString()
            };
            BlobURL = e.GetAttributeValue<String>("pnp_bloburl");
        }
        public Guid Id { get; set; }  
        public Contact PortalUserContact { get; set; }
        public EntityReference PortalUser { get; set; }
        public EntityReference Filing { get; set; }
        public String BlobURL { get; set; }

    }
}
