using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherDrop.Application.Services;

public interface IShareService
{
    Task<string> GenerateShareLinkAsync(int fileId);
}
