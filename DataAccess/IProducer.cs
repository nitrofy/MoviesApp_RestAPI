using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public interface IProducer
    {
        public ResponseClass AddUpdateProducer(ProducerDetailRequest ProducerDetail);
        public ResponseClass GetProducersList(bool detail = false);
    }
}
