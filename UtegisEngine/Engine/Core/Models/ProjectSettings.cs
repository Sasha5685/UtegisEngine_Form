using System;

namespace UtegisEngine
{
    public class ProjectSettings
    {
        public string NameProject { get; set; } = string.Empty;
        public string LocateProject { get; set; } = string.Empty;
        public bool NewFunctions { get; set; }
        public DateTime LastOpened { get; set; } = DateTime.Now;
    }
}