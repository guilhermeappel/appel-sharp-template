using System.Security.Cryptography;
using System.Text;

namespace Appel.SharpTemplate.Common.ExtensionMethods;

public static class CryptographyExtensions
{
    #region Public Methods

    /// <summary>
    /// Encrypts the specified value using AES encryption with a provided key.
    /// </summary>
    /// <param name="key">The encryption key. Must not be null, empty, or whitespace.</param>
    /// <param name="value">The string to encrypt. Must not be null, empty, or whitespace.</param>
    /// <returns>Encrypted string in hexadecimal format.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or value is null, empty, or whitespace.</exception>
    public static string Encrypt(string? key, string? value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null, empty, or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null, empty, or whitespace.");
        }

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(value);
            }

            return memoryStream.ToArray().ByteArrayToString();
        }
    }

    /// <summary>
    /// Decrypts the specified hexadecimal value using AES decryption with a provided key.
    /// </summary>
    /// <param name="key">The decryption key. Must not be null, empty, or whitespace.</param>
    /// <param name="value">The hexadecimal string to decrypt. Must not be null, empty, or whitespace.</param>
    /// <returns>Decrypted string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when key or value is null, empty, or whitespace.</exception>
    public static string Decrypt(string? key, string? value)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key), "Key cannot be null, empty, or whitespace.");
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value), "Value cannot be null, empty, or whitespace.");
        }

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(StringToByteArray(value));
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using (var streamReader = new StreamReader(cryptoStream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }

    #endregion Public Methods

    #region Private Metthods

    private static string ByteArrayToString(this byte[] bytes)
    {
        return BitConverter.ToString(bytes).ToLower().Replace("-", "");
    }

    private static byte[] StringToByteArray(string hex)
    {
        try
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        catch
        {
            return Array.Empty<byte>();
        }
    }

    #endregion Private Methods
}
