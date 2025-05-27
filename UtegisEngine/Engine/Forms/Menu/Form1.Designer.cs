using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using UtegisEngine.Engine.Core.Models;

namespace UtegisEngine
{
    partial class StartWindow
    {
        private System.ComponentModel.IContainer components = null;
        private Button CreateProjectButton;
        private Panel projectsPanel;
        private FlowLayoutPanel projectsListPanel;
        private Label recentProjectsLabel;
        private Button openProjectButton;
        private Button showAllProjectsButton;

        private bool showingAllProjects = false;
        private readonly ProjectManager _projectManager = new ProjectManager();
        private readonly FileSystemService _fileSystemService = new FileSystemService();
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _projectManager.Dispose();
                _fileSystemService.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            CreateProjectButton = new Button();
            projectsPanel = new Panel();
            showAllProjectsButton = new Button();
            openProjectButton = new Button();
            projectsListPanel = new FlowLayoutPanel();
            recentProjectsLabel = new Label();
            projectsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // CreateProjectButton
            // 
            CreateProjectButton.BackColor = Color.FromArgb(67, 92, 116);
            CreateProjectButton.FlatStyle = FlatStyle.Flat;
            CreateProjectButton.Font = new Font("Segoe UI", 12F);
            CreateProjectButton.ForeColor = Color.White;
            CreateProjectButton.Location = new Point(394, 417);
            CreateProjectButton.Name = "CreateProjectButton";
            CreateProjectButton.Size = new Size(300, 138);
            CreateProjectButton.TabIndex = 0;
            CreateProjectButton.Text = "Создать проект";
            CreateProjectButton.UseVisualStyleBackColor = false;
            CreateProjectButton.Click += CreateProjectButton_Click;
            // 
            // projectsPanel
            // 
            projectsPanel.BackColor = Color.FromArgb(34, 54, 74);
            projectsPanel.Controls.Add(showAllProjectsButton);
            projectsPanel.Controls.Add(openProjectButton);
            projectsPanel.Controls.Add(projectsListPanel);
            projectsPanel.Controls.Add(recentProjectsLabel);
            projectsPanel.Dock = DockStyle.Left;
            projectsPanel.ForeColor = Color.White;
            projectsPanel.Location = new Point(0, 0);
            projectsPanel.Name = "projectsPanel";
            projectsPanel.Size = new Size(350, 579);
            projectsPanel.TabIndex = 1;
            // 
            // showAllProjectsButton
            // 
            showAllProjectsButton.BackColor = Color.FromArgb(149, 165, 166);
            showAllProjectsButton.Dock = DockStyle.Bottom;
            showAllProjectsButton.FlatStyle = FlatStyle.Flat;
            showAllProjectsButton.Font = new Font("Segoe UI", 8F);
            showAllProjectsButton.ForeColor = Color.White;
            showAllProjectsButton.Location = new Point(0, 499);
            showAllProjectsButton.Name = "showAllProjectsButton";
            showAllProjectsButton.Size = new Size(350, 40);
            showAllProjectsButton.TabIndex = 3;
            showAllProjectsButton.Text = "Показать все проекты";
            showAllProjectsButton.UseVisualStyleBackColor = false;
            showAllProjectsButton.Visible = false;
            showAllProjectsButton.Click += ShowAllProjectsButton_Click;
            // 
            // openProjectButton
            // 
            openProjectButton.BackColor = Color.FromArgb(52, 152, 219);
            openProjectButton.Dock = DockStyle.Bottom;
            openProjectButton.FlatStyle = FlatStyle.Flat;
            openProjectButton.Font = new Font("Segoe UI", 9F);
            openProjectButton.ForeColor = Color.White;
            openProjectButton.Location = new Point(0, 539);
            openProjectButton.Name = "openProjectButton";
            openProjectButton.Size = new Size(350, 40);
            openProjectButton.TabIndex = 2;
            openProjectButton.Text = "Открыть проект";
            openProjectButton.UseVisualStyleBackColor = false;
            openProjectButton.Click += OpenProjectButton_Click;
            // 
            // projectsListPanel
            // 
            projectsListPanel.AutoScroll = true;
            projectsListPanel.Dock = DockStyle.Fill;
            projectsListPanel.FlowDirection = FlowDirection.TopDown;
            projectsListPanel.Location = new Point(0, 40);
            projectsListPanel.Name = "projectsListPanel";
            projectsListPanel.Padding = new Padding(10, 10, 10, 60);
            projectsListPanel.Size = new Size(350, 539);
            projectsListPanel.TabIndex = 1;
            projectsListPanel.WrapContents = false;
            // 
            // recentProjectsLabel
            // 
            recentProjectsLabel.Dock = DockStyle.Top;
            recentProjectsLabel.Font = new Font("Segoe UI", 12F);
            recentProjectsLabel.Location = new Point(0, 0);
            recentProjectsLabel.Name = "recentProjectsLabel";
            recentProjectsLabel.Padding = new Padding(10, 0, 0, 0);
            recentProjectsLabel.Size = new Size(350, 40);
            recentProjectsLabel.TabIndex = 0;
            recentProjectsLabel.Text = "Последние проекты";
            recentProjectsLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StartWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 44, 64);
            ClientSize = new Size(727, 579);
            Controls.Add(projectsPanel);
            Controls.Add(CreateProjectButton);
            Name = "StartWindow";
            Text = "UtegisEngine";
            Load += StartWindow_Load;
            projectsPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        private void CreateProjectButton_Click(object sender, EventArgs e)
        {
            var createProjectForm = new CreateProjectForm();
            if (createProjectForm.ShowDialog() == DialogResult.OK)
            {
                LoadProjectsList();
            }
        }

        private void StartWindow_Load(object sender, EventArgs e)
        {
            LoadProjectsList();
        }

        private void LoadProjectsList()
        {
            projectsListPanel.Controls.Clear();
            var projectsData = _projectManager.LoadProjectsList();

            if (projectsData?.Projects != null)
            {
                var sortedProjects = projectsData.Projects.Values
                    .OrderByDescending(p => p.LastOpened)
                    .ToList();

                showAllProjectsButton.Visible = sortedProjects.Count > 4;
                int projectsToShow = showingAllProjects ? sortedProjects.Count : Math.Min(4, sortedProjects.Count);

                for (int i = 0; i < projectsToShow; i++)
                {
                    AddProjectToPanel(sortedProjects[i]);
                }
            }
        }

        private void AddProjectToPanel(ProjectSettings project)
        {
            var projectPanel = new Panel
            {
                BackColor = Color.FromArgb(44, 64, 84),
                Margin = new Padding(5),
                Size = new Size(320, 80)
            };

            var nameLabel = new Label
            {
                Text = project.NameProject,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 10),
                AutoSize = true
            };

            var pathLabel = new Label
            {
                Text = project.LocateProject,
                Font = new Font("Segoe UI", 8F),
                ForeColor = Color.LightGray,
                Location = new Point(10, 35),
                AutoSize = true,
                MaximumSize = new Size(300, 0)
            };

            var loadButton = new Button
            {
                Text = "Загрузить",
                BackColor = Color.FromArgb(39, 174, 96),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Size = new Size(100, 30),
                Location = new Point(200, 40),
                Tag = project.LocateProject
            };
            loadButton.Click += LoadButton_Click;

            projectPanel.Controls.Add(nameLabel);
            projectPanel.Controls.Add(pathLabel);
            projectPanel.Controls.Add(loadButton);

            projectsListPanel.Controls.Add(projectPanel);
        }

        private void OpenProjectButton_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Выберите папку с проектом UtegisEngine";
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string projectSettingsPath = Path.Combine(folderDialog.SelectedPath, "ProjectSettings.uts");
                    if (File.Exists(projectSettingsPath))
                    {
                        OpenProject(folderDialog.SelectedPath);
                    }
                    else
                    {
                        MessageBox.Show("Выбранная папка не содержит проекта UtegisEngine",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void ShowAllProjectsButton_Click(object sender, EventArgs e)
        {
            showingAllProjects = !showingAllProjects;
            showAllProjectsButton.Text = showingAllProjects ? "Скрыть старые проекты" : "Показать все проекты";
            LoadProjectsList();
        }

        private void UpdateProjectInList(ProjectSettings project)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string projectsFile = Path.Combine(documentsPath, "UtegisEngine", "Projects.utall");

            ProjectsData projectsData = new ProjectsData();

            if (File.Exists(projectsFile))
            {
                string json = File.ReadAllText(projectsFile);
                projectsData = JsonConvert.DeserializeObject<ProjectsData>(json) ?? new ProjectsData();
            }

            projectsData.Projects[project.NameProject] = project;
            string updatedJson = JsonConvert.SerializeObject(projectsData, Formatting.Indented);
            File.WriteAllText(projectsFile, updatedJson);
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            if (sender is Button button && button.Tag is string projectPath)
            {
                OpenProject(projectPath);
            }
        }



        private void OpenProject(string projectPath)
        {
            try
            {
                var engineInterface = new EngineInteface(projectPath);
                engineInterface.Show();
                this.Hide(); // Скрываем стартовое окно
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось открыть проект: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProject(ProjectSettings project)
        {
            MessageBox.Show($"Проект '{project.NameProject}' успешно загружен!\n" +
                $"Расположение: {project.LocateProject}\n" +
                $"Новые функции: {(project.NewFunctions ? "Да" : "Нет")}",
                "Проект загружен", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}