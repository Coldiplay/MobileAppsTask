using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class AuthorizePageVM : BaseVM
    {
        private readonly Database db = Database.GetDatabase();
        private string login = string.Empty;
        private string password = string.Empty;

        public string Login
        {
            get => login;
            set
            {
                login = value;
                Signal();
                AuthorizeCommand.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                Signal();
                AuthorizeCommand.ChangeCanExecute();
            }
        }

        public Command AuthorizeCommand { get; set; }
        public Command OpenRegistrationPageCommand { get; set; }

        public AuthorizePageVM()
        {
            Initialize();

            AuthorizeCommand = new Command(async () =>
            {
                if (await db.LogIn(Login, Password))
                {
                    await Shell.Current.GoToAsync("//mainPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Неверные данные", "OK");
                }
            }, () => !string.IsNullOrEmpty(Login.Trim()) && !string.IsNullOrEmpty(Password.Trim()));

            OpenRegistrationPageCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//registration");
            });
        }

        private async void Initialize()
        {
            await db.LoadUsers();
        }

    }
}
