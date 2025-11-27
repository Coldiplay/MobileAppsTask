using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.View.ContentViews;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class RegistrationPageVM : BaseVM
    {
        private readonly Database db = Database.GetDatabase();
        private string login = string.Empty;
        private string password = string.Empty;
        public ContentView AD {  get; set; }



        public string Login
        {
            get => login;
            set
            {
                login = value;
                Signal();
                RegistrationCommand.ChangeCanExecute();
            }
        }
        public string Password
        {
            get => password;
            set
            {
                password = value;
                Signal();
                RegistrationCommand.ChangeCanExecute();
            }
        }

        public Command RegistrationCommand { get; set; }

        public RegistrationPageVM()
        {
            AD = new AD();

            Initialize();

            RegistrationCommand = new Command(async () =>
            {
                if (await db.CheckDublicate(Login))
                {
                    await Shell.Current.DisplayAlert("Ошибка", "Такой логин уже существует.", "OK");
                }

                else
                {

                    await Shell.Current.DisplayAlert("Регистрация", "Вы успешно зарегистрировались", "Ok");
                    await db.AddNewLoginPassword(Login, Password);
                    await Shell.Current.GoToAsync("//authorize");
                }
            }, () => !string.IsNullOrEmpty(Login.Trim()) && !string.IsNullOrEmpty(Password.Trim()));
        }

        private async void Initialize()
        {
            await db.LoadUsers();

            
        }
    }
}
