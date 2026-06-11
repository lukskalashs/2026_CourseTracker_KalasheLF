using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class CoursesController(ICourseRepository courseRepository) : BaseApiController
    {
        [HttpGet] // GET api/courses
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
        {
            var courses = await courseRepository.GetCoursesAsync(User.GetUsername());
            return Ok(courses);
        }

        [HttpGet("pastmonth")] // GET api/courses/pastmonth
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetRecentCourses()
        {
            var courses = await courseRepository.GetRecentCoursesAsync(User.GetUsername());
            return Ok(courses);
        }

        [HttpGet("{id}")] // GET api/courses/{id}
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = await courseRepository.GetCourseByIdAsync(id);

            if(course == null) return NotFound();
            if(course.username != User.GetUsername() && course.username != null) return Forbid(); // Access is forbidden if the course belongs to another user

            return new CourseDto
            {
                Id = course.Id,
                Date = course.Date,
                Title = course.Title,
                PlatformId = course.LearnPlatformId,
                PlatformName = course.LearnPlatform.Name,
                TopicId = course.TopicId,
                TopicName = course.Topic.Name,
                IsCompleted = course.isCompleted,
                Rating = course.Rating,
                Notes = course.Notes
            };
        }

        [HttpPost] // POST api/courses
        public async Task<ActionResult<CourseDto>> AddCourse(CourseCreateDto courseCreateDto)
        {
            var course = new Course
            {
                Title = courseCreateDto.Title,
                LearnPlatformId = courseCreateDto.PlatformId,
                TopicId = courseCreateDto.TopicId,
                isCompleted = courseCreateDto.IsCompleted,
                Rating = courseCreateDto.Rating,
                Notes = courseCreateDto.Notes,
                Date = DateTime.UtcNow,
                username = User.GetUsername()
                
            };

            courseRepository.AddCourse(course);

            if(await courseRepository.SaveAllAsync())
            {
                // Fetch the course with its related entities to return a complete CourseDto { Topic & Platform names }
                var createdCourse = await courseRepository.GetCourseByIdAsync(course.Id);
                
                var courseDto = new CourseDto
                {
                    Id = createdCourse!.Id,
                    Date = createdCourse.Date,
                    Title = createdCourse.Title,
                    PlatformId = createdCourse.LearnPlatformId,
                    PlatformName = createdCourse.LearnPlatform.Name,
                    TopicId = createdCourse.TopicId,
                    TopicName = createdCourse.Topic.Name,
                    IsCompleted = createdCourse.isCompleted,
                    Rating = createdCourse.Rating,
                    Notes = createdCourse.Notes
                };

                return CreatedAtAction(nameof(GetCourse), new { id = course.Id }, courseDto);
            }

            return BadRequest("Failed to add course");
                
        }

        [HttpPut("{id}")] // PUT api/courses/{id}
        public async Task<ActionResult> UpdateCourse(int id, CourseUpdateDto courseUpdateDto)
        {
            var course = await courseRepository.GetCourseByIdAsync(id);

            if(course == null) return NotFound();
            if(course.username != User.GetUsername()) return Forbid(); // Access is if the course belongs to another user

            course.Title = courseUpdateDto.Title;
            course.LearnPlatformId = courseUpdateDto.PlatformId;
            course.TopicId = courseUpdateDto.TopicId;
            course.isCompleted = courseUpdateDto.IsCompleted;
            course.Rating = courseUpdateDto.Rating;
            course.Notes = courseUpdateDto.Notes;

            if(await courseRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update course");
        }

        [HttpPatch("{id}")] // PATCH api/courses/{id}
        public async Task<ActionResult> MarkedAsCompleted(int id)
            {
                var course = await courseRepository.GetCourseByIdAsync(id);

                if(course == null) return NotFound();
                if(course.username != User.GetUsername()) return Forbid(); // Access is forbidden if the course belong to anothere existing user

                course.isCompleted = true;
                course.Date = DateTime.UtcNow; // Update the date to reflect when the course was completed

                if(await courseRepository.SaveAllAsync()) return NoContent();

                return BadRequest("Failed to update course - marked as completed");
            }
            
        
        [HttpDelete("{id}")] // DELETE api/courses/{id}
        public async Task<ActionResult> DeleteCourse(int id)
        {
            var course = await courseRepository.GetCourseByIdAsync(id);

            if(course == null) return NotFound();
            if(course.username != User.GetUsername()) return Forbid(); 

            courseRepository.DeleteCourse(course);

            if(await courseRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to delete course"); 
        }
    }
}