using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using ECommerce.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Services.CloudinaryServices
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;

        //public PhotoService(IOptions<CloudinarySettings> config)
        public PhotoService(IConfiguration configuration)
        {
            //var gmailPassword = Environment.GetEnvironmentVariable("gmailPassword");
            var acc = new Account
                (
                    configuration.GetSection("CloudinarySettings")["Cloudname"],
                     configuration.GetSection("CloudinarySettings")["ApiKey"],
                     configuration.GetSection("CloudinarySettings")["ApiSecret"]
                );
          //  var acc = new Account
          //(
          //    Environment.GetEnvironmentVariable("CloudName"),
          //    Environment.GetEnvironmentVariable("ApiKey"),
          //    Environment.GetEnvironmentVariable("ApiSecret")
          //);
            _cloudinary = new Cloudinary(acc);
        }
        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream)
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);
             
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);
            return result;
        }
    }
}
