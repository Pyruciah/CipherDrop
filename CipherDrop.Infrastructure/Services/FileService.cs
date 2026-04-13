using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CipherDrop.Application.Services;
using CipherDrop.Persistence;
using CipherDrop.Domain.Entities;
using Microsoft.AspNetCore.Http;



namespace CipherDrop.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly AppDbContext _context;
    private readonly IEncryptionService _encryptionService;
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Storage");

    public FileService(AppDbContext context, IEncryptionService encryptionService)
    {
        _context = context;
        _encryptionService = encryptionService;
    }

    public FileService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FileRecord> UploadFileAsync(IFormFile file, int userId)
    {
        if (!Directory.Exists(_storagePath))
            Directory.CreateDirectory(_storagePath);

        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(_storagePath, uniqueFileName);

        // Save file physically
        
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var encryptedBytes = _encryptionService.Encrypt(memoryStream.ToArray());

            await System.IO.File.WriteAllBytesAsync(filePath, encryptedBytes);
        }

        // Save metadata to DB
        var fileRecord = new FileRecord
        {
            FileName = file.FileName,
            FilePath = filePath,
            OwnerId = userId,
            UploadDate = DateTime.UtcNow
        };

        _context.Files.Add(fileRecord);
        await _context.SaveChangesAsync();

        return fileRecord;
    }
}
