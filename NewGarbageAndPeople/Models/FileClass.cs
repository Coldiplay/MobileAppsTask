namespace NewGarbageAndPeople.Models
{
    public class FileClass
    {
        private string path;

        public FileClass(string path)
        {
            if (!ChangePath(path))
                throw new ArgumentException($"Файла по пути {path} нет", nameof(path));
        }

        public int Id { get; set; }
        public string Title { get; set; } = "";

        public string Path => path;
        public bool ChangePath(string path)
        {
            if (File.Exists(path))
            {
                this.path = path;
                return true;
            }
            return false; 
        }


        public string Extension => System.IO.Path.GetExtension(this.Path);
        public DateTime DateOfCreating => File.GetCreationTime(Path);
        public DateTime DateOfLastChange => File.GetLastWriteTime(Path);
    }
}
