using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Domain
{
    public class Feedback
    {
        [Required]
        public string Id { get; set; }

        public string Content { get; set; }

        [Range(1, 5, ErrorMessage ="Score must be from 1 to 5")]
        public int Score { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }
    }
}
