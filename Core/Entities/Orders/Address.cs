namespace Core.Entities.Orders
{
    public class Address
    {
        public Address() { }
        public Address(string street, string apartment, string city, string zipCode, string country)
        {
            Street = street;
            Apartment = apartment;
            City = city;
            ZipCode = zipCode;
            Country = country;
        }
        public string Street { get; set; } = string.Empty;
        public string Apartment { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
