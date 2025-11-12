using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;
using System.Text.RegularExpressions;

namespace NewGarbageAndPeople.VM
{
    public class EditFileVM : BaseVM, IQueryAttributable
    {
        private readonly Database db = Database.GetDatabase();
        public FileClass File { get; set; }

        private static string regex = @"^[\w\-. ]+$";
        private string extension = "";
        private string title = "";
        private string? pathToOrigFile;

        public string Title
        {
            get => title;
            set
            {
                title = value;
                Signal();
                SaveFileCommand.ChangeCanExecute();
            }
        }
        public string Extension
        {
            get => extension;
            set
            {
                extension = value;
                Signal();
                SaveFileCommand.ChangeCanExecute();
            }
        }
        private string? PathToOrigFile
        {
            get => pathToOrigFile;
            set
            {
                pathToOrigFile = value;
                Signal();
                SaveFileCommand.ChangeCanExecute();
            }
        }
        public Command SaveFileCommand { get; set; }
        public Command GetPathToFileCommand { get; set; }
        public EditFileVM()
        {
            Initialize();
        }


        private void Initialize()
        {
            SaveFileCommand = new Command(async () =>
            {
                //int index = Title.IndexOf('.');
                File.Title = Title;
                File.Extension = Extension;
                File.OriginalFilePath = PathToOrigFile;
                await db.AddFile(File);
                await Shell.Current.GoToAsync("..");
            },
            () => !string.IsNullOrEmpty(PathToOrigFile) && !string.IsNullOrEmpty(Extension) && !string.IsNullOrEmpty(Title));
            //Regex.IsMatch(Title, regex) && Regex.IsMatch(Extension, regex) && !string.IsNullOrEmpty(PathToOrigFile));

            GetPathToFileCommand = new Command(async () =>
            {
                var fileGet = await FilePicker.PickAsync();
                Title = Path.GetFileNameWithoutExtension(fileGet?.FileName) ?? "";
                Extension = Path.GetExtension(fileGet?.FileName) ?? "";
                PathToOrigFile = fileGet?.FullPath;
            });
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            query.TryGetValue("file", out var file);
            File = (FileClass?)file ?? new FileClass();
        }
    }
}
