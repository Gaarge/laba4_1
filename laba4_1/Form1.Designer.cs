namespace laba4_1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            rtbHtml = new RichTextBox();
            rtbCss = new RichTextBox();
            rtbJs = new RichTextBox();
            lhtml = new Label();
            lcss = new Label();
            ljs = new Label();
            lblStatus = new Label();
            btnValidate = new Button();
            btnLint = new Button();
            btnPerf = new Button();
            dgvResults = new DataGridView();
            progressBar = new ProgressBar();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            SuspendLayout();
            // 
            // rtbHtml
            // 
            rtbHtml.Location = new Point(12, 62);
            rtbHtml.Name = "rtbHtml";
            rtbHtml.Size = new Size(264, 249);
            rtbHtml.TabIndex = 0;
            rtbHtml.Text = "";
            // 
            // rtbCss
            // 
            rtbCss.Location = new Point(330, 62);
            rtbCss.Name = "rtbCss";
            rtbCss.Size = new Size(264, 249);
            rtbCss.TabIndex = 1;
            rtbCss.Text = "";
            // 
            // rtbJs
            // 
            rtbJs.Location = new Point(649, 62);
            rtbJs.Name = "rtbJs";
            rtbJs.Size = new Size(264, 249);
            rtbJs.TabIndex = 2;
            rtbJs.Text = "";
            // 
            // lhtml
            // 
            lhtml.AutoSize = true;
            lhtml.Location = new Point(94, 23);
            lhtml.Name = "lhtml";
            lhtml.Size = new Size(85, 15);
            lhtml.TabIndex = 3;
            lhtml.Text = "Введите HTML";
            // 
            // lcss
            // 
            lcss.AutoSize = true;
            lcss.Location = new Point(409, 23);
            lcss.Name = "lcss";
            lcss.Size = new Size(73, 15);
            lcss.TabIndex = 4;
            lcss.Text = "Введите CSS";
            // 
            // ljs
            // 
            ljs.AutoSize = true;
            ljs.Location = new Point(731, 23);
            ljs.Name = "ljs";
            ljs.Size = new Size(63, 15);
            ljs.TabIndex = 5;
            ljs.Text = "Введите JS";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(479, 355);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(72, 15);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Введите код";
            // 
            // btnValidate
            // 
            btnValidate.Location = new Point(438, 399);
            btnValidate.Name = "btnValidate";
            btnValidate.Size = new Size(156, 23);
            btnValidate.TabIndex = 7;
            btnValidate.Text = "Проверить валидатором";
            btnValidate.UseVisualStyleBackColor = true;
            btnValidate.Click += btnValidate_Click;
            // 
            // btnLint
            // 
            btnLint.Location = new Point(438, 460);
            btnLint.Name = "btnLint";
            btnLint.Size = new Size(156, 23);
            btnLint.TabIndex = 8;
            btnLint.Text = " Запустить лентер";
            btnLint.UseVisualStyleBackColor = true;
            btnLint.Click += btnLint_Click;
            // 
            // btnPerf
            // 
            btnPerf.Location = new Point(438, 526);
            btnPerf.Name = "btnPerf";
            btnPerf.Size = new Size(156, 23);
            btnPerf.TabIndex = 9;
            btnPerf.Text = "Тест производительности";
            btnPerf.UseVisualStyleBackColor = true;
            btnPerf.Click += btnPerf_Click;
            // 
            // dgvResults
            // 
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Location = new Point(12, 399);
            dgvResults.Name = "dgvResults";
            dgvResults.Size = new Size(420, 150);
            dgvResults.TabIndex = 10;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(438, 564);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(156, 29);
            progressBar.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(973, 749);
            Controls.Add(progressBar);
            Controls.Add(dgvResults);
            Controls.Add(btnPerf);
            Controls.Add(btnLint);
            Controls.Add(btnValidate);
            Controls.Add(lblStatus);
            Controls.Add(ljs);
            Controls.Add(lcss);
            Controls.Add(lhtml);
            Controls.Add(rtbJs);
            Controls.Add(rtbCss);
            Controls.Add(rtbHtml);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RichTextBox rtbHtml;
        private RichTextBox rtbCss;
        private RichTextBox rtbJs;
        private Label lhtml;
        private Label lcss;
        private Label ljs;
        private Label lblStatus;
        private Button btnValidate;
        private Button btnLint;
        private Button btnPerf;
        private DataGridView dgvResults;
        private ProgressBar progressBar;
    }
}
