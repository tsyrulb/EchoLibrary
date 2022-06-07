using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Services
{
    public class ContactService
    {
        MariaDbContext _context;
        public ContactService(MariaDbContext cd)
        {
            _context = cd;
        }
        public async Task<IEnumerable<Contact>> GetContacts(string username)
        {
            var com = "SELECT * FROM `MariaDbContext`.`contactdb` WHERE `Username`='" + username + "'";
            return await _context.ContactDB.FromSqlRaw(com).ToListAsync();
        }
        [Microsoft.AspNetCore.Mvc.NonAction]
        public async Task<int> AddContact([FromBody] JsonObject contact, string username)
        {
            // check for correct request
            if (contact == null
                || !contact.ContainsKey("id")
                || !contact.ContainsKey("name")
                || !contact.ContainsKey("server"))
                return 400;
            Contact con = new Contact();
            con.id = contact["id"].ToString();
            con.name = contact["name"].ToString();
            con.server = contact["server"].ToString();
            con.messages = new List<Message>();
            User exist = await _context.UserDB.FirstOrDefaultAsync(x => x.Username == con.id);
            Contact existInContacts = await GetContact(con.id, username);
            if (exist == null || existInContacts != null)
                return 400;
            await _context.ContactDB.AddAsync(con);
            await _context.SaveChangesAsync();
            var com = "UPDATE `MariaDbContext`.`contactdb` SET `Username`='"+username+"' WHERE  `id`='"+con.id+"'";
            await _context.Database.ExecuteSqlRawAsync(com);
            await _context.SaveChangesAsync();
            return 201;
        }

        public async Task<Contact> GetContact(string id, string username)
        {
            return  await _context.ContactDB.FromSqlRaw("SELECT * From `MariaDbContext`.`contactdb` Where Username = {0} AND id = {1}", username, id).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteContact(string id, string username)
        {
            if (id == null)
                return 400;
            Contact con = await _context.ContactDB.FirstOrDefaultAsync(x => x.id == id);
            if (con == null)
                return 404;
            _context.ContactDB.Remove(con);
            await _context.SaveChangesAsync();
            return 204;
        }

        public async Task<int> ChangeContact(string id, string username, [FromBody] JsonObject contact)
        {
            Contact con = await GetContact(id, username);
            if (con == null)
                return 404;
            if(contact.ContainsKey("name"))
                con.name = contact["name"].ToString();
            if (contact.ContainsKey("server"))
                con.name = contact["server"].ToString();
            await _context.SaveChangesAsync();
            return 204;
        }

        private static void ConnectToDataAndAdd(string connectionString)
        {
            //Create a SqlConnection to the Northwind database.
            using (SqlConnection connection =
                       new SqlConnection(connectionString))
            {
                //Create a SqlDataAdapter for the Suppliers table.
                SqlDataAdapter adapter = new SqlDataAdapter();

                // A table mapping names the DataTable.
                adapter.TableMappings.Add("Table", "Suppliers");

                // Open the connection.
                connection.Open();
                Console.WriteLine("The SqlConnection is open.");

                // Create a SqlCommand to retrieve Suppliers data.
                SqlCommand command = new SqlCommand(
                    "SELECT SupplierID, CompanyName FROM dbo.Suppliers;",
                    connection);
                command.CommandType = CommandType.Text;

                // Set the SqlDataAdapter's SelectCommand.
                adapter.SelectCommand = command;

                // Fill the DataSet.
                DataSet dataSet = new DataSet("Suppliers");
                adapter.Fill(dataSet);

                // Create a second Adapter and Command to get
                // the Products table, a child table of Suppliers.
                SqlDataAdapter productsAdapter = new SqlDataAdapter();
                productsAdapter.TableMappings.Add("Table", "Products");

                SqlCommand productsCommand = new SqlCommand(
                    "SELECT ProductID, SupplierID FROM dbo.Products;",
                    connection);
                productsAdapter.SelectCommand = productsCommand;

                // Fill the DataSet.
                productsAdapter.Fill(dataSet);

                // Close the connection.
                connection.Close();
                Console.WriteLine("The SqlConnection is closed.");

                // Create a DataRelation to link the two tables
                // based on the SupplierID.
                DataColumn parentColumn =
                    dataSet.Tables["Suppliers"].Columns["SupplierID"];
                DataColumn childColumn =
                    dataSet.Tables["Products"].Columns["SupplierID"];
                DataRelation relation =
                    new System.Data.DataRelation("SuppliersProducts",
                    parentColumn, childColumn);
                dataSet.Relations.Add(relation);
                Console.WriteLine(
                    "The {0} DataRelation has been created.",
                    relation.RelationName);
            }
        }

        static private string GetConnectionString()
        {
            return "Server=localhost;User Id=root;Password=12345;Database=MariaDbContext";
        }
    }

}
