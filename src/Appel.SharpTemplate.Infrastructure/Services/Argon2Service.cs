using System.Text;
using Appel.SharpTemplate.Application.AppSettings;
using Appel.SharpTemplate.Domain.Interfaces.Services;
using Isopoh.Cryptography.Argon2;
using Microsoft.Extensions.Options;

namespace Appel.SharpTemplate.Infrastructure.Services;

public class Argon2Service : IArgon2Service
{
    private readonly Argon2HashSettings _argon2HashSettings;

    public Argon2Service(IOptions<Argon2HashSettings> optionsArgon2HashSettings)
    {
        _argon2HashSettings = optionsArgon2HashSettings.Value;
    }

    public string CreatePasswordHash(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        var config = GenerateArgon2Config(input);

        using var argon2 = new Argon2(config);
        using var hash = argon2.Hash();

        return config.EncodeString(hash.Buffer);
    }

    public bool VerifyPasswordHash(string? hash, string? input)
    {
        if (string.IsNullOrWhiteSpace(hash) || string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var config = GenerateArgon2Config(input);

        using var argon2 = new Argon2(config);
        using var secureArray = argon2.Hash();
        var newHash = config.EncodeString(secureArray.Buffer);

        return hash == newHash;
    }

    private Argon2Config GenerateArgon2Config(string input)
    {
        return new Argon2Config
        {
            Type = Argon2Type.DataIndependentAddressing,
            Version = Argon2Version.Nineteen,
            TimeCost = 10,
            MemoryCost = 128 * 128,
            Lanes = Environment.ProcessorCount,
            Threads = Environment.ProcessorCount, // higher than "Lanes" doesn't help (or hurt)
            Password = Encoding.UTF8.GetBytes(input),
            Salt = Encoding.UTF8.GetBytes(_argon2HashSettings.Salt!),
            Secret = Encoding.UTF8.GetBytes(_argon2HashSettings.SecretKey!)
        };
    }
}
