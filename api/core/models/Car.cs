namespace api.core.models
{
    public class Car
    {
        public int? CarId { get; set; }
        public string Make { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public int Deleted { get; set; }
    }
}