using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;

public class HttpRequestDemo
{
    private static string userAddress = null;
    private static string[] geoResults = null;
    private static string LatLong = null;

    private static void SetAddress() // prompt user for address
    {
        Console.WriteLine("Enter an address: ");
        userAddress = Console.ReadLine();
    }

    private static async Task<string> FetchLocationData(string address) // fetch location(s) from census data API
    {
        // url encode supplied address
        string escapedUserAddress = HttpUtility.UrlEncode(address);

        // Create an instance of HttpClient
        using HttpClient client = new();

        // Define the URI for the GET request
        string uri = $"https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={escapedUserAddress}&benchmark=4&format=json";

        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");

        // Send the GET request and get the response
        HttpResponseMessage response = await client.GetAsync(uri);

        // Ensure the request was successful (status code 2xx)
        response.EnsureSuccessStatusCode();

        // return response body
        return await response.Content.ReadAsStringAsync();

    }

    private static string SelectAddress() // offer user a list of options to choose
    {
        return "address";
    }

    private static async void FetchForecastData(string Location)
    {
        // Create an instance of HttpClient
        using HttpClient client = new();

        try
        {
            // Define the URI for the GET request
            // string uri = "https://api.weather.gov/points/39.7456,-97.0892";
            string uri = $"https://api.weather.gov/points/{Location}";

            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");

            // Send the GET request and get the response
            HttpResponseMessage response = await client.GetAsync(uri);

            // Ensure the request was successful (status code 2xx)
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Print the response body
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
        }
    }

    public static async Task Main(string[] args)
    {
        // prompt user for address
        SetAddress();

        // use provided address to fetch location data
        string results = await FetchLocationData(userAddress);

        Console.WriteLine(results);

        GeocodeResponse responseData = JsonSerializer.Deserialize<GeocodeResponse>(results);

        if (responseData.result.addressMatches != null)
        {
            Console.WriteLine("we have matches!");
        }
        else
        {
            Console.WriteLine("no matches");
        }

    }

    public class Coordinates
    {
        public float x { get; set; }
        public float y { get; set; }
    }

    public class LocationMatch
    {
        public Coordinates coordinates { get; set; }
        public string matchedAddress { get; set; }
    }

    public class ResultObject
    {
        public List<LocationMatch> addressMatches { get; set; }
    }

    public class GeocodeResponse
    {
        public ResultObject result { get; set; }
    }
}