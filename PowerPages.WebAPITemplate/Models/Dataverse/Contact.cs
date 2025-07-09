using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerPages.WebAPITemplate.API.Models.Dataverse
{
    public class Contact
    {
        public Contact(Entity e) { 
        
        
            Id = e.Id;
            FirstName = e.GetAttributeValue<String>("firstname");
            LastName = e.GetAttributeValue<String>("lastname");
            EmailAddress = e.GetAttributeValue<String>("emailaddress1");
           // Name = e.GetAttributeValue<String>("arch_name");
            Account = e.GetAttributeValue<EntityReference>("parentcustomerid");//new account?         
            //"parentcustomerid", "firstname", "lastname", "emailaddress1")
           
        }

        public Contact()
        {

        }
        public Guid Id { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EmailAddress { get; set; }
        // String Name { get; set; }



        // OptionSetValue civl_therelevantperson {  get; set; }

        public EntityReference Account { get; set; }

    }
}
