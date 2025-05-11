using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Models
{
    public class LintOptions
    {
        public SeverityLevel MinimumSeverity { get; set; }
        public bool FixAutomatically { get; set; }
    }
}