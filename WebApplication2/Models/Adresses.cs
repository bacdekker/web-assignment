using System.IO;
using System.Reflection.Emit;

namespace WebApplication2.Models
{
    public class Address
    {
        public Address(string street = "street", string city = "city", string zipCode = "zipCode", int houseNumber = 0) 
        {
            Street = street;
            City = city;
            ZipCode = zipCode;
            HouseNumber = houseNumber;
        }

        public Address()
        {
            Street = "street";
            City = "city";
            ZipCode = "zipCode";
            HouseNumber = 0;
        }

        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int HouseNumber { get; set; }
    }
}
