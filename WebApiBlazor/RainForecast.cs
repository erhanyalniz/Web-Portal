namespace WebApiBlazor
{
    public class RainForecast
    {
        public DateOnly Date { get; set; }

        public int ChanceOfRain { get; set; }
        public string RainType { get; set; }
    }
}
