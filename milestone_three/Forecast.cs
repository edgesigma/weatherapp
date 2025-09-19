public class Forecast
{
    public static string userAddress { get; set; }
    public static string LatLong { get; set; }
    public static GeocodeResponse responseData { get; set; }


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