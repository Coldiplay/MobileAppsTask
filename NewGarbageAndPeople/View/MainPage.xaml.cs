using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            //((MainPageVM)BindingContext).Set(this);
        }

        protected override void OnAppearing()
        {
            ((MainPageVM)BindingContext).LoadLists();
        }
    }
}
