namespace PowerPages.WebAPITemplate.API
{
    /*
      var firstName = claim.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname").Value;
            var lastName = claim.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname").Value;
     */
    public class LastPaymentDate
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class RegistrationDate
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }

    public class Info
    {
        //            #nullable enable annotations
        public object cstatus { get; set; } = String.Empty;

        public object companySize { get; set; } = String.Empty;
        public object accountName { get; set; } = String.Empty;
        public object role { get; set; } = new object();
        public object balance { get; set; } = new object();
        public object socialId { get; set; } = new object();

        public string ctype { get; set; } = String.Empty;

        //  public object family_name { get; set; }

        public object given_name { get; set; } = String.Empty;

        public string imei { get; set; } = String.Empty;

        public string userName { get; set; } = String.Empty;

        public string customerId { get; set; } = String.Empty;

        public LastPaymentDate lastPaymentDate { get; set; } = new LastPaymentDate();
        public RegistrationDate registrationDate { get; set; } = new RegistrationDate();
        //#nullable disable annotations
    }

    public class PortalClaimInfo
    {
        public string type { get; set; } = string.Empty;
        public Info? info { get; set; }
    }
}
