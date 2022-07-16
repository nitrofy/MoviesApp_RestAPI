using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.Models
{

    public class ProducerDetail
    {
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public string Bio { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
    }
    public class ProducerDetailRequest
    {
        public string ActivityType { get; set; }
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
        public string Bio { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
        public string Company { get; set; }
    }
    public class ProducerShort
    {
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
    }

}
