using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM.VMTools;

namespace NewGarbageAndPeople.VM
{
    public class EditThingVM : BaseVM, IQueryAttributable
    {
        private Thing? thing;
        public Thing? Thing
        {
            get => thing;
            set
            {
                thing = value;
                Signal();
            }
        }
        private Database db;
        private IReadOnlyList<Owner> owners;
        private string title = string.Empty;
        private byte count;

        public IReadOnlyList<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }

        public Command Redacting { get; set; }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                Signal();
                Redacting.ChangeCanExecute();
            }
        }
        public byte Count
        {
            get => count;
            set
            {
                count = value;
                Signal();
            }
        }
        public EditThingVM() 
        {
            Redacting = new Command(async () =>
            {
                Thing.Title = Title.Trim();
                Thing.Count = Count;
                if (Thing.Owner != null)
                    Thing.OwnerId = Thing.Owner.Id;
                Thing.Description = Thing.Description.Trim();
                await db.AddThing(Thing);
                await Shell.Current.GoToAsync("..");
            }, () => !string.IsNullOrEmpty(Title.Trim()));

        }

        public async void LoadLists()
        {
            Owners = await db.VerniMneSpisokOwner();
            Thing.Owner = Owners.FirstOrDefault(o => o.Id == Thing.OwnerId);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            query.TryGetValue("thing", out var thing);
            Thing = (Thing?)thing ?? new Thing();
            db = (Database)query["db"];
            Title = Thing.Title;
            Count = Thing.Count;
            LoadLists();
        }
    }
}
