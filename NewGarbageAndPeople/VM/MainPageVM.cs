using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.View;
using NewGarbageAndPeople.VM.VMTools;
using System.Collections.ObjectModel;

namespace NewGarbageAndPeople.VM
{
    public class MainPageVM : BaseVM
    {
        private readonly Database db = new();
        private Owner? currentOwner;
        private ObservableCollection<Owner> owners;
        private List<Thing> things;
        private List<Thing> ownersThings;
        private Thing? selectedThing;

        public ObservableCollection<Owner> Owners
        {
            get => owners;
            set
            {
                owners = value;
                Signal();
            }
        }
        public List<Thing> Things
        {
            get => things;
            set
            {
                things = value;
                Signal();
            }
        }

        public Owner? CurrentOwner
        {
            get => currentOwner;
            set
            {
                currentOwner = value;
                if (value != null)
                    ChangeOwnersThingsList(value.Id);
                Signal();
                RedactOwner.ChangeCanExecute();
                RemoveOwner.ChangeCanExecute();
            }
        }
        public Thing? SelectedThing
        {
            get => selectedThing;
            set
            {
                selectedThing = value;
                Signal();
                //RemoveThing.ChangeCanExecute();
                //RedactThing.ChangeCanExecute();
            }
        }

        public List<Thing> OwnersThings
        {
            get => ownersThings;
            set
            {
                ownersThings = value;
                Signal();
            }
        }


        public Command CreateThing { get; set; }
        public Command CreateOwner { get; set; }
        public Command<Thing> RemoveThing { get; set; }
        public Command RemoveOwner { get; set; }
        public Command<Thing> RedactThing { get; set; }
        public Command RedactOwner { get; set; }

        public MainPageVM() 
        {
            //Initialize();
            CreateThing = new Command(async () =>
            {
                //await page.Navigation.PushAsync(new EditThing(new Thing(), db));
                await Shell.Current.GoToAsync("editThing", new Dictionary<string, object> {{"db", db}});
            }, () => true);
            CreateOwner = new Command(async () =>
            {
                await Shell.Current.GoToAsync("editOwner", new Dictionary<string, object> {{ "db", db } });
                // await page.Navigation.PushAsync(new EditOwner(new Owner(), db));
            }, () => true);

            RemoveThing = new Command<Thing>(async (thing) =>
            {
                await db.RemoveThing(thing);
                Things = await db.GetThingsAsync();
                if (CurrentOwner != null)
                    ChangeOwnersThingsList(CurrentOwner.Id);
            }, (thing) => thing != null);
            RemoveOwner = new Command<Owner>(async (owner) => 
            {
                await db.RemoveOwner(owner, Shell.Current.CurrentPage);
                Owners = [..await db.VerniMneSpisokOwner()];
                CurrentOwner = Owners.FirstOrDefault();
            }, (owner) => owner != null && owner.Id != 0);

            RedactThing = new Command<Thing>(async (thing) =>
            {
                await Shell.Current.GoToAsync("editThing", new Dictionary<string, object> { { "thing", thing }, { "db", db } });
            }, (thing) => thing != null);
            RedactOwner = new Command<Owner>(async (owner) => 
            {
                await Shell.Current.GoToAsync("editOwner", new Dictionary<string, object> {{"owner", owner }, {"db", db}});
                //await page.Navigation.PushAsync(new EditOwner(owner, db));
            }, (owner) => owner != null && owner.Id != 0);
        }

        public async void LoadLists()
        {
            Owners = [.. await db.VerniMneSpisokOwner()];
            Things = await db.GetThingsAsync();
            if (CurrentOwner is not null)
                ChangeOwnersThingsList(CurrentOwner.Id);
        }
        private static void Initialize()
        {
            //Routing.RegisterRoute("mainPage", typeof(MainPage));

        }


        private async void ChangeOwnersThingsList(int ownerId)
        {
            if (ownerId == 0) 
                OwnersThings = (await db.GetThingsAsync()).Where(t => t.OwnerId == null || t.OwnerId == 0).ToList();
            else if (ownerId > 0)
                OwnersThings = await db.GetThingsByOwnerIdAsync(ownerId);
        }
    }
}
