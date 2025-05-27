using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace UtegisEngine
{
    public partial class EngineInteface : Form
    {
        private ListView assetsListView;
        private ListView hierarchyListView;
        private string projectPath;
        private string assetsPath;
        private string currentDirectory;
        private ImageList imageList;
        private Stack<string> directoryHistory = new Stack<string>();
        private SceneData sceneData;
        private string sceneFilePath;

        public EngineInteface(string projectPath)
        {
            this.projectPath = projectPath ?? throw new ArgumentNullException(nameof(projectPath));
            this.assetsPath = Path.Combine(projectPath, "Assets");
            this.currentDirectory = assetsPath;
            this.sceneFilePath = Path.Combine(projectPath, "SceneGame.scene");

            InitializeComponent();
            InitializeUI();
            LoadSceneData();
            LoadAssets();
            LoadHierarchy();
        }

        private void InitializeUI()
        {
            // Настройка главного окна
            this.WindowState = FormWindowState.Maximized;
            this.Text = $"UtegisEngine - {Path.GetFileName(projectPath)}";
            this.BackColor = Color.FromArgb(64, 64, 64);
            this.FormBorderStyle = FormBorderStyle.Sizable;

            // Создаем ImageList для иконок
            imageList = new ImageList
            {
                ColorDepth = ColorDepth.Depth32Bit,
                ImageSize = new Size(64, 64)
            };

            string folderPath = Path.Combine(Application.StartupPath, "Images");


            Image folderIcon = Image.FromFile(Path.Combine(folderPath, "folder.png"));
            Image scriptIcon = Image.FromFile(Path.Combine(folderPath, "script.png"));
            Image fileIcon = Image.FromFile(Path.Combine(folderPath, "file.png"));
            Image imageIcon = Image.FromFile(Path.Combine(folderPath, "image.png"));
            Image textIcon = Image.FromFile(Path.Combine(folderPath, "text.png"));

            imageList.Images.Add("folder", folderIcon);
            imageList.Images.Add("script", scriptIcon);
            imageList.Images.Add("file", fileIcon);
            imageList.Images.Add("image", imageIcon);
            imageList.Images.Add("text", textIcon);


            // Главная панель
            var mainPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(56, 56, 56) };
            this.Controls.Add(mainPanel);

            // Панель иерархии (слева)
            var hierarchyPanel = new Panel
            {
                Width = 300,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            mainPanel.Controls.Add(hierarchyPanel);

            // Заголовок панели иерархии
            var hierarchyHeader = new Label
            {
                Text = "Hierarchy",
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            hierarchyPanel.Controls.Add(hierarchyHeader);

            // Кнопки для создания объектов
            var createButtonsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(50, 50, 50)
            };
            hierarchyPanel.Controls.Add(createButtonsPanel);

            var createImageButton = new Button
            {
                Text = "Image",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                Height = 25,
                Width = 70,
                Margin = new Padding(2)
            };
            createImageButton.Click += (s, e) => CreateGameObject("Image", "image");
            createButtonsPanel.Controls.Add(createImageButton);

            var createTextButton = new Button
            {
                Text = "Text",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                Height = 25,
                Width = 70,
                Margin = new Padding(2)
            };
            createTextButton.Click += (s, e) => CreateGameObject("Text", "text");
            createButtonsPanel.Controls.Add(createTextButton);

            // ListView для иерархии
            hierarchyListView = new ListView
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                View = View.Details,
                FullRowSelect = true,
                MultiSelect = false,
                ContextMenuStrip = CreateHierarchyContextMenu()
            };
            hierarchyListView.Columns.Add("Name", 200);
            hierarchyListView.Columns.Add("Type", 100);
            hierarchyListView.SmallImageList = imageList;
            hierarchyListView.DoubleClick += HierarchyListView_DoubleClick;
            hierarchyPanel.Controls.Add(hierarchyListView);

            // Панель активов (внизу) - увеличена высота
            var assetsPanel = new Panel
            {
                Height = 300, // Увеличена высота
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(40, 40, 40)
            };
            mainPanel.Controls.Add(assetsPanel);

            // Панель навигации для Assets
            var assetsNavPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                BackColor = Color.FromArgb(50, 50, 50)
            };
            assetsPanel.Controls.Add(assetsNavPanel);

            var backButton = new Button
            {
                Text = "← Back",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                Width = 70
            };
            backButton.Click += (s, e) => NavigateBack();
            assetsNavPanel.Controls.Add(backButton);

            var currentPathLabel = new Label
            {
                Text = "Assets",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            assetsNavPanel.Controls.Add(currentPathLabel);

            // Заголовок панели активов
            var assetsHeader = new Label
            {
                Text = "Project",
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 30, 30),
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            assetsPanel.Controls.Add(assetsHeader);

            // ListView для отображения папки Assets (в виде иконок)
            assetsListView = new ListView
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(40, 40, 40),
                ForeColor = Color.White,
                View = View.LargeIcon,
                LargeImageList = imageList,
                MultiSelect = false,
                ContextMenuStrip = CreateAssetsContextMenu()
            };
            assetsListView.DoubleClick += AssetsListView_DoubleClick;
            assetsPanel.Controls.Add(assetsListView);

            // Основная рабочая область (сцена)
            var scenePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(56, 56, 56),
                BorderStyle = BorderStyle.FixedSingle
            };
            mainPanel.Controls.Add(scenePanel);
        }

        private ContextMenuStrip CreateAssetsContextMenu()
        {
            var menu = new ContextMenuStrip();

            // Создать папку
            var createFolderItem = new ToolStripMenuItem("Create Folder");
            createFolderItem.Click += (s, e) => CreateNewFolder();
            menu.Items.Add(createFolderItem);

            // Создать скрипт
            var createScriptItem = new ToolStripMenuItem("Create Script.scriptsutg");
            createScriptItem.Click += (s, e) => CreateNewScript();
            menu.Items.Add(createScriptItem);

            menu.Items.Add(new ToolStripSeparator());

            // Переименовать
            var renameItem = new ToolStripMenuItem("Rename");
            renameItem.Click += (s, e) => RenameSelectedAsset();
            menu.Items.Add(renameItem);

            // Удалить
            var deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += (s, e) => DeleteSelectedAsset();
            menu.Items.Add(deleteItem);

            menu.Items.Add(new ToolStripSeparator());

            // Обновить
            var refreshItem = new ToolStripMenuItem("Refresh");
            refreshItem.Click += (s, e) => LoadAssets();
            menu.Items.Add(refreshItem);

            return menu;
        }

        private ContextMenuStrip CreateHierarchyContextMenu()
        {
            var menu = new ContextMenuStrip();

            // Переименовать
            var renameItem = new ToolStripMenuItem("Rename");
            renameItem.Click += (s, e) => RenameSelectedGameObject();
            menu.Items.Add(renameItem);

            // Удалить
            var deleteItem = new ToolStripMenuItem("Delete");
            deleteItem.Click += (s, e) => DeleteSelectedGameObject();
            menu.Items.Add(deleteItem);

            return menu;
        }

        private void CreateNewFolder()
        {
            string newFolderName = "New Folder";
            string newFolderPath = Path.Combine(currentDirectory, newFolderName);
            int counter = 1;

            while (Directory.Exists(newFolderPath))
            {
                newFolderName = $"New Folder {counter++}";
                newFolderPath = Path.Combine(currentDirectory, newFolderName);
            }

            try
            {
                Directory.CreateDirectory(newFolderPath);
                LoadAssets();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating folder: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateNewScript()
        {
            string newScriptName = "NewScript.scriptsutg";
            string newScriptPath = Path.Combine(currentDirectory, newScriptName);
            int counter = 1;

            while (File.Exists(newScriptPath))
            {
                newScriptName = $"NewScript{counter++}.scriptsutg";
                newScriptPath = Path.Combine(currentDirectory, newScriptName);
            }

            try
            {
                File.WriteAllText(newScriptPath, "// New UtegisEngine Script");
                LoadAssets();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating script: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenameSelectedAsset()
        {
            if (assetsListView.SelectedItems.Count == 0) return;

            var selectedItem = assetsListView.SelectedItems[0];
            string oldPath = selectedItem.Tag as string;
            string oldName = Path.GetFileNameWithoutExtension(oldPath);
            string extension = Path.GetExtension(oldPath);

            using (var dialog = new RenameDialog(oldName))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string newName = dialog.NewName;
                    string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName + extension);

                    try
                    {
                        if (Directory.Exists(oldPath))
                        {
                            Directory.Move(oldPath, newPath);
                        }
                        else if (File.Exists(oldPath))
                        {
                            File.Move(oldPath, newPath);
                        }
                        LoadAssets();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error renaming: {ex.Message}", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DeleteSelectedAsset()
        {
            if (assetsListView.SelectedItems.Count == 0) return;

            var selectedItem = assetsListView.SelectedItems[0];
            string path = selectedItem.Tag as string;

            if (MessageBox.Show($"Delete '{Path.GetFileName(path)}'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        Directory.Delete(path, true);
                    }
                    else if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    LoadAssets();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CreateGameObject(string type, string iconKey)
        {
            string name = $"{type}";
            int counter = 1;

            while (sceneData.GameObjects.Exists(g => g.Name == name))
            {
                name = $"{type} {counter++}";
            }

            var gameObject = new GameObject
            {
                Name = name,
                Type = type,
                Position = new Vector3(0, 0, 0),
                Components = new List<Component>()
            };

            sceneData.GameObjects.Add(gameObject);
            SaveSceneData();
            LoadHierarchy();
        }

        private void RenameSelectedGameObject()
        {
            if (hierarchyListView.SelectedItems.Count == 0) return;

            var selectedItem = hierarchyListView.SelectedItems[0];
            var gameObject = sceneData.GameObjects[selectedItem.Index];

            using (var dialog = new RenameDialog(gameObject.Name))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    gameObject.Name = dialog.NewName;
                    SaveSceneData();
                    LoadHierarchy();
                }
            }
        }

        private void DeleteSelectedGameObject()
        {
            if (hierarchyListView.SelectedItems.Count == 0) return;

            var selectedItem = hierarchyListView.SelectedItems[0];
            var gameObject = sceneData.GameObjects[selectedItem.Index];

            if (MessageBox.Show($"Delete '{gameObject.Name}'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                sceneData.GameObjects.RemoveAt(selectedItem.Index);
                SaveSceneData();
                LoadHierarchy();
            }
        }

        private void LoadAssets()
        {
            try
            {
                if (!Directory.Exists(currentDirectory))
                {
                    Directory.CreateDirectory(currentDirectory);
                }

                assetsListView.Items.Clear();

                // Добавляем папки
                foreach (var directory in Directory.GetDirectories(currentDirectory))
                {
                    var item = new ListViewItem
                    {
                        Text = Path.GetFileName(directory),
                        ImageKey = "folder",
                        Tag = directory
                    };
                    assetsListView.Items.Add(item);
                }

                // Добавляем файлы
                foreach (var file in Directory.GetFiles(currentDirectory))
                {
                    var item = new ListViewItem
                    {
                        Text = Path.GetFileName(file),
                        ImageKey = GetFileIcon(file),
                        Tag = file
                    };
                    assetsListView.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading assets: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadHierarchy()
        {
            hierarchyListView.Items.Clear();

            foreach (var gameObject in sceneData.GameObjects)
            {
                var item = new ListViewItem(gameObject.Name);
                item.SubItems.Add(gameObject.Type);
                item.ImageKey = gameObject.Type.ToLower();
                hierarchyListView.Items.Add(item);
            }
        }

        private void LoadSceneData()
        {
            try
            {
                if (File.Exists(sceneFilePath))
                {
                    string json = File.ReadAllText(sceneFilePath);
                    sceneData = JsonConvert.DeserializeObject<SceneData>(json);
                }
                else
                {
                    sceneData = new SceneData { GameObjects = new List<GameObject>() };
                    SaveSceneData();
                }
            }
            catch
            {
                sceneData = new SceneData { GameObjects = new List<GameObject>() };
            }
        }

        private void SaveSceneData()
        {
            try
            {
                string json = JsonConvert.SerializeObject(sceneData, Formatting.Indented);
                File.WriteAllText(sceneFilePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving scene: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetFileIcon(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".scriptsutg" => "script",
                ".png" or ".jpg" or ".jpeg" or ".bmp" => "image",
                _ => "file"
            };
        }

        private void AssetsListView_DoubleClick(object sender, EventArgs e)
        {
            if (assetsListView.SelectedItems.Count == 0) return;

            var selectedItem = assetsListView.SelectedItems[0];
            string path = selectedItem.Tag as string;

            if (Directory.Exists(path))
            {
                // Переходим в папку
                directoryHistory.Push(currentDirectory);
                currentDirectory = path;
                LoadAssets();
            }
            else if (File.Exists(path))
            {
                // Открываем файл
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = path,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void HierarchyListView_DoubleClick(object sender, EventArgs e)
        {
            if (hierarchyListView.SelectedItems.Count == 0) return;

            // Здесь можно добавить логику для выбора объекта в сцене
            // Например, выделить его или открыть свойства
        }

        private void NavigateBack()
        {
            if (directoryHistory.Count > 0)
            {
                currentDirectory = directoryHistory.Pop();
                LoadAssets();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSceneData();
            base.OnFormClosing(e);
        }
    }

    // Классы для хранения данных сцены
    public class SceneData
    {
        public List<GameObject> GameObjects { get; set; }
    }

    public class GameObject
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public Vector3 Position { get; set; }
        public List<Component> Components { get; set; }
    }

    public class Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3() : this(0, 0, 0) { }
    }

    public class Component
    {
        public string Type { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    // Диалог для переименования
    public class RenameDialog : Form
    {
        public string NewName { get; private set; }

        public RenameDialog(string currentName)
        {
            this.Text = "Rename";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(300, 80);

            var textBox = new TextBox
            {
                Text = currentName,
                Dock = DockStyle.Top,
                Margin = new Padding(10)
            };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Right,
                Width = 80
            };

            var cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Right,
                Width = 80
            };

            okButton.Click += (s, e) =>
            {
                NewName = textBox.Text;
                this.Close();
            };

            cancelButton.Click += (s, e) => this.Close();

            this.Controls.Add(textBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }
    }
}