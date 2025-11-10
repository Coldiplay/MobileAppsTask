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
            Routing.RegisterRoute("FileViewerPage/Edit", typeof(EditFile));
        }
    }
}
