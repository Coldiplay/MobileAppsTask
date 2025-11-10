using NewGarbageAndPeople.Models;
using NewGarbageAndPeople.Models.DB;
using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople.View;

public partial class EditOwner : ContentPage
{
	public EditOwner()
	{
        InitializeComponent();
		var vm = new EditOwnerVM();
		//vm.Set(owner, db, this);
		BindingContext = vm;
	}
}