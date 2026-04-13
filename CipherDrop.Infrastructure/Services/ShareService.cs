using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CipherDrop.Application.Services;
using CipherDrop.Persistence;
using CipherDrop.Domain.Entities;

namespace CipherDrop.Infrastructure.Services;

public class ShareService : IShareService
{
    private readonly AppDbContext _context;

    public ShareService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateShareLinkAsync(int fileId)
    {
        var file = _context.Files.FirstOrDefault(f => f.Id == fileId);

        if (file == null)
            throw new Exception("File not found");

        var token = Guid.NewGuid().ToString();

        var shareLink = new ShareLink
        {
            FileId = fileId,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddHours(24),
            MaxDownloads = 5,
            CurrentDownloads = 0
        };

        _context.ShareLinks.Add(shareLink);
        await _context.SaveChangesAsync();

        return $"https://localhost:7200/api/files/download/{token}";
    }
}
