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
        Task<string> UploadImageAsync(IFormFile imageFile, string? imageName = default);

        string GetImageUrl(string imageName);

        Task<string> UpdateImageAsync(IFormFile imageFile, string imageName);

        Task DeleteImageAsync(string imageName);

        Task<string[]> UploadImagesAsync(IFormFileCollection files);
    }
}
