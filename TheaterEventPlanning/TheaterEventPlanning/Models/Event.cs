using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheaterEventPlanning.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int EventId { get; set; }

        
        public string name { get; set; }

        
        public DateTime startDate { get; set; }

        
        public DateTime endDate { get; set; }

        
        public string location { get; set; }
        
        public List<CastMember> CastMembers { get; set; }
    }

    
    public class PartialEvent
    {
       
        public int EventId { get; set; }
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string location { get; set; }
    }

    public class ControlEvent
    {
       
        public string name { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string location { get; set; }

        public List<InputCastMember> CastMembers { get; set; }
    }
}
