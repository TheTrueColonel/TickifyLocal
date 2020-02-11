using System.Security.Cryptography;
using System.Text;

namespace Tickify.Extensions {
    public static class StringExtensions {
        /// <summary>
        /// Parses the input string to Discord Channel-esque output.
        /// </summary>
        public static string ToDiscordChannel (this string str) =>
            str.ToLower().Replace(" ", "-");

        public static string GetSha256 (this string str) {
            var hash = new SHA256CryptoServiceProvider();

            byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(str));

            var stringBuilder = new StringBuilder();

            foreach (var character in data) {
                stringBuilder.Append(character.ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}