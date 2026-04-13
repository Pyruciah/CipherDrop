using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using CipherDrop.Application.Services;

namespace CipherDrop.Infrastructure.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key = System.Text.Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes
    private readonly byte[] _iv = System.Text.Encoding.UTF8.GetBytes("1234567890123456");

    public byte[] Encrypt(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor();
        return encryptor.TransformFinalBlock(data, 0, data.Length);
    }

    public byte[] Decrypt(byte[] data)
    {
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor();
        return decryptor.TransformFinalBlock(data, 0, data.Length);
    }
}
