using ClassLib;
using ClassLib.Models;

namespace laba4_1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        private void UpdateStatusLabel()
        {
            bool allWrite =
                !string.IsNullOrWhiteSpace(rtbHtml.Text) &&
                !string.IsNullOrWhiteSpace(rtbCss.Text) &&
                !string.IsNullOrWhiteSpace(rtbJs.Text);
            lblStatus.Text = allWrite ? "Код введён" : "Код не введён";
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;
            var validator = new CodeValidator();
            // разметка колонок
            dgvResults.Columns.Clear();
            dgvResults.Columns.Add("Language", "Язык");
            dgvResults.Columns.Add("Line", "Строка");
            dgvResults.Columns.Add("Column", "Столбец");
            dgvResults.Columns.Add("Message", "Сообщение");

            // валидируем
            var htmlRes = validator.ValidateHtml(rtbHtml.Text);
            var cssRes = validator.ValidateCss(rtbCss.Text);
            var jsRes = validator.ValidateJs(rtbJs.Text);

            // заполняем
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
        }

        private void btnPerf_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;
        }
    }
}
