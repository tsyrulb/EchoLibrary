using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int ContactID { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string server { get; set; }
        public string? last { get; set; }
        public DateTime? lastdate { get; set; }
        [JsonIgnore]
        public List<Message>? messages { get; set; }
    }
}
