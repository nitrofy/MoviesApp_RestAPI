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
    public class ProducerController : ControllerBase
    {
        IProducer producer;
        public ProducerController(IProducer _producer)
        {
            producer = _producer;
        }
        [HttpGet]
        [Route("GetProducersList")]
        public IActionResult GetProducersList()
        {
            try
            {
                var result = producer.GetProducersList();
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpGet]
        [Route("GetProducersListDetail")]
        public IActionResult GetProducersListDetail()
        {
            try
            {
                var result = producer.GetProducersList(detail: true);
                return result != null ? StatusCode((int)HttpStatusCode.OK, result)
                 : StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server unable to process the request.");
            }
        }
        [HttpPost]
        [Route("AddUpdateProducer")]
        public IActionResult AddUpdateProducer(ProducerDetailRequest producerDetail)
        {
            try
            {
                var result = producer.AddUpdateProducer(producerDetail);
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
