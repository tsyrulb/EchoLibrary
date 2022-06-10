using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using System.Text.Json.Nodes;

namespace Services
{
    public class MessageService
    {

        MariaDbContext _context;

        public MessageService(MariaDbContext cd)
        {
            _context = cd;
        }

        public async Task<int> getContactID(string username, string contactId)
        {
            Contact contact = await _context.ContactDB.FromSqlRaw("SELECT * From `MariaDbContext`.`contactdb` Where Username = {0} AND id = {1}", username, contactId).FirstOrDefaultAsync();
            return contact.ContactID;
        }

        public async Task<List<Message>> GetMessages(string username, string contactId)
        {
            int id = await getContactID(username, contactId);
            var com = "SELECT * FROM `MariaDbContext`.`messagedb` WHERE `ContactID`='" + id + "'";
            List<Message> messages = await _context.MessageDB.FromSqlRaw(com).ToListAsync();
            return messages;
        }
        public async Task<int> AddMessage(string username, string contactId, [FromBody] JsonObject message)
        {
            if (! await isContactExist(username, contactId))
                return 404;
            if (!message.ContainsKey("content"))
                return 400;
            List<Message> messages = await GetMessages(username, contactId);
            Message m = new Message();
            m.content = message["content"].ToString();
            m.sent = bool.Parse(message["sent"].ToString());
            m.created = DateTime.Now;
            messages.Add(m);
            int contact_id = await getContactID(username, contactId);
            Contact contact = await _context.ContactDB.FirstOrDefaultAsync(x => x.ContactID == contact_id);
            contact.last = m.content.ToString();
            contact.messages = messages;
            contact.lastdate = DateTime.Now;
            await _context.SaveChangesAsync();
            return 201;
        }


        public async Task<Message> GetMessage(int id, string username, string contactId)
        {
            return await _context.MessageDB.FirstOrDefaultAsync(x => x.id == id);
        }
         
        public async Task<int> DeleteMessage(int messageid, string username, string contactId)
        {
            if (! await isContactExist(username, contactId) 
                || ! await isMessageExist(messageid, username, contactId))
                return 404;
            Message message = await _context.MessageDB.FirstOrDefaultAsync(x => x.id == messageid);
            int contact_id = await getContactID(username, contactId);
            Contact contact = await _context.ContactDB.FirstOrDefaultAsync(x => x.ContactID == contact_id);
            _context.MessageDB.Remove(message);
            await _context.SaveChangesAsync();
            return 204;
        }

        public async Task<int> ChangeMessage(string contactid, int messageId, string username, [FromBody] JsonObject content)
        {
            Message msg = await GetMessage(messageId, username, contactid);
            if (msg == null)
                return 404;
            if (!content.ContainsKey("content"))
                return 400;
            msg.content = content["content"].ToString();
            await _context.SaveChangesAsync();
            return 204;
        }

        public async Task<Boolean> isContactExist(string user, string contact)
        {
            if (await _context.ContactDB.FirstAsync(x => x.id == contact) == null)
                return false;
            return true;
        }
        public async Task<Boolean> isMessageExist(int id, string username, string contactId)
        {
            if (await _context.MessageDB.FirstAsync(x => x.id == id) == null)
                return false;
            return true;
        }
    }
}