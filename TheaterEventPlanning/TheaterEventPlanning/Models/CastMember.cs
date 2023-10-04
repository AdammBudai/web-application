using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheaterEventPlanning.Models
{
    public class CastMember
    {

        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CastMemberId { get; set; }
        public string actorName { get; set; }

        public string role { get; set; }
    }

    public class InputCastMember
    {
        public string actorName { get; set; }
        public string role { get; set; }
    }
}
