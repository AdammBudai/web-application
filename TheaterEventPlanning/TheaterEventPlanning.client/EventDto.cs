using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheaterEventPlanning.client
{
    internal class EventDto
    {
       

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
