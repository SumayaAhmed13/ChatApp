using ChatingApi.Helper;
using ChatingApi.Interface;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatingApi.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        public PhotoService(IOptions<CloudinarySetting>config)
        {
            var acc = new Account
                (
                 config.Value.CloudName,
                 config.Value.ApiKey,
                 config.Value.ApiSecret
                
                );
            _cloudinary = new Cloudinary(acc);

        }
        public  async Task<ImageUploadResult> AddPhotoAync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParems = new ImageUploadParams
                {
                    File=new FileDescription(file.FileName,stream),
                    Transformation=new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParems);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAync(string publicId)
        {
            var deleteParam = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParam);
            return result;
        }
    }
}
