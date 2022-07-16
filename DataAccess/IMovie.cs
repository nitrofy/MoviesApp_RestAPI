using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.DataAccess
{
    public interface IMovie
    {
        public ResponseClass AddUpdateMovie(MovieDetailRequest movieDetail);
        public ResponseClass MoviePoster(MoviePosterRequest moviePosterRequest);
        public ResponseClass GetMoviesList(bool detail = false);
    }
}
