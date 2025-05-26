using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UtegisEngine
{
    public class ProjectSettings
    {
        public string NameProject { get; set; }
        public string LocateProject { get; set; }
        public bool NewFunctions { get; set; }
        public DateTime LastOpened { get; set; } = DateTime.Now;
    }

    public class ProjectsData
    {
        public Dictionary<string, ProjectSettings> Projects { get; set; } = new Dictionary<string, ProjectSettings>();
    }

    partial class CreateProjectForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox projectNameTextBox;
        private TextBox locationTextBox;
        private Button browseButton;
        private CheckBox useLatestFeaturesCheckBox;
        private Button createButton;
        private Button cancelButton;
        private Panel headerPanel;
        private Label titleLabel;
        private Panel contentPanel;
        private Panel footerPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.useLatestFeaturesCheckBox = new System.Windows.Forms.CheckBox();
            this.browseButton = new System.Windows.Forms.Button();
            this.locationTextBox = new System.Windows.Forms.TextBox();
            this.projectNameTextBox = new System.Windows.Forms.TextBox();
            this.footerPanel = new System.Windows.Forms.Panel();
            this.cancelButton = new System.Windows.Forms.Button();
            this.createButton = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            this.contentPanel.SuspendLayout();
            this.footerPanel.SuspendLayout();
            this.SuspendLayout();

            // headerPanel
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.ForeColor = System.Drawing.Color.White;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(600, 60);
            this.headerPanel.TabIndex = 0;

            // titleLabel
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(20, 18);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(181, 25);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Создать новый проект";

            // contentPanel
            this.contentPanel.BackColor = System.Drawing.Color.FromArgb(52, 73, 94);
            this.contentPanel.Controls.Add(this.useLatestFeaturesCheckBox);
            this.contentPanel.Controls.Add(this.browseButton);
            this.contentPanel.Controls.Add(this.locationTextBox);
            this.contentPanel.Controls.Add(this.projectNameTextBox);
            this.contentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contentPanel.ForeColor = System.Drawing.Color.White;
            this.contentPanel.Location = new System.Drawing.Point(0, 60);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Padding = new System.Windows.Forms.Padding(20);
            this.contentPanel.Size = new System.Drawing.Size(600, 240);
            this.contentPanel.TabIndex = 1;

            // useLatestFeaturesCheckBox
            this.useLatestFeaturesCheckBox.AutoSize = true;
            this.useLatestFeaturesCheckBox.Checked = true;
            this.useLatestFeaturesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useLatestFeaturesCheckBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.useLatestFeaturesCheckBox.Location = new System.Drawing.Point(23, 130);
            this.useLatestFeaturesCheckBox.Name = "useLatestFeaturesCheckBox";
            this.useLatestFeaturesCheckBox.Size = new System.Drawing.Size(312, 23);
            this.useLatestFeaturesCheckBox.TabIndex = 3;
            this.useLatestFeaturesCheckBox.Text = "Использовать новейшие функции движка";
            this.useLatestFeaturesCheckBox.UseVisualStyleBackColor = true;

            // browseButton
            this.browseButton.BackColor = System.Drawing.Color.FromArgb(41, 128, 185);
            this.browseButton.FlatAppearance.BorderSize = 0;
            this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.browseButton.ForeColor = System.Drawing.Color.White;
            this.browseButton.Location = new System.Drawing.Point(450, 80);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(100, 25);
            this.browseButton.TabIndex = 2;
            this.browseButton.Text = "Обзор...";
            this.browseButton.UseVisualStyleBackColor = false;
            this.browseButton.Click += new System.EventHandler(this.BrowseButton_Click);

            // locationTextBox
            this.locationTextBox.BackColor = System.Drawing.Color.FromArgb(60, 84, 107);
            this.locationTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.locationTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.locationTextBox.ForeColor = System.Drawing.Color.White;
            this.locationTextBox.Location = new System.Drawing.Point(23, 80);
            this.locationTextBox.Name = "locationTextBox";
            this.locationTextBox.Size = new System.Drawing.Size(420, 25);
            this.locationTextBox.TabIndex = 1;

            // projectNameTextBox
            this.projectNameTextBox.BackColor = System.Drawing.Color.FromArgb(60, 84, 107);
            this.projectNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.projectNameTextBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.projectNameTextBox.ForeColor = System.Drawing.Color.White;
            this.projectNameTextBox.Location = new System.Drawing.Point(23, 30);
            this.projectNameTextBox.Name = "projectNameTextBox";
            this.projectNameTextBox.PlaceholderText = "Название проекта";
            this.projectNameTextBox.Size = new System.Drawing.Size(527, 25);
            this.projectNameTextBox.TabIndex = 0;

            // footerPanel
            this.footerPanel.BackColor = System.Drawing.Color.FromArgb(44, 62, 80);
            this.footerPanel.Controls.Add(this.cancelButton);
            this.footerPanel.Controls.Add(this.createButton);
            this.footerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.footerPanel.Location = new System.Drawing.Point(0, 300);
            this.footerPanel.Name = "footerPanel";
            this.footerPanel.Padding = new System.Windows.Forms.Padding(10);
            this.footerPanel.Size = new System.Drawing.Size(600, 60);
            this.footerPanel.TabIndex = 2;

            // cancelButton
            this.cancelButton.BackColor = System.Drawing.Color.FromArgb(192, 57, 43);
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Location = new System.Drawing.Point(400, 10);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(90, 40);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Отмена";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);

            // createButton
            this.createButton.BackColor = System.Drawing.Color.FromArgb(39, 174, 96);
            this.createButton.FlatAppearance.BorderSize = 0;
            this.createButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createButton.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.createButton.ForeColor = System.Drawing.Color.White;
            this.createButton.Location = new System.Drawing.Point(500, 10);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(90, 40);
            this.createButton.TabIndex = 0;
            this.createButton.Text = "Создать";
            this.createButton.UseVisualStyleBackColor = false;
            this.createButton.Click += new System.EventHandler(this.CreateButton_Click);

            // CreateProjectForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 360);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.footerPanel);
            this.Controls.Add(this.headerPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateProjectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Новый проект - UtegisEngine";
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.contentPanel.ResumeLayout(false);
            this.contentPanel.PerformLayout();
            this.footerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Выберите папку для проекта";
                folderBrowserDialog.ShowNewFolderButton = true;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    locationTextBox.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(projectNameTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите название проекта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(locationTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, укажите расположение проекта", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string fullPath = Path.Combine(locationTextBox.Text, projectNameTextBox.Text);
            if (Directory.Exists(fullPath))
            {
                MessageBox.Show("Папка проекта уже существует. Выберите другое имя или расположение.",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            locationTextBox.Text = fullPath;
            CreateProjectStructure();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CreateProjectStructure()
        {
            try
            {
                Directory.CreateDirectory(locationTextBox.Text);
                Directory.CreateDirectory(Path.Combine(locationTextBox.Text, "Editor"));
                Directory.CreateDirectory(Path.Combine(locationTextBox.Text, "Assets"));
                Directory.CreateDirectory(Path.Combine(locationTextBox.Text, "Export"));

                var settings = new ProjectSettings
                {
                    NameProject = projectNameTextBox.Text,
                    LocateProject = locationTextBox.Text,
                    NewFunctions = useLatestFeaturesCheckBox.Checked,
                    LastOpened = DateTime.Now
                };

                string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(Path.Combine(locationTextBox.Text, "ProjectSettings.uts"), settingsJson);

                UpdateProjectsList(settings);
                MessageBox.Show("Проект успешно создан!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании проекта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateProjectsList(ProjectSettings newProject)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string engineFolder = Path.Combine(documentsPath, "UtegisEngine");
            string projectsFile = Path.Combine(engineFolder, "Projects.utall");

            ProjectsData projectsData = new ProjectsData();

            if (!Directory.Exists(engineFolder))
            {
                Directory.CreateDirectory(engineFolder);
            }

            if (File.Exists(projectsFile))
            {
                string json = File.ReadAllText(projectsFile);
                projectsData = JsonConvert.DeserializeObject<ProjectsData>(json) ?? new ProjectsData();
            }

            projectsData.Projects[newProject.NameProject] = newProject;
            string updatedJson = JsonConvert.SerializeObject(projectsData, Formatting.Indented);
            File.WriteAllText(projectsFile, updatedJson);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}