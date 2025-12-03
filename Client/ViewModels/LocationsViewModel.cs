using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client.ViewModels;
 public partial class LocationsViewModel : ObservableObject
{
	private readonly IConfiguration _config;
	private readonly IHttpClientFactory _httpClientFactory;

	[ObservableProperty]
	private int? dniNazaj;

	[ObservableProperty]
	private string labelText;

	[ObservableProperty]
	private string carId;

	[ObservableProperty]
	private ObservableCollection<CarLocation> locations = new();

	public LocationsViewModel(IConfiguration config, IHttpClientFactory httpClientFactory)
	{
		_config = config;
		_httpClientFactory = httpClientFactory;
	}

	[RelayCommand]
	private async void LookupLocations()
	{
		try { 
			var baseUrl = _config["BaseUrl"] ?? throw new Exception("BaseUrl not configured.");
			using var client = _httpClientFactory.CreateClient();

			string url;

			if (!string.IsNullOrWhiteSpace(CarId))
				url = $"{baseUrl}/car/{CarId}";
			else
				url = $"{baseUrl}/car";

			if (DniNazaj.HasValue)
				url += $"?timeframe={DniNazaj.Value}";


			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var locationData = JsonSerializer.Deserialize<List<CarLocation>>(data, new JsonSerializerOptions{PropertyNameCaseInsensitive = true});

					Locations.Clear();
					foreach(var l in locationData!.AsEnumerable().Reverse())
					{
						Locations.Add(l);
					}
					LabelText = $"Prikazujem {Locations.Count} zadetkov";
				}
			else
				{
					LabelText = $"Napaka: {response.StatusCode}";
					Locations.Clear();
				}
		}
        catch (Exception ex)
        {
            LabelText = $"Izjema: {ex.Message}";
			Locations.Clear();
		}
	}

	public class CarLocation
	{
		public string CarId { get; set; }
		public double Latitude { get; set; }
		public double Longitude { get; set; }
		public DateTime Time { get; set; }
	}
}
