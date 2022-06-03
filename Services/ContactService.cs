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
        ContextData _context;
        public ContactService(ContextData cd)
        {
            _context = cd;
        }
        public IEnumerable<Contact> GetContacts(string username)
        {
            return _context.GetContacts(username);
        }
        [Microsoft.AspNetCore.Mvc.NonAction]
        public int AddContact([FromBody] JsonObject contact, string username)
        {
            // check for correct request
            if (contact == null
                || !contact.ContainsKey("id")
                || !contact.ContainsKey("name")
                || !contact.ContainsKey("server"))
                return 400; 
            // create Contact from JSON
 //           if (!_context.isUserExit(contact["id"].ToString()))
 //               return 404;
            Contact con = new Contact();
            con.id = contact["id"].ToString(); 
            con.name = contact["name"].ToString();
            con.server = contact["server"].ToString();
            con.messages = new List<Message>();
            List<Contact> contacts = _context.GetContacts(username);
            Contact exist = contacts.Find(person => person.id == con.id);
            if (exist != null)
                return 400;
            contacts.Add(con);
            _context.setContacts(contacts, username);
            return 201;
        }

        public Contact GetContact(string id, string username)
        {
            Contact contact = _context.GetContacts(username).FirstOrDefault(person => person.id == id);
            return contact;
        }

        public int DeleteContact(string id, string username)
        {
            if (id == null)
                return 400;
            List<Contact> contacts = _context.GetContacts(username);
            Contact con = GetContact(id, username);
            if (con == null)
                return 404;
            contacts.Remove(con);
            _context.setContacts(contacts, username);
            return 204;
        }

        public int ChangeContact(string id, string username, [FromBody] JsonObject contact)
        {
            Contact con = GetContact(id, username);
            if (con == null)
                return 404;
            if(contact.ContainsKey("name"))
                con.name = contact["name"].ToString();
            if (contact.ContainsKey("server"))
                con.name = contact["server"].ToString();
            return 204;
        }
    }

}
