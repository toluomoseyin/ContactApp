using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Models
{
    public class PhotoToAddDto
    {
        public IFormFile PhotoFile { get; set; }
      
    }
}
