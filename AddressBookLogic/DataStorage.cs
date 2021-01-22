using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AddressBookLogic
{
    class DataStorage
    {
        public static void Save(List<ContactViewModel> ContactsToStore)
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                // check if new db-file or existing
                var command = con.CreateCommand();
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name = 'Contacts'";
                var result = command.ExecuteScalar();
                if ((Int64)result == 0)
                {
                    // tabelle neu aufbauen
                    command.CommandText = "create table Contacts (firstname varchar(50) not null, lastname varchar(50) not null, street  varchar(50) not null, houseno varchar(50) not null, zip  varchar(50) not null, city varchar(50) not null, state varchar(50) not null, country varchar(50) not null)";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "delete from Contacts;"; // sqlite kennt kein truncate als extra befehl. delete ohne where wird automatisch zu tuncate
                command.ExecuteNonQuery();
                foreach (var item in ContactsToStore)
                {
                    command.CommandText = "insert into Contacts values (@firstname, @lastname, @street, @houseno, @zip, @city, @state, @country);";
                    command.Parameters.AddWithValue("firstname", item.FirstName);
                    command.Parameters.AddWithValue("lastname", item.LastName);
                    command.Parameters.AddWithValue("street", item.Street);
                    command.Parameters.AddWithValue("houseno", item.HouseNo);
                    command.Parameters.AddWithValue("zip", item.ZIP);
                    command.Parameters.AddWithValue("city", item.City);
                    command.Parameters.AddWithValue("state", item.State);
                    command.Parameters.AddWithValue("country", item.Country);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<ContactViewModel> Load()
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            List<ContactViewModel> resultList = new();
            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                var command = con.CreateCommand();
                // check if new db-file or existing
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name = 'Contacts'";
                var result = command.ExecuteScalar();
                if ((Int64)result != 0)
                {
                    command.CommandText = "select firstname, lastname, street, houseno, zip, city, state, country from contacts order by firstname;";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                        resultList.Add(new ContactViewModel(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7)));
                }
            }

            return resultList;
        }
    }
}
