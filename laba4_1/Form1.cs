using ClassLib;
using ClassLib.Models;
using System.Diagnostics;
using Microsoft.Web.WebView2.Core;

namespace laba4_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _ = webView.EnsureCoreWebView2Async();

        }
        private void UpdateStatusLabel()
        {
            bool allWrite =
                !string.IsNullOrWhiteSpace(rtbHtml.Text) &&
                !string.IsNullOrWhiteSpace(rtbCss.Text) &&
                !string.IsNullOrWhiteSpace(rtbJs.Text);
            lblStatus.Text = allWrite ? "Код введён" : "Код не введён";
        }
        private Stopwatch swFcp;
        private Stopwatch swTti;

        private void btnValidate_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;
            var validator = new CodeValidator();
            dgvResults.Columns.Clear();
            dgvResults.Columns.Add("Language", "Язык");
            dgvResults.Columns.Add("Line", "Строка");
            dgvResults.Columns.Add("Column", "Столбец");
            dgvResults.Columns.Add("Message", "Сообщение");

            var htmlRes = validator.ValidateHtml(rtbHtml.Text);
            var cssRes = validator.ValidateCss(rtbCss.Text);
            var jsRes = validator.ValidateJs(rtbJs.Text);

            void AddErrors(string lang, ValidationResult res)
            {
                foreach (var err in res.Errors)
                    dgvResults.Rows.Add(lang, err.Line, err.Column, err.Message);
            }
            AddErrors("HTML", htmlRes);
            AddErrors("CSS", cssRes);
            AddErrors("JS", jsRes);

            progressBar.Value = 100;
        }

        private void btnLint_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;

            var linter = new CodeLinter();

            dgvResults.Columns.Clear();
            dgvResults.Columns.Add("Language", "Язык");
            dgvResults.Columns.Add("Line", "Строка");
            dgvResults.Columns.Add("Column", "Столбец");
            dgvResults.Columns.Add("Rule", "Правило");
            dgvResults.Columns.Add("Message", "Сообщение");

            var htmlLint = linter.LintHtml(rtbHtml.Text, new LintOptions());
            var cssLint = linter.LintCss(rtbCss.Text, new LintOptions());
            var jsLint = linter.LintJs(rtbJs.Text, new LintOptions());

            void AddLint(string lang, LintResult res)
            {
                foreach (var err in res.Error)
                    dgvResults.Rows.Add(lang, err.Line, err.Column, err.RuleId, err.Message);
            }
            AddLint("HTML", htmlLint);
            AddLint("CSS", cssLint);
            AddLint("JS", jsLint);

            progressBar.Value = 100;
        }

        private async void btnPerf_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;

            string html = rtbHtml.Text;
            string css = rtbCss.Text;
            string js = rtbJs.Text;
            string fullHtml = "<!DOCTYPE html><html><head>" +
                              "<meta charset=\"utf-8\"/>" +
                              "<style>" + css + "</style></head><body>" +
                              html + "<script>" + js + "</script></body></html>";

            await webView.EnsureCoreWebView2Async();

            swFcp = Stopwatch.StartNew();
            webView.CoreWebView2.DOMContentLoaded += OnDomContentLoaded;

            swTti = new Stopwatch();
            webView.CoreWebView2.NavigationStarting += OnNavigationStarting;
            webView.CoreWebView2.NavigationCompleted += OnNavigationCompleted;

            webView.NavigateToString(fullHtml);
        }
        private void OnDomContentLoaded(object sender, CoreWebView2DOMContentLoadedEventArgs e)
        {
            swFcp.Stop();
            lblFCP.Text = $"FCP: {swFcp.ElapsedMilliseconds} ms";
            webView.CoreWebView2.DOMContentLoaded -= OnDomContentLoaded;
        }

        private void OnNavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            swTti.Restart();
        }

        private void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            swTti.Stop();
            lblTTI.Text = $"TTI: {swTti.ElapsedMilliseconds} ms";
            progressBar.Value = 100;
            webView.CoreWebView2.NavigationCompleted -= OnNavigationCompleted;
        }
    }
}
