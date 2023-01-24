using System.Diagnostics.CodeAnalysis;
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
            Street = null;
            City = null;
            ZipCode = null;
            HouseNumber = null;
        }

        public bool Validate()
        {
            return true;
        }

        [AllowNull]
        public string Street { get; set; }
        [AllowNull]
        public string City { get; set; }
        [AllowNull]
        public string ZipCode { get; set; }
        [AllowNull]
        public int? HouseNumber { get; set; }
    }
}
