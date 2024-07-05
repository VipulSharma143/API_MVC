namespace API.Models
{
    public class NationalPark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string State { get; set; }
        public byte[]? Picture { get; set; }
        public DateTime Created { get; set; }
        public DateTime Established { get; set; }
    }
}
