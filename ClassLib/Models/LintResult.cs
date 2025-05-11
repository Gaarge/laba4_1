using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLib.Models
{
    public class LintResult
    {
        public bool HasIssues { get; set; }
        public List<LintError> Error { get; } = new();
    }
    public class LintError
    {
        public string RuleId { get; set; }
        public string Message { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public SeverityLevel Severity { get; set; }
    }
    public enum SeverityLevel { Info, Warning, Error }
}

