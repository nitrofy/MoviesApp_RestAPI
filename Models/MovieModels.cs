using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.Models
{

    public class MovieDetailRequest
    {
        public string ActivityType { get; set; }
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int ProducerId { get; set; }
        public int[] ActorIds { get; set; }
        public string Plot { get; set; }
        public DateTime DateOfRelease { get; set; }
    }
    
    public class MoviesDetailResponse
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
        public int[] ActorIds { get; set; }
        public string Plot { get; set; }
        public DateTime DateOfRelease { get; set; }
        public List<ActorShort> ActorsList { get; set; }
        public int ProducerId { get; set; }
        public string ProducerName { get; set; }
    }
    public class MovieShort
    {
        public int MovieId { get; set; }
        public string MovieName { get; set; }
    }
    public class MovieActorRelation
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string ActorName { get; set; }

    }
    public class MoviePosterRequest
    {
        public string ActivityType { get; set; }
        public int MovieId { get; set; }
        public string MoviePoster { get; set; } // Base64 encoded string converted from ImageBytes
    }
    public class MoviePosterResponse
    {
        public int MovieId { get; set; }
        public string Poster { get; set; } // Base64 encoded string converted from ImageBytes
    }
}
