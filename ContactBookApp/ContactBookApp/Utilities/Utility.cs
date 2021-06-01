using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ContactBookApp.Utilities
{
    public class CloudinaryUpload
    {
        private readonly IConfiguration _config;
        private  readonly Cloudinary _cloudinary;
      

        public  CloudinaryUpload(IConfiguration config)
        {
            _config = config;
            
            Account account = new Account
            {
                Cloud = _config.GetSection("CloudinarySettings:CloudName").Value,
                ApiKey = _config.GetSection("CloudinarySettings:ApiKey").Value,
                ApiSecret = _config.GetSection("CloudinarySettings:ApiSecret").Value

               
            };

            _cloudinary = new Cloudinary(account);
        }

       public List<string> UploadMyPic(IFormFile formFile)
        {
            var list = new List<string>();
            var imageUploadResult = new ImageUploadResult();
            using (var fs = formFile.OpenReadStream())
            {
                var imageUploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(formFile.FileName, fs),
                    Transformation = new Transformation().Width(300).Height(300)
                    .Crop("fill").Gravity("face")
                };
                imageUploadResult = _cloudinary.Upload(imageUploadParams);
            }
            var publicId = imageUploadResult.PublicId;
            var Url = imageUploadResult.Url.ToString();
            list.Add(Url);
            list.Add(publicId);
            return list;
        }


       


    }
}
