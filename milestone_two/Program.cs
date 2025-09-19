using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;

public class HttpRequestDemo
{
    private static string userAddress = null;
    // private static string geoResults = null;
    private static string LatLong = null;
    private static GeocodeResponse responseData = null;




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

    private static void SelectAddress() // offer user a list of options to choose
    {
        Dictionary<string, string>[] arrayOfMatches = new Dictionary<string, string>[responseData.result.addressMatches.Count];
        for (int i = 0; i < arrayOfMatches.Length; i++)
        {
            arrayOfMatches[i] = new Dictionary<string, string>();
        }


        int ctr = 0;

        Console.WriteLine("Your search matched multiple addresses. Please select one.");

        foreach (LocationMatch match in responseData.result.addressMatches)
        {
            arrayOfMatches[ctr++].Add(match.matchedAddress, $"{match.coordinates.y},{match.coordinates.x}");
            Console.WriteLine($"{ctr}. " + match.matchedAddress);
        }

        int selection = int.Parse(Console.ReadLine());
        selection-=1;

        LatLong = arrayOfMatches[selection].Values.ElementAt(0);
        Console.WriteLine("lat/long: " + LatLong);

    }

    private static async Task FetchForecastData(string Location)
    {
        Console.WriteLine("now, fetch weather data...");

        // Create an instance of HttpClient
        using HttpClient client = new();

        try
        {
            // Define the URI for the GET request
            string uri = $"https://api.weather.gov/points/{Location}";

            Console.WriteLine(uri);

            client.Timeout = TimeSpan.FromSeconds(10);
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");

            // Send the GET request and get the response
            HttpResponseMessage response = await client.GetAsync(uri);

            // Ensure the request was successful (status code 2xx)
            response.EnsureSuccessStatusCode();

            // Read the response content as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Print the response body
            Console.WriteLine(responseBody);

            // debug statements
            Console.WriteLine($"response headers: {response.Headers}");
            Console.WriteLine($"response headers: {response.StatusCode}");
            // Console.WriteLine($"response headers: {response.Headers}");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (TaskCanceledException e)
        {
            Console.WriteLine("weather forecast request timed out!");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
        }
    }

    public static async Task Main(string[] args)
    {
        while (LatLong == null)
        {
            // prompt user for address
            SetAddress();

            // fetch geolocation data with user-supplied address
            string results = await FetchLocationData(userAddress);

            // debug statement
            Console.WriteLine(results);

            // deserialize the JSON response
            responseData = JsonSerializer.Deserialize<GeocodeResponse>(results);

            if (responseData.result.addressMatches.Count > 1)
            {
                SelectAddress(); // rename disambiguateAddress
            }
            else
            {
                Dictionary<string, string>[] arrayOfMatches = new Dictionary<string, string>[responseData.result.addressMatches.Count];
                Console.WriteLine("check single array length: " + responseData.result.addressMatches.Count);
                for (int i = 0; i < arrayOfMatches.Length; i++)
                {
                    arrayOfMatches[i] = new Dictionary<string, string>();
                }

                Console.WriteLine("only one match.");

                int ctr = 0;

                foreach (LocationMatch match in responseData.result.addressMatches)
                {
                    // Console.WriteLine(LocationMatch.ToString());
                    arrayOfMatches[ctr].Add(match.matchedAddress, $"{match.coordinates.y},{match.coordinates.x}");
                }

                LatLong = arrayOfMatches[0].Values.ElementAt(0);
                Console.WriteLine("lat/long: " + LatLong);

            }
        }

        await FetchForecastData(LatLong);
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