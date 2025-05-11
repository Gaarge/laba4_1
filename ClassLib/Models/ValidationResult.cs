using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Models
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<ValidationError> Errors { get; } = new();
    }
    public class ValidationError
    {
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
    }
}
