using VaultSharp;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.AuthMethods.Token;

namespace VaultHelpers
{
    public class VaultHelper
    {
        private readonly IVaultClient _vaultClient;

        public VaultHelper(string vaultAddress, string vaultToken)
        {
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo(vaultToken);
            VaultClientSettings vaultClientSettings = new VaultClientSettings(vaultAddress, authMethod);
            _vaultClient = new VaultClient(vaultClientSettings);
        }

        public IDictionary<string, object> GetSecrets(string path, string mountPoint = "secret")
        {
            var secret = _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path, mountPoint: mountPoint).Result;
            return secret.Data.Data;
        }
    }
}
