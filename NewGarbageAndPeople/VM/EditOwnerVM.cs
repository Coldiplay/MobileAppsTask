using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class EditOwnerVM : BaseVM, IQueryAttributable
    {
        private Owner? owner;
        private Database db;
        private string fName = string.Empty;
        private string lName = string.Empty;
        private string phone = string.Empty;

        public Command Redacting { get; set; }
        public Owner? Owner
        {
            get => owner;
            set
            {
                owner = value;
                Signal();
                //Redacting.ChangeCanExecute();
            }
        }

        public string FName
        {
            get => fName;
            set
            {
                fName = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public string LName
        {
            get => lName;
            set
            {
                lName = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public string Phone
        {
            get => phone;
            set
            {
                phone = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public EditOwnerVM()
        {
            Redacting = new Command(async () =>
            {
                Owner.FirstName = FName.Trim();
                Owner.LastName = LName.Trim();
                Owner.Email = Owner.Email.Trim();
                Owner.PhoneNumber = Phone.Trim();

                await db.AddOwner(Owner);
                await Shell.Current.GoToAsync("..");
            }, () => !string.IsNullOrEmpty(FName.Trim()) && 
            !string.IsNullOrEmpty(LName.Trim()) &&
            !string.IsNullOrEmpty(Phone.Trim()));

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            query.TryGetValue("owner", out var owner);
            Owner = (Owner?)owner ?? new Owner();
            db = (Database)query["db"];
            FName = Owner.FirstName;
            LName = Owner.LastName;
            Phone = Owner.PhoneNumber;
        }
    }
}
