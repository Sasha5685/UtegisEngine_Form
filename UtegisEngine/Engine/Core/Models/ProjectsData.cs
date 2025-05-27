using System.Collections.Generic;

namespace UtegisEngine.Engine.Core.Models
{
    public class ProjectsData
    {
        public Dictionary<string, ProjectSettings> Projects { get; set; } = new Dictionary<string, ProjectSettings>();
    }
}