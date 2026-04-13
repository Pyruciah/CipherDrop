using CipherDrop.Application.Services;
using CipherDrop.Domain.Entities;
using CipherDrop.Infrastructure.Services;
using CipherDrop.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CipherDrop.API.Controllers;

[Authorize]
[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly IFileService _fileService;

    public FilesController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(
    IFormFile file,
    [FromServices] IShareService shareService)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var fileRecord = await _fileService.UploadFileAsync(file, userId);

        var link = await shareService.GenerateShareLinkAsync(fileRecord.Id);

        return Ok(new
        {
            message = "File uploaded successfully",
            downloadLink = link
        });
    }


    [HttpPost("generate-link/{fileId}")]
    public async Task<IActionResult> GenerateLink(int fileId, [FromServices] IShareService shareService)
    {
        var link = await shareService.GenerateShareLinkAsync(fileId);
        return Ok(new { link });
    }

   
    [HttpGet("download/{token}")]
    public async Task<IActionResult> Download(
    string token,
    [FromServices] AppDbContext context,
    [FromServices] IEncryptionService encryptionService)
    {
        var share = context.ShareLinks.FirstOrDefault(s => s.Token == token);

        if (share == null)
            return NotFound("Invalid link");

        if (share.ExpiryDate < DateTime.UtcNow)
            return BadRequest("Link expired");

        if (share.CurrentDownloads >= share.MaxDownloads)
            return BadRequest("Download limit reached");

        var file = context.Files.FirstOrDefault(f => f.Id == share.FileId);

        if (file == null)
            return NotFound("File not found");

        var encryptedBytes = await System.IO.File.ReadAllBytesAsync(file.FilePath);
        var decryptedBytes = encryptionService.Decrypt(encryptedBytes);

        share.CurrentDownloads++;

        context.AuditLogs.Add(new AuditLog
        {
            FileId = file.Id,
            AccessedBy = "Anonymous",
            Timestamp = DateTime.UtcNow,
            IPAddress = HttpContext.Connection.RemoteIpAddress?.ToString()
        });

        await context.SaveChangesAsync();

        return File(decryptedBytes, "application/octet-stream", file.FileName);
    }

}