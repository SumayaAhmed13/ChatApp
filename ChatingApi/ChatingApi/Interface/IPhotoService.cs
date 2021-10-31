using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Interface
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAync(IFormFile file);
        Task<DeletionResult> DeletePhotoAync(string publicId);
    }
}
