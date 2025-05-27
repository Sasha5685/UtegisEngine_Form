using System;
using System.IO;
using System.Windows.Forms;

namespace UtegisEngine
{
    public class FileSystemService : IDisposable
    {
        private FolderBrowserDialog? _folderDialog;

        public string? SelectFolder(string description = "Выберите папку")
        {
            _folderDialog = new FolderBrowserDialog
            {
                Description = description,
                ShowNewFolderButton = true
            };

            return _folderDialog.ShowDialog() == DialogResult.OK 
                ? _folderDialog.SelectedPath 
                : null;
        }

        public bool DirectoryExists(string? path)
        {
            return !string.IsNullOrWhiteSpace(path) && Directory.Exists(path);
        }

        public void Dispose()
        {
            _folderDialog?.Dispose();
        }
    }
}