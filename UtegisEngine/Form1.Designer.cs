namespace UtegisEngine
{
    partial class StartWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CreateProjectButton = new Button();
            SuspendLayout();
            // 
            // CreateProjectButton
            // 
            CreateProjectButton.BackColor = Color.FromArgb(67, 92, 116);
            CreateProjectButton.FlatStyle = FlatStyle.Flat;
            CreateProjectButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            CreateProjectButton.ForeColor = Color.White;
            CreateProjectButton.Location = new Point(463, 195);
            CreateProjectButton.Name = "CreateProjectButton";
            CreateProjectButton.Size = new Size(300, 138);
            CreateProjectButton.TabIndex = 0;
            CreateProjectButton.Text = "Создать проект";
            CreateProjectButton.UseVisualStyleBackColor = false;
            CreateProjectButton.Click += CreateProjectButton_Click;
            // 
            // StartWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 44, 64);
            ClientSize = new Size(800, 450);
            Controls.Add(CreateProjectButton);
            Name = "StartWindow";
            Text = "UtegisEngine";
            ResumeLayout(false);
        }

        #endregion

        private Button CreateProjectButton;

        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            var createProjectForm = new CreateProjectForm();
            createProjectForm.ShowDialog();
        }
    }
}
