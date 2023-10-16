namespace Emerge.Models
{
    public class Company
    {
        //     Id, Name, Gstin, Cin, BuildingNumber, Line1, Line2, City, State, PostalCode, Country,  Telephone1, Telephone2, Url
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gstin { get; set; }
        public string Cin { get; set; }
        public string BuildingNumber { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
   
        public string Country { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Url { get; set; }
    }
}
