using System.Text.Json;

namespace NewGarbageAndPeople.Models.DB
{
    public class Database
    {
        private static Database db;
        private Database()
        {
        }
        public static Database GetDatabase()
        {
            db ??= new Database();
            return db;
        }
        private int incrementOwner;
        private int incrementThing;
        private int incrementFile;
        private int incrementUser;
        //public Database() 
        //{
        //    StartDb();
        //    //owners.Add(new Owner { Id = 0, FirstName = "Бесхозные вещи"});
        //    //SaveOwnersAsync();
        //}
        private List<Owner> owners = new();
        private List<Thing> things = new();
        private List<FileClass> files = new();
        private List<User> users = [];
        private User loggedUser;

        public async Task LoadUsers()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "users.json"), FileMode.Open))
                    users = await JsonSerializer.DeserializeAsync<List<User>>(fs);
                incrementUser = users.Max(u => u.Id);
            }
            catch (Exception ex)
            {
                users = [new User { Id = 0, Name = "admin", Password = "root"}];
            }
        }

        public async void StartDb()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Open))
                {
                    //throw new Exception();
                    owners = [.. (await JsonSerializer.DeserializeAsync<List<Owner>>(fs)).Where(o => o.UserId == loggedUser.Id)];
                    incrementOwner = owners.Max(x => x.Id);
                }
            }
            catch (Exception e)
            {
                owners = new();
            }

            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.Open))
                {
                    
                    things = [..(await JsonSerializer.DeserializeAsync<List<Thing>>(fs)).Where(t => t.UserId == loggedUser.Id)];
                    incrementThing = things.Max(x => x.Id);
                }

            }
            catch(Exception e)
            {
                things = new List<Thing>();
            }

            try 
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "files.json"), FileMode.Open))
                {
                    //var test = File.ReadAllText(Path.Combine(FileSystem.Current.AppDataDirectory, "files.json"));
                    files = [..(await JsonSerializer.DeserializeAsync<List<FileClass>>(fs)).Where(f => f.UserId == loggedUser.Id)];
                    incrementFile = files.Max(x => x.Id);
                }
            }
            catch (Exception e)
            {
                files = new();
            }

            for (int i = 0; i < things.Count; i++)
            {
                var owner = owners.FirstOrDefault(o => o.Id == things[i].OwnerId);
                things[i].Owner = owner;
                owner?.Things.Add(things[i]);
            }

            //AddOwner(new Owner() { FirstName = "asdasd", LastName = "sdfsd", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });
            //AddOwner(new Owner() { FirstName = "vbnmvbn", LastName = "xcbv", Email = "asdasd@gmail.com", PhoneNumber = "+12345678901" });
        }

        public string GetUserName() => loggedUser.Name;



        // Получение списка
        public async Task<List<Owner>> VerniMneSpisokOwner()
        {
            await Task.Delay(300);
            return owners.ToList();
        }
        public async Task<List<Thing>> GetThingsAsync()
        {
            await Task.Delay(300);
            return things.ToList();
        }
        public async Task<List<FileClass>> GetFilesAsync()
        {
            await Task.Delay(300);
            return files.ToList();
        }


        //Получение списка вещей по id owner-а
        public async Task<List<Thing>> GetThingsByOwnerIdAsync(int ownerId) 
            => (await GetThingsAsync()).Where(t => t.OwnerId == ownerId).ToList();


        // Сохранение
        public async Task<bool> SaveThingsAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.Create))
                    await JsonSerializer.SerializeAsync<List<Thing>>(fs, things);
            }
            catch(Exception e)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> SaveOwnersAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Create))
                    await JsonSerializer.SerializeAsync(fs, owners);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<bool> SaveFilesAsync()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "files.json"), FileMode.Create))
                {
                    await JsonSerializer.SerializeAsync(fs, files);
                }

            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }


        // Получение по id
        public async Task<Owner?> GetOwnerAsync(int id)
        {
            var owner = (await VerniMneSpisokOwner()).FirstOrDefault(owner => owner.Id == id);
            return owner == null ? null : new Owner
                {
                    Id = owner.Id,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Email = owner.Email,
                    PhoneNumber = owner.PhoneNumber,
                    Things = owner.Things
                };
        }
        public async Task<Thing?> GetThingAsync(int id)
        {
            var thing = (await GetThingsAsync()).FirstOrDefault(thing => thing.Id == id);
            return thing == null ? null : new Thing 
            {
                Id = thing.Id,
                Description = thing.Description,
                Owner = thing.Owner,
                OwnerId = thing.OwnerId,
                Title = thing.Title,
            };
        }
        public async Task<FileClass?> GetFileAsync(int id)
        {
            var file = (await GetFilesAsync()).FirstOrDefault(file => file.Id == id);
            return file is null ? null : new FileClass(file.Path){ Id = file.Id, Title = file.Title };
        }


        // Добавление
        public async Task AddOwner(Owner owner)
        {
            if (owner.Id == 0)
            {
                owner.Id = ++incrementOwner;
                owners.Add(owner);
            }
            else
            {
                var ownerToChange = owners.First(x => x.Id == owner.Id);
                ownerToChange.FirstName = owner.FirstName;
                ownerToChange.LastName = owner.LastName;
                ownerToChange.PhoneNumber = owner.PhoneNumber;
                ownerToChange.Email = owner.Email;
            }
            await SaveOwnersAsync();
        }
        public async Task AddThing(Thing thing)
        {
            if (thing.Id == 0)
            {
                thing.Id = ++incrementThing;
                things.Add(thing);
                thing.OwnerId = thing.Owner?.Id;
            }
            else
            {
                var thingToChange = things.First(x => x.Id == thing.Id);
                thingToChange.Title = thing.Title;
                thingToChange.Description = thing.Description;
                thingToChange.Carryable = thing.Carryable;
                thingToChange.Count = thing.Count;
                if (thingToChange.OwnerId != thing.OwnerId)
                {
                    thingToChange.Owner?.Things.Remove(thing);
                    thingToChange.OwnerId = thing.OwnerId;
                }
                thing = thingToChange;
            }
            if (thing.OwnerId != 0)
            {
                thing.Owner = owners.FirstOrDefault(o => o.Id == thing.OwnerId);
                if (thing.Owner is not null && !thing.Owner.Things.Contains(thing))
                    thing.Owner.Things.Add(thing);
            }
            else
                thing.OwnerId = null;
            await SaveThingsAsync();
        }
        public async Task AddFile(FileClass file)
        {
            if (!File.Exists(file.Path) && !File.Exists(file.OriginalFilePath))
                throw new ArgumentException($"Нет такого файла по пути {file.Path}", nameof(file.Path));

            if (!string.IsNullOrEmpty(file.OriginalFilePath))
                file.OriginalFilePath = Path.GetFullPath(file.OriginalFilePath);

            string newPath = Path.Combine(FileSystem.Current.AppDataDirectory, $"Files/{file.Title}{file.Extension}");

            if (file.Id == 0)
            {
                file.Id = ++incrementFile;
                //FileClass.MoveFile(file, newPath);
                File.Copy(file.OriginalFilePath ?? file.Path, newPath, true);
                file.OriginalFilePath = null;
                file.ChangePath(newPath);
                files.Add(file);
            }
            else
            {
                var fileToChange = files.First(f => f.Id == file.Id);
                file.OriginalFilePath = fileToChange.Path;

                if (file.Path != file.OriginalFilePath)
                { 
                    File.Copy(file.OriginalFilePath, newPath, true);
                    if (fileToChange.Path.Contains(Path.Combine(FileSystem.Current.AppDataDirectory, "Files/")))
                    {
                        File.Delete(fileToChange.Path);
                    }
                }
                fileToChange.Title = file.Title;
                fileToChange.Extension = file.Extension;
                fileToChange.OriginalFilePath = null;
                fileToChange.ChangePath(file.Path);
            }

            await SaveFilesAsync();
        }


        // Удаление
        public async Task RemoveThing(Thing thing)
        {
            thing.Owner?.Things.Remove(thing);
            things.Remove(things.FirstOrDefault(t => t.Id == thing.Id));
            await SaveThingsAsync();
        }
        public async Task RemoveOwner(Owner owner, Page page)
        {
            var own = owners.FirstOrDefault(o => o.Id == owner.Id);
            if (own?.Things.Count > 0)
            {
                page?.DisplayAlert("Ошибка","Нельзя удалить хозяина, пока у него есть вещи.","ОК");
                return;
            }
            owners.Remove(own);
            await SaveOwnersAsync();
        }
        public async Task RemoveFile(FileClass file)
        {
            if (!File.Exists(file.Path) || !file.Path.Contains(Path.Combine(FileSystem.Current.AppDataDirectory, "Files/")))
                throw new Exception();

            File.Delete(file.Path);
            files.Remove(file);
            await SaveFilesAsync();
        }


        //Авторизация
        public async Task<bool> LogIn(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.CurrentCultureIgnoreCase) && u.Password.Equals(password, StringComparison.CurrentCultureIgnoreCase));
            if (user is not null)
            {
                loggedUser = user;
                StartDb();
                return true;
            }
            return false;
        }

        public async Task<bool> CheckDublicate(string username) => users.Any(u => username == u.Name);

        public async Task AddNewLoginPassword(string username, string password)
        {
            User newuser = new User();
            newuser.Name = username;
            newuser.Password = password;
            users.Add(newuser);
            await LoadfileLofinPassword();
        }

        public async Task<bool> LoadfileLofinPassword()
        {
            try
            {
                using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "users.json"), FileMode.Create))
                    await JsonSerializer.SerializeAsync<List<User>>(fs, users);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
