using Alpha.Integrations.Encryption.DataTransferObjects;

namespace Alpha.Integrations.Encryption
{
    public interface IEncryptionService
    {
        string Encrypt(string encryptValue);
        string Decrypt(string decryptValue);
        GeneratePasswordResponse GenerateEncryptedPassword(string cipher);
    }
}
