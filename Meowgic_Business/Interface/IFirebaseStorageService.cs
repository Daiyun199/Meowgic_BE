using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Meowgic.Business.Interface
{
    public interface IFirebaseStorageService
    {
        public Task<string> Push(IFormFile file, FileStream data);
    }
}
