using System.IO;
using System.Windows;

namespace Lab16_Folders
{
    public class CreateDirectoryService
    {
        public CreateDirectoryService(string rootFolder)
        {
            _directory = new DirectoryInfo(rootFolder);
        }

        public void AddAllFolders(FolderTree folder, string path="")
        {
            foreach (FolderTree item in folder.ChildsCollection)
            {
                if (string.IsNullOrWhiteSpace(item.Name) || item.Name == "")
                {
                    item.Name = item.CreateUniqueName(@"!");
                    MessageBox.Show("Пустое имя было заменено на !");
                }

                AddAllFolders(item, $@"{path}\{item.Name}");
            }

            if (!string.IsNullOrEmpty(path))
            {
                path = path.Remove(0, 1);
                var subFolder = _directory.CreateSubdirectory(path);
            }
        }

        private readonly DirectoryInfo _directory;
    }
}