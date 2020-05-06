namespace Ukor.Configuration
{
    public class LocalServerOptions
    {
        public string IpAddress { get; set; }
        public string RootUrl => $"http://{IpAddress}:8060";
    }
}
