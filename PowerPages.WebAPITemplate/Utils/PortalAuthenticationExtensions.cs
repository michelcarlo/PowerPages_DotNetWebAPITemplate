using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography;
using Org.BouncyCastle.OpenSsl;


namespace PowerPages.WebAPITemplate.API.Utils
{
    public static class PortalExtensions
    {
        public static async Task SetupPortalAuthenticationAsync(this WebApplicationBuilder builder)
        {
            var jwt_key = "";
            if (string.IsNullOrEmpty(jwt_key))
            {
                var portalUrl = builder.Configuration["PowerPages:PortalUrl"];

                HttpClient client = new HttpClient();
                jwt_key = await client.GetStringAsync("https://" + portalUrl + "/_services/auth/publickey");
            }

            var x = new PemReader(new StringReader(jwt_key));

            var y = (RsaKeyParameters)x.ReadObject();

            var rsaInfo = new RSAParameters
            {
                Modulus = y.Modulus.ToByteArrayUnsigned(),
                Exponent = y.Exponent.ToByteArrayUnsigned()
            };

            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaInfo);

            var _signingKey = new RsaSecurityKey(rsa);

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ClockSkew = TimeSpan.Zero,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidateTokenReplay = true,
                        ValidIssuer = builder.Configuration["PowerPages:PortalUrl"],
                        ValidAudience = builder.Configuration["PowerPages:PortalAppAudience"],
                        IssuerSigningKey = _signingKey
                    };
                });
        }

        public static void SetupAPIMHeaders(this HttpClient client,string APIMKey) {
            client.DefaultRequestHeaders.Add("ContentType", "application/json");
            client.DefaultRequestHeaders.Add("CacheControl", "nocache");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", APIMKey);
        }

        public static async Task<String> GetRawBodyString(this HttpRequest Request)
        {
            Request.EnableBuffering();
            Request.Body.Position = 0;

            return await new StreamReader(Request.Body).ReadToEndAsync();
        }
    }
}
