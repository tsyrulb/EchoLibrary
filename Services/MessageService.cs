using Domain;
using Microsoft.AspNetCore.Mvc;
using Repository;
using System.Text.Json.Nodes;

namespace Services
{
    public class MessageService
    {

        ContextData _context;

        public MessageService(ContextData cd)
        {
            _context = cd;
        }
        public List<Message> GetMessages(string username, string contactId)
        {
           // return _context.GetContacts(username).Find(person => person.id == contactId).messages;
           return _context.getMessages(username, contactId).ToList();
        }
        public int AddMessage(string username, string contactId, [FromBody] JsonObject message)
        {
            if (!isContactExist(username, contactId))
                return 404;
            if (!message.ContainsKey("content"))
                return 400;
            List<Message> messages = GetMessages(username, contactId);
            Message m = new Message();
            m.id = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
            m.content = message["content"].ToString();
            m.sent = bool.Parse(message["sent"].ToString());
            m.created = DateTime.Now;
            messages.Add(m);
            _context.setLast(username, contactId, m.content);
            _context.setMessage(messages, username, contactId);
            return 201;
        }


        public Message GetMessage(long id, string username, string contactId)
        {
            List<Message> messages = GetMessages(username, contactId);
             return messages.Find(m => m.id == id);
        }
        public int DeleteMessage(long messageid, string username, string contactId)
        {
            if (!isContactExist(username, contactId) 
                || !isMessageExist(messageid, username, contactId))
                return 404;
            List<Message> messages = GetMessages(username, contactId);
            Message m = messages.Find(x => x.id == messageid);
            messages.Remove(m);
            _context.setMessage(messages, username, contactId);
            return 204;
        }

        public int ChangeMessage(string contactid, long messageId, string username, [FromBody] JsonObject content)
        {
            Message msg = GetMessage(messageId, username, contactid);
            if (msg == null)
                return 404;
            if (!content.ContainsKey("content"))
                return 400;
            msg.content = content["content"].ToString();
            return 204;
        }

        public Boolean isContactExist(string user, string contact)
        {
            if (_context.GetContacts(user).Find(x => x.id == contact) == null)
                return false;
            return true;
        }
        public Boolean isMessageExist(long id, string username, string contactId)
        {
            if (_context.GetContacts(username).Find(x => x.id == contactId).messages.Find(x => x.id == id) == null)
                return false;
            return true;
        }
    }
}