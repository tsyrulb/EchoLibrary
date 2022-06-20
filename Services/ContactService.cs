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
            List<Contact> contacts =  await _context.ContactDB.FromSqlRaw(com).ToListAsync();
            return contacts;
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
            Contact con = await GetContact(id, username);
            if (con == null)
                return 404;
            await _context.Database.ExecuteSqlRawAsync("DELETE FROM `MariaDbContext`.`messagedb` WHERE ContactID = {0} AND sent = {1}", con.ContactID, 1);
            _context.ContactDB.Remove(con);
            await _context.SaveChangesAsync();
            return 204;
        }

        public async Task<int> ChangeContact(string id, string username, [FromBody] JsonObject contact)
        {
            Contact con = await GetContact(id, username);
            if (con == null)
                return 404;
            if (contact.ContainsKey("name"))
                _context.ContactDB.FromSqlRaw("UPDATE `MariaDbContext`.`contactdb` SET 'name' = {2} Where Username = {0} AND id = {1}", username, id, contact["name"].ToString());
            con.name = contact["name"].ToString();
            if (contact.ContainsKey("server"))
                _context.ContactDB.FromSqlRaw("UPDATE `MariaDbContext`.`contactdb` SET 'server' = {2} Where Username = {0} AND id = {1}", username, id, contact["server"].ToString());
            con.server = contact["server"].ToString();
            await _context.SaveChangesAsync();
            return 204;
        }

    }

}
