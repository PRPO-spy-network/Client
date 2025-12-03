using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Devices.Sensors;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.ViewModels;
public partial class RegistrationsViewModel : ObservableObject
{
	private readonly IConfiguration _config;
	private readonly IHttpClientFactory _httpClientFactory;

	[ObservableProperty]
	private string labelText;

	[ObservableProperty]
	private string registrationToGet;

	[ObservableProperty]
	private string registrationToPush;
	[ObservableProperty]
	private string regionToPush;

	[ObservableProperty]
	private ObservableCollection<CarRegistration> registrations = new();

	public RegistrationsViewModel(IConfiguration config, IHttpClientFactory httpClientFactory)
	{
		_config = config;
		_httpClientFactory = httpClientFactory;
	}

	[RelayCommand]
	private async void Registriraj()
	{
		Registrations.Clear();
		try
		{
			var baseUrl = _config["BaseUrl"] ?? throw new Exception("BaseUrl not configured.");
			using var client = _httpClientFactory.CreateClient();

			string url = $"{baseUrl}/registration"; ;

			if (string.IsNullOrWhiteSpace(RegistrationToPush))
			{
				LabelText = $"Registracija ni vpisana";
				return;
			}

			var registration = new CarRegistration
			{
				Id = RegistrationToPush,
				Region = RegionToPush
			};

			var response = await client.PostAsJsonAsync(url, registration);
			if (response.IsSuccessStatusCode)
			{
				var result = await response.Content.ReadAsStringAsync();
				LabelText = "Vnešeno";
			}
			else
			{
				var text = await response.Content.ReadAsStringAsync();
				LabelText = $"Napaka: {response.StatusCode}, {text}";
			}
		}
		catch (Exception ex)
		{
			LabelText = $"Izjema: {ex.Message}";
		}
	}

	[RelayCommand]
	private async void Oglej()
	{
		try
		{
			var baseUrl = _config["BaseUrl"] ?? throw new Exception("BaseUrl not configured.");
			using var client = _httpClientFactory.CreateClient();

			string url;

			if (!string.IsNullOrWhiteSpace(RegistrationToGet))
				url = $"{baseUrl}/registration/{RegistrationToGet}";
			else
				url = $"{baseUrl}/registration";


			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync(); 
				if (string.IsNullOrWhiteSpace(RegistrationToGet)) // Če je več objektov
				{
					var locationData = JsonSerializer.Deserialize<List<CarRegistration>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

					Registrations.Clear();
					foreach (var l in locationData!)
					{
						Registrations.Add(l);
					}
				}
				else
				{
					var locationData = JsonSerializer.Deserialize <CarRegistration>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
					Registrations.Clear();
					Registrations.Add(locationData!);
				}
					LabelText = $"Prikazujem {Registrations.Count} zadetkov";
			}
			else
			{
				LabelText = $"Napaka: {response.StatusCode}";
				Registrations.Clear();
			}
		}
		catch (Exception ex)
		{
			LabelText = $"Izjema: {ex.Message}";
			Registrations.Clear();
		}
	}

	public class CarRegistration
	{
		public string Id { get; set; }
		public string Region { get; set; }
	}
}

