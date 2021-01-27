using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace AddressBookLogic
{
    public class DataStorage
    {

        private List<ContactViewModel> _contactList = new List<ContactViewModel>();
        public List<ContactViewModel> ContactList { get => _contactList; }


        public void Save()
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                // check if new db-file or existing
                var command = con.CreateCommand();
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name in ('Contacts', 'Links')";
                var result = command.ExecuteScalar();
                if ((Int64)result != 2)
                {
                    // tabelle neu aufbauen
                    command.CommandText = "create table Contacts (firstname varchar(50) not null, lastname varchar(50) not null, street  varchar(50) not null, houseno varchar(50) not null, zip  varchar(50) not null, city varchar(50) not null, state varchar(50) not null, country varchar(50) not null)";
                    command.ExecuteNonQuery();
                    command.CommandText = "create table Links (contactID int not null, link varchar(150))";
                    command.ExecuteNonQuery();
                }

                command.CommandText = "delete from Contacts;"; // sqlite kennt kein truncate als extra befehl. delete ohne where wird automatisch zu tuncate
                command.ExecuteNonQuery();
                foreach (var item in ContactList)
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
                    command.CommandText = "select last_insert_rowid()";
                    result = command.ExecuteScalar();
                    command.CommandText = "insert into Links values (@ContactID, @Link)";
                    foreach (var webProfile in item.WebProfiles)
                    {
                        command.Parameters.AddWithValue("@ContactID", result);
                        command.Parameters.AddWithValue("@Link", webProfile.Link);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public void Load()
        {
            SQLiteConnectionStringBuilder builder = new();
            builder.Version = 3;
            builder.DataSource = "Contacts.db";

            _contactList = new();
            using (SQLiteConnection con = new(builder.ToString()))
            {
                con.Open();
                var command = con.CreateCommand();
                var commandLink = con.CreateCommand();
                // check if new db-file or existing
                command.CommandText = "SELECT count(name) FROM sqlite_master WHERE type = 'table' AND name = 'Contacts'";
                var result = command.ExecuteScalar();
                if ((Int64)result != 0)
                {
                    command.CommandText = "select firstname, lastname, street, houseno, zip, city, state, country, rowid from contacts order by firstname;";
                    commandLink.CommandText = "select link from links where contactID = @contactID;";
                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var newContact = new ContactViewModel(
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5),
                            reader.GetString(6),
                            reader.GetString(7));
                        int contactID = reader.GetInt32(8);

                        commandLink.Parameters.Clear();
                        commandLink.Parameters.AddWithValue("@contactID", contactID);

                        using (var linkReader = commandLink.ExecuteReader())
                            while (linkReader.Read())
                                newContact.WebProfiles.Add(new WebLinkViewModel { Link = linkReader.GetString(0) });
                        
                        _contactList.Add(newContact);
                    }
                }
            }
        }
    }
}
