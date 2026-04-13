using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using CipherDrop.Domain.Entities;


namespace CipherDrop.Application.Services;

public interface IFileService
{
    Task<FileRecord> UploadFileAsync(IFormFile file, int userId);
}
