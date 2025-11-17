using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople.View;

public partial class FileViewerPage : ContentPage
{
	public FileViewerPage()
	{
		InitializeComponent();
		//BindingContext = new FileViewerPageVM();
	}


    protected override void OnAppearing()
    {
        ((FileViewerPageVM)BindingContext).LoadFiles();
    }

}