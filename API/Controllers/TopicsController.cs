using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class TopicsController(ICourseRepository courseRepository) : BaseApiController
    {
        [HttpGet] // GET api/topics

        public async Task<ActionResult<IEnumerable<Topic>>> GetTopics()
        {
            var topics = await courseRepository.GetTopicsAsync();
            return Ok(topics);
        }

    }
}