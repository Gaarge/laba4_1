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
            lblStatus.Text = allWrite ? "��� �����" : "��� �� �����";
        }

        private void btnValidate_Click(object sender, EventArgs e)
        {
            UpdateStatusLabel();
            progressBar.Value = 0;
            var validator = new CodeValidator();
            // �������� �������
            dgvResults.Columns.Clear();
            dgvResults.Columns.Add("Language", "����");
            dgvResults.Columns.Add("Line", "������");
            dgvResults.Columns.Add("Column", "�������");
            dgvResults.Columns.Add("Message", "���������");

            // ����������
            var htmlRes = validator.ValidateHtml(rtbHtml.Text);
            var cssRes = validator.ValidateCss(rtbCss.Text);
            var jsRes = validator.ValidateJs(rtbJs.Text);

            // ���������
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
