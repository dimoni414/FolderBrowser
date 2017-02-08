using Mvvm.Core.Services;
using Mvvm.Core.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Lab16_Folders
{
    public class FolderTree : ViewModelBase, IDataErrorInfo
    {
        #region Commands

        public ICommand AddFolderCommand { get; set; }
        public ICommand ChooseFolderCommand { get; set; }
        public ICommand BackFolderCommand { get; set; }
        public ICommand DeleteFolderCommand { get; set; }

        #endregion Commands

        #region Properties

        public ObservableCollection<FolderTree> ChildsCollection { get; set; }

        public FolderTree Parent
        {
            get { return _parent; }
            set { UpdateValue(value, ref _parent); }
        }

        public string Name
        {
            get { return _name; }
            set { UpdateValue(value, ref _name); }
        }

        public string Error { get; }

        #endregion Properties

        #region Validation

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case "Name":
                        string[] symbolsArr = { "\\", "/", ":", "\"", "*", "?", "<", ">" };
                        if (symbolsArr.Any(Name.Contains))
                        {
                            symbolsArr.All(s =>
                            {
                                Name = Name.Replace(s, "");
                                return true;
                            });
                            MessageBox.Show("Имя файла не должно содержать следующих знаков:  \\ / : \" * ? <>");
                        }
                        break;
                }
                return error;
            }
        }

        #endregion Validation

        #region Functions

        public FolderTree(FolderTree folderObject, string name, MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            var commandFactory = new RelayCommandFactory();
            AddFolderCommand = commandFactory.CreateCommand(AddFolder);
            ChooseFolderCommand = commandFactory.CreateCommand(ChooseFolder);
            BackFolderCommand = commandFactory.CreateCommand(BackFolder);
            DeleteFolderCommand = commandFactory.CreateCommand(DeleteFolder);

            Parent = folderObject;
            Name = name;

            ChildsCollection = new ObservableCollection<FolderTree>();
        }

        private void DeleteFolder()
        => Parent.ChildsCollection.Remove(this);

        public void AddFolder(object name)
        {
            var uniqueName = CreateUniqueName(_mainViewModel.StartNameFolder);
            ChildsCollection?.Add(new FolderTree(this, uniqueName, _mainViewModel));
        }

        public string CreateUniqueName(string nameFolder)
        {
            var i = 2;
            var strName = nameFolder;
            while (ChildsCollection.Any(o => o.Name == strName))
            {
                strName = $"{nameFolder} ({i++})";
            }
            return strName;
        }

        private void ChooseFolder()
        {
            _mainViewModel.Folder = this;
            _mainViewModel.PathFolders += "\\" + this.Name;
        }

        private void BackFolder()
        {
            if (Parent == null)
            {
                return;
            }

            _mainViewModel.Folder = Parent;
            _mainViewModel.PathFolders = _mainViewModel.PathFolders.Remove(_mainViewModel.PathFolders.Length - Name.Length - 1);
        }

        #endregion Functions

        #region Fields

        private string _name;
        private FolderTree _parent;
        private readonly MainViewModel _mainViewModel;

        #endregion Fields
    }
}