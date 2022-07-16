using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public interface IActor
    {
        public ResponseClass AddUpdateActor(ActorDetailRequest actorDetail);
        public ResponseClass GetActorsList(bool detail = false);
    }
}
