namespace WebApplication2.Models
{
    public class Address
    {
        public Address(string street, string city, string zipCode, int houseNumber) 
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
            HouseNumber = houseNumber;

        }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int HouseNumber { get; set; }
    }
}
