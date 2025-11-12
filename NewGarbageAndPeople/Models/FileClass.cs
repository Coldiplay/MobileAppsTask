using System.Text.Json.Serialization;

namespace NewGarbageAndPeople.Models
{
    public class FileClass
    {
        private string extension;

        public FileClass(string path)
        {
            if (!ChangePath(path))
                throw new ArgumentException($"Файла по пути {path} нет", nameof(path));
        }
        public FileClass() { }

        public int Id { get; set; }
        public string Title { get; set; } = "";

        public string Path { get; set; }
        public string? OriginalFilePath { get; set; }
        public bool ChangePath(string path)
        {
            if (File.Exists(path))
            {
                Path = path;
                return true;
            }
            return false; 
        }


        public string Extension 
        {
            get 
            {
                return extension;
            }
            set 
            {
                extension = value;
            }
        }
        [JsonIgnore]
        public DateTime DateOfCreating => File.GetCreationTime(Path);
        [JsonIgnore]
        public DateTime DateOfLastChange => File.GetLastWriteTime(Path);


        public static void MoveFile(FileClass file, string? pathToWhere = null)
        {
            if (pathToWhere is null)
            {
                pathToWhere = file.Path;
                if (!System.IO.Path.IsPathFullyQualified(pathToWhere))
                    throw new ArgumentException();
            }

            if (pathToWhere != file.OriginalFilePath)
            {
                if (File.Exists(pathToWhere))
                    File.Delete(pathToWhere);
                File.Copy(file.OriginalFilePath ?? file.Path, pathToWhere);
            }
        }
    }
}
