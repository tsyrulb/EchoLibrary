using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Domain;

namespace Repository
{
    public class ContextData : DbContext
    {

        private User _bodek = new User()
        {
            Username = "Bodek",
            Nickname = "Bodek AP",
            Password = "w2",
            contacts = new List<Contact>()
        {
            new Contact() { id = "semyon", name = "Semyon Guretskiy", server = "localhost:7099", last = "hello", lastdate = new DateTime(2021, 12, 12, 12, 12, 12),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello", created = new DateTime(2021, 12, 12, 12, 12, 12), sent = false
                } } },
            new Contact() { id = "Harry_Potter", name = "Harry Potter", server = "localhost:7099", last = "expeliarmus", lastdate = new DateTime(2021, 12, 12, 12, 12, 13),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "expeliarmus", created = new DateTime(2021, 12, 12, 12, 12, 13), sent = false
                } } },
            new Contact() { id = "Alon88", name = "Alon1997", server = "localhost:7099", last = "hello2", lastdate = new DateTime(2021, 12, 12, 12, 12, 14),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello2", created = new DateTime(2021, 12, 12, 12, 12, 14), sent = true
                } } },
            new Contact() { id = "Borys", name = "Borys", server = "localhost:7099", last = "hello3", lastdate = new DateTime(2021, 12, 12, 12, 12, 15),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello3", created = new DateTime(2021, 12, 12, 12, 12, 15), sent = true
                } } }
        }
        };
        private User _semyon = new User()
        {
            Username = "semyon",
            Nickname = "Semyon Guretskiy",
            Password = "q1",
            contacts = new List<Contact>() {
            new Contact() { id = "Bodek", name = "bodek AP", server = "localhost:7099", last = "hello", lastdate = new DateTime(2021, 12, 12, 12, 12, 12),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello", created = new DateTime(2021, 12, 12, 12, 12, 12), sent = true
                }}} }
        };
        private User _alon = new User()
        {
            Username = "Alon88",
            Nickname = "Alon1997",
            Password = "q1",
            contacts = new List<Contact>(){
        new Contact() { id = "Bodek", name = "Bodek AP", server = "localhost:7099", last = "hello2", lastdate = new DateTime(2021, 12, 12, 12, 12, 14),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello2", created = new DateTime(2021, 12, 12, 12, 12, 14), sent = false
                } } }}
        };
        private User _hp = new User()
        {
            Username = "Harry_Potter",
            Nickname = "Harry Potter",
            Password = "q1",
            contacts = new List<Contact>()
            {
                new Contact() { id = "Bodek", name = "Bodek AP", server = "localhost:7099", last = "expeliarmus", lastdate = new DateTime(2021, 12, 12, 12, 12, 13),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "expeliarmus", created = new DateTime(2021, 12, 12, 12, 12, 13), sent = true
                }}}}
        };
        private User _borys = new User()
        {
            Username = "Borys",
            Nickname = "Borys",
            Password = "q1",
            contacts = new List<Contact>()
            {
                new Contact() { id = "Bodek", name = "Bodek", server = "localhost:7099", last = "hello3", lastdate = new DateTime(2021, 12, 12, 12, 12, 15),
                messages = new List<Message>() { new Message()
                {
                    id = (int)DateTime.Now.Ticks, content = "hello3", created = new DateTime(2021, 12, 12, 12, 12, 15), sent = false
                } } }
            }
        };
        private User _sagi = new User()
        {
            Username = "Sagi",
            Nickname = "Sagi",
            Password = "q1",
            contacts = new List<Contact>()
        };


        private static List<User> _data = new List<User>();

        public ContextData()
        {
            if (_data.Count == 0)
            {
                _data.Add(_bodek);
                _data.Add(_sagi);
                _data.Add(_hp);
                _data.Add(_alon);
                _data.Add(_semyon);
                _data.Add(_borys);
            }
           
        }

        public List<User> Data()
        {
            return _data;
        }
        public ContextData(DbContextOptions<ContextData> options)
            : base(options)
        {
        }

        public bool userValidation(string username, string password) 
        {
            User user = _data.Find(x => x.Username == username);
            if (user == null)
                return false;
            if (password != user.Password)
                return false;
            return true;
        }
        
        public void addUser(User u)
        {
            _data.Add(u);
        }
        public bool isUserExit(string id)
        {
            return _data.Exists(x => x.Username == id);
        }
        public List<Contact> GetContacts(string contactid)
        {
            return _data.Find(person => person.Username == contactid).contacts.ToList();
        }
        public void setContacts(List<Contact> ct, string username)
        {
            _data.Find(person => person.Username == username).contacts = ct;
        }

        public void setMessage(List<Message> m, string user, string contact)
        {
            _data.Find(person => person.Username == user).contacts.
                Find(c => c.id == contact).messages = m;
        }

        public void setLast(string user, string contact, string msg)
        {
            _data.Find(person => person.Username == user).contacts.
                Find(c => c.id == contact).last = msg;
            _data.Find(person => person.Username == user).contacts.
                Find(c => c.id == contact).lastdate = DateTime.Now; 
        }

        public List<Message> getMessages(string user, string contact)
        {
            if (_data.Find(person => person.Username == user).contacts.
                Find(c => c.id == contact) != null)
            {
                return _data.Find(person => person.Username == user).contacts.
                    Find(c => c.id == contact).messages;
            } else
            {
                return null;
            }
        }
    }
}


