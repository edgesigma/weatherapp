public class HttpRequestDemo
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("testing the National Weather Svc. API");

        // Create an instance of HttpClient
        using HttpClient client = new();

        try
        {
            // Define the URI for the GET request
            string uri = "https://api.weather.gov/points/39.7456,-97.0892";

            // client.DefaultRequestHeaders.Add("accept-encoding", "gzip, deflate, br, zstd");
            // client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            // client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
            // client.DefaultRequestHeaders.Add("cache-control", "no-cache");
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
}