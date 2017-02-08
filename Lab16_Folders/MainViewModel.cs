using Mvvm.Core.Abstract;
using Mvvm.Core.ViewModels;
using System.Windows.Forms;
using System.Windows.Input;

namespace Lab16_Folders
{
    public class MainViewModel : ViewModelBase
    {
        public FolderTree Folder
        {
            get { return _folder; }
            set
            {
                UpdateValue(value, ref _folder);
            }
        }

        public string PathFolders
        {
            get { return _pathFolders; }
            set { UpdateValue(value, ref _pathFolders); }
        }

        public string StartNameFolder
        {
            get { return _startNameFolder; }
            set { UpdateValue(value, ref _startNameFolder); }
        }

        public ICommand CreateRealFolderCommand { get; set; }
        public ICommand HomeFolderCommand { get; set; }

        //public ICommand  { get; set; }

        public MainViewModel(ICommandFactory commandFactory)
        {
            _rootFolder = Folder = new FolderTree(null, "/:", this);

            PathFolders = "/:";
            StartNameFolder = "Папка";
            CreateRealFolderCommand = commandFactory.CreateCommand(CreateRealDirectory);
            HomeFolderCommand = commandFactory.CreateCommand(o =>
             {
                 Folder = _rootFolder;
                 PathFolders = _rootFolder.Name;
             });
        }

        private void CreateRealDirectory()
        {
            var folderBrowser = new FolderBrowserDialog();
            var resultDialog = folderBrowser.ShowDialog();
            if (resultDialog != DialogResult.OK || string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                return;
            }

            var qwe = new CreateDirectoryService(folderBrowser.SelectedPath);
            qwe.AddAllFolders(_rootFolder);
        }

        private FolderTree _folder;
        private string _pathFolders;
        private readonly FolderTree _rootFolder;
        private string _startNameFolder;
    }
}