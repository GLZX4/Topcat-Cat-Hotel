using System;
using System.Security.Cryptography;
using System.Text;
using Topcat_Cat_Hotel.Services;

public class KeyGeneratorService : IKeyGeneratorService
{
    private static readonly char[] chars =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
    private const int KeySize = 6;

    public string GenerateKey()
    {
        byte[] data = new byte[4 * KeySize];
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }
        StringBuilder result = new StringBuilder(KeySize);
        for (int i = 0; i < KeySize; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }
}
