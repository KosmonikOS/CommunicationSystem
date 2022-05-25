using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Services.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace CommunicationSystem.Services.Services
{
    public class PasswordHashService : IPasswordHashService
    {
        public UserSaltPass GenerateSaltPass(string password)
        {
            var salt = GenerateSalt();
            var hash = GenerateHash(password, salt);
            var saltPass = new UserSaltPass()
            {
                PasswordHash = hash,
                Salt = Encoding.ASCII.GetString(salt)
            };
            return saltPass;

        }
        public bool ComparePasswords(string hash, string salt, string password)
        {
            var saltPass = GenerateHash(password, Encoding.ASCII.GetBytes(salt));
            return hash == saltPass;
        }
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }
            return salt;
        }
        public string GenerateHash(string value, byte[] salt)
        {
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: value,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
            return hash;
        }

    }
}
