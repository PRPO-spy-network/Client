using Client.Pages;

namespace Client
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

			Routing.RegisterRoute(nameof(LocationsPage), typeof(LocationsPage));
			Routing.RegisterRoute(nameof(RegistrationsPage), typeof(RegistrationsPage));
		}
	}
}
