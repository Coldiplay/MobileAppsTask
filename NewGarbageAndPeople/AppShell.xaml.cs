using NewGarbageAndPeople.View;

namespace NewGarbageAndPeople
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute("authorize", typeof(AuthorizePage));
            Routing.RegisterRoute("editThing", typeof(EditThing));
            Routing.RegisterRoute("editOwner", typeof(EditOwner));
            Routing.RegisterRoute("fileViewerPage", typeof(FileViewerPage));
            Routing.RegisterRoute("fileViewerPage/Edit", typeof(EditFile));
        }
    }
}
