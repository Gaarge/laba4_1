using ClassLib.Models;

namespace ClassLib
{
    public class CodeValidator
    {
        public ValidationResult ValidateHtml(string html) => throw new NotImplementedException();
        public ValidationResult ValidateCss(string css) => throw new NotImplementedException();
        public ValidationResult ValidateJs(string js) => throw new NotImplementedException();
    }
    public class CodeLinter
    {
        public LintResult LintHtml(string html, LintOptions opts) => throw new NotImplementedException();
        public LintResult LintCss(string css, LintOptions opts) => throw new NotImplementedException();
        public LintResult LintJs(string js, LintOptions opts) => throw new NotImplementedException();
    }
    public class PerformanceAnalyzer
    {
        public PerformanceMetrics Measure(string fullHtml) => throw new NotImplementedException();
    }
}