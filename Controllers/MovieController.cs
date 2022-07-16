using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp_RestAPI.DataAccess;
using MoviesApp_RestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MoviesApp_RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        IMovie movie;
        public MovieController(IMovie _movie)
        {
            movie = _movie;
        }
        [HttpGet]
        [Route("GetMoviesList")]
        public IActionResult GetMoviesList()
        {
            try
            {
                var result = movie.GetMoviesList();
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpGet]
        [Route("GetMoviesListDetail")]
        public IActionResult GetMoviesListDetail()
        {
            try
            {
                var result = movie.GetMoviesList(detail: true);
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpPost]
        [Route("AddUpdateMovie")]
        public IActionResult AddUpdateMovie(MovieDetailRequest movieDetail)
        {
            try
            {
                var result = movie.AddUpdateMovie(movieDetail);
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpPost]
        [Route("AddUpdateMoviePoster")]
        public IActionResult AddUpdateMoviePoster(MoviePosterRequest moviePosterRequest)
        {
            try
            {
                var result = movie.MoviePoster(moviePosterRequest);
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpGet]
        [Route("GetMoviePoster/{movieId}")]
        public IActionResult GetMoviePoster(int movieId)
        {
            try
            {
                var result = movie.MoviePoster(new MoviePosterRequest { ActivityType="R",MovieId=movieId});
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        
    }
}
