using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherDrop.Domain.Entities;

public class FileRecord
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public int OwnerId { get; set; }
    public DateTime UploadDate { get; set; }

    public User Owner { get; set; }
}
