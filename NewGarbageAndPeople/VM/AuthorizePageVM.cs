using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class AuthorizePageVM : BaseVM
    {
        private readonly Database db = Database.GetDatabase();
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;


        public Command AuthorizeCommand { get; set; }
        public Command OpenRegistrationPageCommand { get; set; }

        public AuthorizePageVM()
        {
            
        }

        private void Initialize()
        {
            AuthorizeCommand = new Command(async () =>
            {
                if (await db.LogIn(Login, Password))
                {
                    await Shell.Current.GoToAsync("MainPage");
                }
                else 
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Неверные данные", "");
                }
            });

            OpenRegistrationPageCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("");
            });
        }

    }
}
