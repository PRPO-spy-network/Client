using Client.ViewModels;
namespace Client.Pages;

public partial class RegistrationsPage : ContentPage
{
	public RegistrationsPage(RegistrationsViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}