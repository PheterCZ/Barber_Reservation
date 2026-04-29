using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.DTOs
{

    public class AuthResult
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
    }
}