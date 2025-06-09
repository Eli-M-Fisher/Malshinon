using System;
using System.Text;

namespace MalshinonApp.Utils
{
    public static class SecretCodeGenerator
    {
        private static readonly string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string Generate(int length = 8)
        {
            var random = new Random();
            var builder = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                char c = chars[random.Next(chars.Length)];
                builder.Append(c);
            }

            return builder.ToString();
        }
    }
}