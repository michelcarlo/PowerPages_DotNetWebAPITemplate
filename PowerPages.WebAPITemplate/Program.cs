using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using PowerPages.WebAPITemplate.API.Utils;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using System.Reflection;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);
var apiTitle = "Power Pages WebAPI Template";
var apiVersion = $"v{Assembly.GetEntryAssembly()?.GetName().Version?.ToString()}";
var config = builder.Configuration;
builder.Services.AddAzureClients(clientBuilder =>
{
    // Add a KeyVault client
    clientBuilder.AddSecretClient(new Uri(builder.Configuration["vaultUri"]));
    // Use DefaultAzureCredential by default//
    clientBuilder.UseCredential(new DefaultAzureCredential(new DefaultAzureCredentialOptions
    {
        ExcludeEnvironmentCredential = true,
        ExcludeInteractiveBrowserCredential = true,
        ExcludeAzurePowerShellCredential = true,
        ExcludeSharedTokenCacheCredential = true,
        ExcludeVisualStudioCodeCredential = true,
        ExcludeAzureCliCredential = true,
        ExcludeVisualStudioCredential = false,
        ExcludeManagedIdentityCredential = false,
    }));
});

builder.Services.AddSingleton<IKeyVaultService, KeyVaultService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = apiTitle, Version = $"{apiVersion}" });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins($"https://{builder.Configuration["PowerPages:PortalUrl"]}").AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});

await builder.SetupPortalAuthenticationAsync();

//builder.Services.Configure<KestrelServerOptions>(options =>
//{
//    options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
//});

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", $"{apiTitle} {apiVersion}");
});


app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);
//////
app.UseAuthentication();
//////
app.UseAuthorization();

app.MapControllers();

app.Run();
