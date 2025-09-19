using System.Threading.Tasks;
using System.Web;
using System.Text.Json;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "ForecastAPI"; 
    config.Title = "ForecastAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "ForecastAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// serve static files
app.UseDefaultFiles();

app.MapPost("/address/submit", async (AddressRequest request) =>
{
    // url encode supplied address
    string escapedUserAddress = HttpUtility.UrlEncode(request.Address);

    // Create an instance of HttpClient
    using HttpClient client = new();

    // Define the URI for the GET request
    string uri = $"https://geocoding.geo.census.gov/geocoder/locations/onelineaddress?address={escapedUserAddress}&benchmark=4&format=json";

    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");

    // Send the GET request and get the response
    HttpResponseMessage response = await client.GetAsync(uri);

    // Ensure the request was successful (status code 2xx)
    response.EnsureSuccessStatusCode();

    string responseBody = await response.Content.ReadAsStringAsync();

    // deserialize the JSON response
    GeocodeResponse responseData = JsonSerializer.Deserialize<GeocodeResponse>(responseBody);

    // Check if we got any matches
    if (responseData?.result?.addressMatches == null || responseData.result.addressMatches.Count == 0)
    {
        // Return empty array if no matches found
        return Results.Ok(new List<LocationMatch>());
    }
    
    // Return the array of address matches (could be 1 or more elements)
    return Results.Ok(responseData.result.addressMatches);


});

app.MapPost("/address/select", async (AddressSelectRequest request) =>
{
    // Create an instance of HttpClient
    using HttpClient client = new();

    // Define the URI for the GET request
    string uri = $"https://api.weather.gov/points/{request.LatLong}";

    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36");

    var weatherResponse = await client.GetAsync(uri);

    // Ensure the request was successful (status code 2xx)
    weatherResponse.EnsureSuccessStatusCode();

    string weatherBody = await weatherResponse.Content.ReadAsStringAsync();

    // Parse JSON to get the forecast URL
    var pointsData = JsonSerializer.Deserialize<JsonElement>(weatherBody);
    string forecastUrl = pointsData.GetProperty("properties").GetProperty("forecast").GetString();
    
    // Call the forecast API using the URL we just got
    var forecastResponse = await client.GetAsync(forecastUrl);
    // Ensure the forecast request was successful
    forecastResponse.EnsureSuccessStatusCode();
    // Read the forecast response as text
    string forecastBody = await forecastResponse.Content.ReadAsStringAsync();
    
    // Parse the forecast JSON and get only the first 7 days
    var forecastData = JsonSerializer.Deserialize<JsonElement>(forecastBody);
    var periods = forecastData.GetProperty("properties").GetProperty("periods").EnumerateArray().Take(7).ToArray();
    
    // Return the 7-day forecast to the frontend
    return Results.Ok(periods);
});

// app.MapGet("/todoitems/complete", async (TodoDb db) =>
//     await db.Todos.Where(t => t.IsComplete).ToListAsync());


app.Run();

public class AddressRequest
{
    public string Address { get; set; }
}

public class AddressSelectRequest
{
    public string Address { get; set; }
    public string LatLong { get; set; }
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