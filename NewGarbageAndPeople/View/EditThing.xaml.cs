using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople.View;

public partial class EditThing : ContentPage
{
	public EditThing()
	{
		InitializeComponent();
        //((EditThingVM)BindingContext).Set(thing, db, this);
    }
}