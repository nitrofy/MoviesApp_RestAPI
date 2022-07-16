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
    public class ActorController : ControllerBase
    {
        IActor actor;
        public ActorController(IActor _actor)
        {
            actor = _actor;
        }
        [HttpGet]
        [Route("GetActorsList")]
        public IActionResult GetActorsList()
        {
            try
            {
                var result = actor.GetActorsList();
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpGet]
        [Route("GetActorsListDetail")]
        public IActionResult GetActorsListDetail()
        {
            try
            {
                var result = actor.GetActorsList(detail: true);
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpPost]
        [Route("AddUpdateActor")]
        public IActionResult AddUpdateActor(ActorDetailRequest actorDetail)
        {
            try
            {
                var result = actor.AddUpdateActor(actorDetail);
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
