using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using Domain;

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
            return await _context.ContactDB.ToListAsync();
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
            //           List<Contact> contacts = _context.GetContacts(username);
            //            Contact exist = contacts.Find(person => person.id == con.id);
            Contact exist = await _context.ContactDB.FirstOrDefaultAsync(x => x.id == con.id);
            if(exist == null)
                return 400;
            await _context.ContactDB.AddAsync(con);
            await _context.SaveChangesAsync();
            return 201;
        }

        public async Task<Contact> GetContact(string id, string username)
        {
            //Contact contact = _context.GetContacts(username).FirstOrDefault(person => person.id == id);
            return await _context.ContactDB.FirstOrDefaultAsync(x => x.id == id);
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
    }

}
