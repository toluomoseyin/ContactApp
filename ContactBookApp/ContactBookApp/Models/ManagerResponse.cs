using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookApp.Models
{
    public class ManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime? ExpiryDate  { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
