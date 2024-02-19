using MauiApp1.Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Country> countriesList = new ObservableCollection<Country>();

        public MainPage()
        {
            InitializeComponent();
           
        }
        protected override void OnAppearing()
        {
           
            LoadDataFromApiAsync();
            base.OnAppearing();
        }

        public async Task LoadDataFromApiAsync()
        {
            try
            {
                string apiURL = "http://localhost:5078/api/Countries";
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(apiURL);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<Country> serviceResponse = JsonSerializer.Deserialize<List<Country>>(content);
                    countriesList = new ObservableCollection<Country>(serviceResponse);
                    lstCountry.ItemsSource = countriesList;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Unable to fetch data from the API", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }

        private async void AddCountryButton_Clicked(object sender, EventArgs e)
        {
            // Create a new Country object from user input
            Country newCountry = new Country
            {
                name = countryNameEntry.Text,
                zipCode = Convert.ToInt32(zipCodeEntry.Text),
                continent = continentEntry.Text
                // Add other properties as needed
            };

            // Send a POST request to add the country to the API
            string apiURL = "http://localhost:5078/api/Countries";
            HttpClient httpClient = new HttpClient();

            string jsonContent = JsonSerializer.Serialize(newCountry);
            HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                // Update the local list of countries
                countriesList.Add(newCountry);

                // Clear the input fields
                countryNameEntry.Text = string.Empty;
                zipCodeEntry.Text = string.Empty;
                continentEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Failed to add the country.", "OK");
            }
        }


        private async void btnEdit_Clicked(object sender, EventArgs e)
        {
            var clickedButton = (Button)sender;
            Country selectedCountry = (Country)clickedButton.BindingContext;

            EditPage etd = new EditPage(selectedCountry);
            await Navigation.PushAsync(etd);
        }
        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var country = (Country)button.BindingContext; // Get the selected country

            // Confirm with the user before deleting
            bool answer = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this country?", "Yes", "No");

            if (answer)
            {
                // Send a DELETE request to remove the country from the server
                string yourApiUrl = $"http://localhost:5078/api/Countries/{country.id}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.DeleteAsync(yourApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        // If the delete was successful, remove the country from the local list
                        countriesList.Remove(country);
                    }
                    else
                    {
                        // Handle the error if the delete fails
                        await DisplayAlert("Error", "Failed to delete the country.", "OK");
                    }
                }
            }
        }

    }
}

