using MauiApp1.Model;
using System.Text.Json;
using System.Text;
using System.Diagnostics.Metrics;

namespace MauiApp1;

public partial class EditPage : ContentPage
{
    public Country Country { get; set; }
    public EditPage(Country selectedCountry)
	{
		InitializeComponent();
        Country = selectedCountry;

        
        editCountryNameEntry.Text = Country.name;
        editZipCodeEntry.Text = Country.zipCode.ToString();
        editContinentEntry.Text = Country.continent;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        // Update the Country object with the edited data
        Country.name = editCountryNameEntry.Text;
        Country.zipCode = int.Parse(editZipCodeEntry.Text);
        Country.continent = editContinentEntry.Text;

        
        // Make an HTTP PUT or PATCH request to update the data on your API

        // For example, you can use the HttpClient class to send a request to your API
       
        string yourApiUrl = "http://localhost:5078/api/Countries";

        using (var httpClient = new HttpClient())
        {
            try
            {
                // Serialize the updated Country object to JSON
                var jsonContent = JsonSerializer.Serialize(Country);

                // Create HttpContent with the JSON data
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Send a PUT or PATCH request to update the data
                var response = await httpClient.PutAsync(yourApiUrl + "/" + Country.id, content);

                if (response.IsSuccessStatusCode)
                {
                    // The update was successful

                    
                    await Navigation.PopAsync();
                }
                else
                {
                    // Handle the error if the update fails
                    await DisplayAlert("Error", "Failed to update the country.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "An error occurred: " + ex.Message, "OK");
            }
        }
    }

    private async void Cancel_Button_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PopAsync();
    }

}