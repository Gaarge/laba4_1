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

            var result = new CodeValidator().ValidateHtml(rtbHtml.Text);

            dgvResults.Rows.Clear();
            foreach (var err in result.Errors)
                dgvResults.Rows.Add(err.Line, err.Column, err.Message);
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
