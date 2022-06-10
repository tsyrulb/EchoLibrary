using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id { get; set; }
        public string content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime created { get; set; }
        public Boolean sent { get; set; }
    }
}
