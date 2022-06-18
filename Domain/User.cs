using System.ComponentModel.DataAnnotations;

namespace Domain

{
    public class User
    {

        public User()
        { }

        public static object Claims { get; internal set; }

        [Key]
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public string? Image { get; set; }
        public string Token { get; set; }
        public List<Contact> contacts { get; set; }

    }
}
