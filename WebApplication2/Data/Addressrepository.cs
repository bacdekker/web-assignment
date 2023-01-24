using System.Data;
using WebApplication2.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Sqlite;
using System.Net;

namespace WebApplication2.Data
{
    //TODO: REGEX expressions in order to make sure no SQL injection happens on the input or use a library for that
    public class AddressRepository : IAddressRepository
    {
        private readonly SqliteConnection _db_connection;
        public AddressRepository(SqliteConnection db_connection) 
        {
            _db_connection = db_connection;
            _db_connection.Open();
        }
        public List<(int, Address)> readerToAddresses(SqliteDataReader reader)
        {
            List<(int, Address)> addresses = new List<(int, Address)>();
            while (reader.Read())
            {
                int ID = reader.GetInt32("ID");
                string street =  reader[1].ToString();
                string city   =  reader[2].ToString();
                string zipcode = reader[3].ToString();
                int houseNumber = (int)(long)reader[4];
                addresses.Add((ID, new Address(street, city, zipcode, houseNumber)));

            }
            return addresses;
        }

        public string getSearchAddressesString(string start, Address search_info)
        {
            string search_command_string = start;
            // In case the search command is not null loop through all non null properties and add them to the search
            if (search_info != null)
            {
                foreach (var prop in search_info.GetType().GetProperties())
                {
                    if (prop.GetValue(search_info) != null)
                    {
                        if (prop.PropertyType.Name == "String")
                        {
                            search_command_string += prop.Name.ToString() + " = '" + prop.GetValue(search_info).ToString() + "'" + ", ";
                        }
                        else if (prop.PropertyType.Name == "Int32")
                        {
                            search_command_string += prop.Name.ToString() + " = " + ((int)prop.GetValue(search_info)).ToString() + ", ";
                        }
                    }
                }
            }
            return search_command_string;
        }
        public List<(int, Address)> getAddresses(Address search_info, bool orderByAscending)
        {
            // Initialize search_command in case there is no info in the address to search on
            string search_command_string = $"SELECT * FROM ADDRESSES WHERE ";


            // In case the search command is not null loop through all non null properties and add them to the search
            search_command_string = getSearchAddressesString(search_command_string, search_info);

            // In case there isn't any info to search on just return the entire table
            if (search_command_string == "SELECT * FROM ADDRESSES WHERE ")
                search_command_string = "SELECT * FROM ADDRESSES";

            // Execute command
            SqliteCommand search_command = new SqliteCommand(search_command_string, _db_connection);
            SqliteDataReader reader = search_command.ExecuteReader();
            return readerToAddresses(reader);
        }
        public void InsertAddress(Address address)
        {
            string insert_command_string_part_one = "INSERT INTO ADDRESSES (";
            string insert_command_string_part_two = "VALUES(";
            if (address != null)
            {
                foreach (var prop in address.GetType().GetProperties())
                {
                    insert_command_string_part_one += prop.Name.ToString() + ", ";
                    if (prop.GetValue(address) != null)
                    {
                        if (prop.PropertyType.Name == "String")
                        {
                            insert_command_string_part_two += "'" + prop.GetValue(address).ToString() + "'" + ", "; 
                        }
                        else if (prop.PropertyType.Name == "Int32")
                        {
                            insert_command_string_part_two += ((int)prop.GetValue(address)).ToString() + ", ";
                        }
                    }
                }
            }
            insert_command_string_part_one = insert_command_string_part_one.Substring(0, insert_command_string_part_one.Length - 2) + ") ";
            insert_command_string_part_two = insert_command_string_part_two.Substring(0, insert_command_string_part_two.Length - 2) + ')';
            SqliteCommand insert_command = new SqliteCommand(insert_command_string_part_one + insert_command_string_part_two, _db_connection);
            insert_command.ExecuteNonQuery();
        }

        public void UpdateAddress(Address address, int id)
        {
            bool not_all_null = false;
            string update_command_string = "UPDATE ADDRESSES SET ";
            if (address != null)
            {
                foreach (var prop in address.GetType().GetProperties())
                {
                    if (prop.GetValue(address) != null)
                    {
                        not_all_null = true;
                        if (prop.PropertyType.Name == "String")
                        {
                            update_command_string += prop.Name.ToString() + " = '" + prop.GetValue(address).ToString() + "'" + ", ";
                        }
                        else if (prop.PropertyType.Name == "Int32")
                        {
                            update_command_string += prop.Name.ToString() + " = " + ((int)prop.GetValue(address)).ToString() + ", ";
                        }
                    }
                }
                if (not_all_null) 
                {
                    update_command_string = update_command_string.Substring(0, update_command_string.Length - 2) + $" WHERE ID = {(int)id}";
                    SqliteCommand update_command = new SqliteCommand(update_command_string, _db_connection);
                    update_command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteAddress(int id)
        {
            string delete_command_string = $"DELETE FROM ADDRESSES WHERE ID = {id}";
            SqliteCommand delete_command = new SqliteCommand(delete_command_string, _db_connection);
            delete_command.ExecuteNonQuery();
        }

        public bool existAddress(int id)
        {
            string exists_command_string = $"SELECT * FROM ADDRESSES WHERE ID={id}";
            SqliteCommand exists_command = new SqliteCommand(exists_command_string, _db_connection);
            SqliteDataReader reader = exists_command.ExecuteReader();

            while (reader.Read())
                return true;

            return false;
        }

        public Address getSingleAddress(int id)
        {
            string get_address_string = $"SELECT * FROM ADDRESSES WHERE ID={id}";
            SqliteCommand single_address_command = new SqliteCommand(get_address_string, _db_connection);
            SqliteDataReader reader = single_address_command.ExecuteReader();
            var collection = readerToAddresses(reader);
            return collection[0].Item2;
        }
    }
}
