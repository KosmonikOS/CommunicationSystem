using CommunicationSystem.Domain.Entities;
using CommunicationSystem.Services.Services.Interfaces;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

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
                Salt = Convert.ToBase64String(salt)
            };
            return saltPass;

        }
        public bool ComparePasswords(string hash, string salt, string password)
        {
            var saltPass = GenerateHash(password, Convert.FromBase64String(salt));
            return hash == saltPass;
        }
        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[32];
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
