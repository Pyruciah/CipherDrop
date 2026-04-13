using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherDrop.Domain.Entities;

public class ShareLink
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int MaxDownloads { get; set; }
    public int CurrentDownloads { get; set; }

    public FileRecord File { get; set; }
}
