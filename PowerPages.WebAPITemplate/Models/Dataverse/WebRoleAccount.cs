using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPages.WebAPITemplate.API.Models.Dataverse
{
    public class WebRoleAccount
    {
        public WebRoleAccount(Entity e)
        {
            WebRoleId = e.GetAttributeValue<Guid>("adx_webroleid");
            AccountId = e.GetAttributeValue<Guid>("accountid");
        }
        public Guid WebRoleId { get; set; }
        public Guid AccountId { get; set; }
    }
}
