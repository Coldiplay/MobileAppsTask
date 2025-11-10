using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;
using System.Text.RegularExpressions;

namespace NewGarbageAndPeople.VM
{
    public class EditFileVM : BaseVM, IQueryAttributable
    {
        private Database db;
        public FileClass File { get; set; }
        private static string regex = @"^[\w\-. ]+$";
        private string extension;
        private string title;

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
        private string PathToOrigFile;
        public Command SaveFileCommand { get; set; }
        public EditFileVM()
        {
            Initialize();
        }


        private void Initialize()
        {
            SaveFileCommand = new Command(() => 
            {

            }, () => Regex.IsMatch(Title, regex) && Regex.IsMatch(Extension, regex) && !string.IsNullOrEmpty(PathToOrigFile));
        }


        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            query.TryGetValue("file", out var file);
            File = (FileClass?)file ?? new FileClass("");
            db = (Database)query["db"];
        }
    }
}
