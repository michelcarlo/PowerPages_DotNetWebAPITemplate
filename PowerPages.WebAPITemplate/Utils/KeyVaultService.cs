using Azure.Security.KeyVault.Secrets;

namespace PowerPages.WebAPITemplate.API.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public interface IKeyVaultService
    {
        public Task<string> GetSecretAsync(string secretName);
    }


    /// <summary>
    /// 
    /// </summary>
    public sealed class KeyVaultService : IKeyVaultService
    {
        private readonly SecretClient _secretClient;

        public KeyVaultService(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultSecret keyValueSecret = await _secretClient.GetSecretAsync(secretName);

            return keyValueSecret.Value;
        }
    }

}
