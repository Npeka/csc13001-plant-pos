using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using csc13001_plant_pos.Model;
using Microsoft.Extensions.Configuration;
using Windows.Storage;

namespace csc13001_plant_pos.Service
{
    public interface ICredentialStorageService
    {
        Task<List<RememberedCredential>> GetRememberedCredentialsAsync();
        Task SaveCredentialAsync(string username, string password);
        Task<RememberedCredential> GetCredentialByUsernameAsync(string username);
        Task ClearAllCredentialsAsync();
        Task RemoveCredentialAsync(string username);
    }

    public class CredentialStorageService : ICredentialStorageService
    {
        private const string CREDENTIALS_FILENAME = "remembered_credentials.json";
        private readonly string _encryptionKey;
        private readonly string _filePath;

        public CredentialStorageService(IConfiguration configuration)
        {
            _filePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, CREDENTIALS_FILENAME);
            _encryptionKey = configuration["SecuritySettings:EncryptionKey"]
                ?? throw new InvalidOperationException("Encryption key not found in configuration.");
        }

        public async Task<List<RememberedCredential>> GetRememberedCredentialsAsync()
        {
            if (!File.Exists(_filePath))
            {
                return new List<RememberedCredential>();
            }

            try
            {
                string encryptedJson = await File.ReadAllTextAsync(_filePath);
                string decryptedJson = Decrypt(encryptedJson);
                var credentials = JsonSerializer.Deserialize<List<RememberedCredential>>(decryptedJson);
                return credentials ?? new List<RememberedCredential>();
            }
            catch
            {
                return new List<RememberedCredential>();
            }
        }

        public async Task SaveCredentialAsync(string username, string password)
        {
            var credentials = await GetRememberedCredentialsAsync();

            var existingCredential = credentials.FirstOrDefault(c => c.Username == username);
            if (existingCredential != null)
            {
                credentials.Remove(existingCredential);
            }

            credentials.Add(new RememberedCredential(username, password));

            credentials = credentials.OrderByDescending(c => c.LastUsed).ToList();

            string json = JsonSerializer.Serialize(credentials);
            string encryptedJson = Encrypt(json);
            await File.WriteAllTextAsync(_filePath, encryptedJson);
        }

        public async Task<RememberedCredential> GetCredentialByUsernameAsync(string username)
        {
            var credentials = await GetRememberedCredentialsAsync();
            return credentials.FirstOrDefault(c => c.Username == username);
        }

        public async Task ClearAllCredentialsAsync()
        {
            if (File.Exists(_filePath))
            {
                await Task.Run(() => File.Delete(_filePath));
            }
        }

        public async Task RemoveCredentialAsync(string username)
        {
            var credentials = await GetRememberedCredentialsAsync();
            var credentialToRemove = credentials.FirstOrDefault(c => c.Username == username);

            if (credentialToRemove != null)
            {
                credentials.Remove(credentialToRemove);
                string json = JsonSerializer.Serialize(credentials);
                string encryptedJson = Encrypt(json);
                await File.WriteAllTextAsync(_filePath, encryptedJson);
            }
        }

        private string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32, '0').Substring(0, 32));
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private string Decrypt(string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(_encryptionKey.PadRight(32, '0').Substring(0, 32));
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}