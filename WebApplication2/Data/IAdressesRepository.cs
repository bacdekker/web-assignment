using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public interface IAddressRepository
    {
        (List<Address>, List<int>) getAddresses(Address address, bool orderByAscending);

        Address getSingleAddress(int id);
        void InsertAddress(Address address);
        void UpdateAddress(Address address, int ID);
        void DeleteAddress(int ID);
        bool existAddress(int ID);
    }
}
