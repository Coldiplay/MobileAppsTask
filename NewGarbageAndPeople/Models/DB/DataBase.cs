using System.Net.Http.Json;
using System.Text.Json;

namespace NewGarbageAndPeople.Models.DB
{
    public class Database
    {
        private static Database db;
        private HttpClient client;
        private Database()
        {
            client = new HttpClient()
            {
                BaseAddress = new Uri(Config.DB_URI)
            };
        }
        public static Database GetDatabase()
        {
            db ??= new Database();
            return db;
        }
        private User loggedUser;

        public string GetUserName() => loggedUser.Name;



        // Получение списка
        public async Task<List<Owner>> VerniMneSpisokOwner() => await client.GetFromJsonAsync<List<Owner>>("/Owners/List");
        public async Task<List<Thing>> GetThingsAsync() => await client.GetFromJsonAsync<List<Thing>>("/Things/List");
        //public async Task<List<FileClass>> GetFilesAsync()
        //{
        //    await Task.Delay(300);
        //    return files.ToList();
        //}


        //Получение списка вещей по id owner-а
        public async Task<List<Thing>> GetThingsByOwnerIdAsync(int ownerId) => await client.GetFromJsonAsync<List<Thing>>($"/Things/ByOwnerId/{ownerId}");


        // Сохранение
        //public async Task<bool> SaveThingsAsync()
        //{
        //    try
        //    {
        //        using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "things.json"), FileMode.Create))
        //            await JsonSerializer.SerializeAsync<List<Thing>>(fs, things);
        //    }
        //    catch(Exception e)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public async Task<bool> SaveOwnersAsync()
        //{
        //    try
        //    {
        //        using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "owners.json"), FileMode.Create))
        //            await JsonSerializer.SerializeAsync(fs, owners);
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    return true;
        //}
        //public async Task<bool> SaveFilesAsync()
        //{
        //    try
        //    {
        //        using (var fs = new FileStream(Path.Combine(FileSystem.Current.AppDataDirectory, "files.json"), FileMode.Create))
        //        {
        //            await JsonSerializer.SerializeAsync(fs, files);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }

        //    return true;
        //}


        // Получение по id
        public async Task<Owner?> GetOwnerAsync(int id) => await client.GetFromJsonAsync<Owner?>($"/Owners/{id}");
        public async Task<Thing?> GetThingAsync(int id) => await client.GetFromJsonAsync<Thing?>($"/Things/{id}");
        //public async Task<FileClass?> GetFileAsync(int id)
        //{
        //    var file = (await GetFilesAsync()).FirstOrDefault(file => file.Id == id);
        //    return file is null ? null : new FileClass(file.Path){ Id = file.Id, Title = file.Title };
        //}


        // Добавление
        public async Task AddOwner(Owner owner) => await client.PutAsJsonAsync("/Owners/Create", owner);
        public async Task AddThing(Thing thing) => await client.PutAsJsonAsync("/Things/Create", thing);
        //{
        //    if (thing.Id == 0)
        //    {
        //        thing.Id = ++incrementThing;
        //        things.Add(thing);
        //        thing.OwnerId = thing.Owner?.Id;
        //    }
        //    else
        //    {
        //        var thingToChange = things.First(x => x.Id == thing.Id);
        //        thingToChange.Title = thing.Title;
        //        thingToChange.Description = thing.Description;
        //        thingToChange.Carryable = thing.Carryable;
        //        thingToChange.Count = thing.Count;
        //        if (thingToChange.OwnerId != thing.OwnerId)
        //        {
        //            thingToChange.Owner?.Things.Remove(thing);
        //            thingToChange.OwnerId = thing.OwnerId;
        //        }
        //        thing = thingToChange;
        //    }
        //    if (thing.OwnerId != 0)
        //    {
        //        thing.Owner = owners.FirstOrDefault(o => o.Id == thing.OwnerId);
        //        if (thing.Owner is not null && !thing.Owner.Things.Contains(thing))
        //            thing.Owner.Things.Add(thing);
        //    }
        //    else
        //        thing.OwnerId = null;
        //    await SaveThingsAsync();
        //}
        //public async Task AddFile(FileClass file)
        //{
        //    if (!File.Exists(file.Path) && !File.Exists(file.OriginalFilePath))
        //        throw new ArgumentException($"Нет такого файла по пути {file.Path}", nameof(file.Path));

        //    if (!string.IsNullOrEmpty(file.OriginalFilePath))
        //        file.OriginalFilePath = Path.GetFullPath(file.OriginalFilePath);

        //    string newPath = Path.Combine(FileSystem.Current.AppDataDirectory, $"Files/{file.Title}{file.Extension}");

        //    if (file.Id == 0)
        //    {
        //        file.Id = ++incrementFile;
        //        //FileClass.MoveFile(file, newPath);
        //        File.Copy(file.OriginalFilePath ?? file.Path, newPath, true);
        //        file.OriginalFilePath = null;
        //        file.ChangePath(newPath);
        //        files.Add(file);
        //    }
        //    else
        //    {
        //        var fileToChange = files.First(f => f.Id == file.Id);
        //        file.OriginalFilePath = fileToChange.Path;

        //        if (file.Path != file.OriginalFilePath)
        //        { 
        //            File.Copy(file.OriginalFilePath, newPath, true);
        //            if (fileToChange.Path.Contains(Path.Combine(FileSystem.Current.AppDataDirectory, "Files/")))
        //            {
        //                File.Delete(fileToChange.Path);
        //            }
        //        }
        //        fileToChange.Title = file.Title;
        //        fileToChange.Extension = file.Extension;
        //        fileToChange.OriginalFilePath = null;
        //        fileToChange.ChangePath(file.Path);
        //    }

        //    await SaveFilesAsync();
        //}


        // Удаление
        public async Task RemoveThing(Thing thing) => await client.DeleteAsync($"/Things/Delete/{thing.Id}");
        public async Task RemoveOwner(Owner owner, Page page) => page.DisplayAlert("Ответ от сервера", await (await client.DeleteAsync($"/Owners/Delete/{owner.Id}")).Content.ReadAsStringAsync(), "Ок");
        //{
        //    //var own = owners.FirstOrDefault(o => o.Id == owner.Id);
        //    //if (own?.Things.Count > 0)
        //    //{
        //    //    page?.DisplayAlert("Ошибка","Нельзя удалить хозяина, пока у него есть вещи.","ОК");
        //    //    return;
        //    //}
        //    //owners.Remove(own);
        //    //await SaveOwnersAsync();
        //}
        //public async Task RemoveFile(FileClass file)
        //{
        //    if (!File.Exists(file.Path) || !file.Path.Contains(Path.Combine(FileSystem.Current.AppDataDirectory, "Files/")))
        //        throw new Exception();

        //    File.Delete(file.Path);
        //    files.Remove(file);
        //    await SaveFilesAsync();
        //}


        //Авторизация
        public async Task<bool> LogIn(string username, string password)
        {
            var user = new User { Name = username, Password = password };
            var result = await client.PostAsJsonAsync("/Users/LogIn", user);
            int res = await result.Content.ReadFromJsonAsync<int>();
            if (res == -1)
                return false;
            else
            {
                user.Id = res;
                loggedUser = user;
                return true;
            }
        }

        public async Task<bool> CheckDublicate(string username) => await client.GetFromJsonAsync<bool>($"/Users/CheckDublicate/{username}");

        public async Task AddNewLoginPassword(string username, string password) => await client.PostAsJsonAsync("/Users/Registrate", new User { Name = username, Password = password});

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
