using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherDrop.Domain.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public int FileId { get; set; }
    public string AccessedBy { get; set; }
    public DateTime Timestamp { get; set; }
    public string IPAddress { get; set; }
}
