using Client.ViewModels;
using Microsoft.Extensions.Configuration;
namespace Client.Pages;

public partial class LocationsPage : ContentPage
{
	private readonly IConfiguration _config;
	public LocationsPage(LocationsViewModel vm, IConfiguration config)
	{
		InitializeComponent();
		BindingContext = vm;
		_config = config;
	}
}