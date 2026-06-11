using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class PlatformsController(ICourseRepository courseRepository) : BaseApiController
    {
        
        [HttpGet] // GET api/platforms
        public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
        {
            var platforms = await courseRepository.GetPlatfdormsAsync();
            return Ok(platforms);
        }
        
    }
}