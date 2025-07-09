using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPages.WebAPITemplate.API.Models.Dataverse
{
    public class WebRoleContact
    {
        public WebRoleContact(Entity e)
        {
            WebRoleId = e.GetAttributeValue<Guid>("adx_webroleid");
            ContactId = e.GetAttributeValue<Guid>("contactid");          
        }
        public Guid WebRoleId { get; set; }
        public Guid ContactId { get; set; }
    }
}
