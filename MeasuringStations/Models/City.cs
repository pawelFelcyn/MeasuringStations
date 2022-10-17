namespace MeasuringStations.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Commune Commune { get; set; }
    }
}
