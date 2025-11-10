using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class FileViewerPageVM : BaseVM, IQueryAttributable
    {
        private Database db;
        public List<FileClass> Files { get; set; }


        public Command<FileClass> RemoveFileCommand { get; set; }
        public Command<FileClass> RedactFileCommand { get; set; }
        public Command AddFileCommand { get; set; }

        public FileViewerPageVM()
        {
            Initialize();
            
        }


        private void Initialize()
        {
            RemoveFileCommand = new Command<FileClass>(async (file) => 
            {
                await db.RemoveFile(file);
                Files = await db.GetFilesAsync();
            }, (file) => file is not null);

            RedactFileCommand = new Command<FileClass>(async (file) =>
            {
                // Открытие окна редактирования
                await Shell.Current.GoToAsync("Edit", new Dictionary<string, object> { { "db", db }, {"file", file } });
            }, (file) => file is not null);
        }

        public async void LoadFiles()
        {
            Files = await db.GetFilesAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            db = (Database)query["db"];
        }
    }
}
