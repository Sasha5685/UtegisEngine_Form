using System;
using System.IO;
using Newtonsoft.Json;
using UtegisEngine.Engine.Core.Models;

namespace UtegisEngine
{
    public class ProjectManager : IDisposable
    {
        private readonly FileSystemService _fileSystemService = new FileSystemService();

        public void CreateProjectStructure(ProjectSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (string.IsNullOrWhiteSpace(settings.NameProject)) throw new ArgumentException("Project name cannot be empty");
            if (string.IsNullOrWhiteSpace(settings.LocateProject)) throw new ArgumentException("Project location cannot be empty");

            try
            {
                Directory.CreateDirectory(settings.LocateProject);
                Directory.CreateDirectory(Path.Combine(settings.LocateProject, "Editor"));
                Directory.CreateDirectory(Path.Combine(settings.LocateProject, "Assets"));
                Directory.CreateDirectory(Path.Combine(settings.LocateProject, "Export"));

                string settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(Path.Combine(settings.LocateProject, "ProjectSettings.uts"), settingsJson);

                UpdateProjectsList(settings);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to create project structure", ex);
            }
        }

        public void UpdateProjectsList(ProjectSettings newProject)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string engineFolder = Path.Combine(documentsPath, "UtegisEngine");
            string projectsFile = Path.Combine(engineFolder, "Projects.utall");

            var projectsData = new ProjectsData();

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

        public ProjectsData? LoadProjectsList()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string projectsFile = Path.Combine(documentsPath, "UtegisEngine", "Projects.utall");

            if (!File.Exists(projectsFile)) return null;

            string json = File.ReadAllText(projectsFile);
            return JsonConvert.DeserializeObject<ProjectsData>(json);
        }

        public void Dispose()
        {
            _fileSystemService.Dispose();
        }
    }
}