namespace Emerge.Models
{
    public class Product
    {
        // Id, Name, FrequencyId, Amount, ClientId
        public int Id { get; set; }
        public string Name { get; set; }
        public int FrequencyId { get; set; }
        public string Amount { get; set; }
        public int ClientId { get; set; }
    }
}
