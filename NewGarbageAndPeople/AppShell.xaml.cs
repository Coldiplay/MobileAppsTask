using NewGarbageAndPeople.View;
using NewGarbageAndPeople.VM;

namespace NewGarbageAndPeople
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("editThing", typeof(EditThing));
            Routing.RegisterRoute("editOwner", typeof(EditOwner));
            Routing.RegisterRoute("fileViewerPage", typeof(FileViewerPage));
            Routing.RegisterRoute("fileViewerPage/Edit", typeof(EditFile));
        }
    }
}
