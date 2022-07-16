using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.Models
{

    public class ActorDetail
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string Bio { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
    }
    public class ActorDetailRequest
    {
        public string ActivityType { get; set; }
        public int ActorId { get; set; }
        public string ActorName { get; set; }
        public string Bio { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
    }
    public class ActorShort
    {
        public int ActorId { get; set; }
        public string ActorName { get; set; }
    }

}
