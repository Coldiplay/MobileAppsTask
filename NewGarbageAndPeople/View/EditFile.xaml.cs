using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople.View;

public partial class EditFile : ContentPage
{
	public EditFile()
	{
		InitializeComponent();
		BindingContext = new EditFileVM();
	}
}