using Client.Pages;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


		private async void ToLocations(object sender, EventArgs e)
		{
			var page = App.Services.GetRequiredService<LocationsPage>();
			await Shell.Current.Navigation.PushAsync(page);
		}

		private async void ToRegistrations(object sender, EventArgs e)
		{
			var page = App.Services.GetRequiredService<RegistrationsPage>();
			await Shell.Current.Navigation.PushAsync(page);
		}

	}
}
