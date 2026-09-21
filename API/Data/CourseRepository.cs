using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class CourseRepository(AppDbContext context) : ICourseRepository
    {
        public void AddCourse(Course course)
        {
            context.Courses.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            context.Courses.Remove(course);
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            return await context.Courses
                .Include(c => c.LearnPlatform)
                .Include(c => c.Topic)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IReadOnlyList<CourseDto>> GetCoursesAsync(string username)
        {
            return await context.Courses
                .Where(c => c.username == username)
                .OrderByDescending(c => c.Date)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Date = c.Date,
                    Title = c.Title,
                    PlatformId = c.LearnPlatformId,
                    PlatformName = c.LearnPlatform.Name,
                    TopicId = c.TopicId,
                    TopicName = c.Topic.Name,
                    IsCompleted = c.isCompleted,
                    Rating = c.Rating,
                    Notes = c.Notes
                }).ToListAsync();
        }

        public async Task<IReadOnlyList<Platform>> GetPlatfdormsAsync()
        {
            return await context.Platforms.ToListAsync();
        }

        public async Task<IReadOnlyList<CourseDto>> GetRecentCoursesAsync(string username)
        {
            var ThirtyDaysAgo = DateTime.UtcNow.AddDays(-30); //Strict Postgress sql fix

            return await context.Courses
                .Where(c => c.username == username && c.Date >= ThirtyDaysAgo)
                .OrderByDescending(c => c.Date)
                .Select(c => new CourseDto
                {
                    Id = c.Id,
                    Date = c.Date,
                    Title = c.Title,
                    PlatformId = c.LearnPlatformId,
                    PlatformName = c.LearnPlatform.Name,
                    TopicId = c.TopicId,
                    TopicName = c.Topic.Name,
                    IsCompleted = c.isCompleted,
                    Rating = c.Rating,
                    Notes = c.Notes
                }).ToListAsync();
        }

        public async Task<IReadOnlyList<Topic>> GetTopicsAsync()
        {
            return await context.Topics.ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await context.SaveChangesAsync() > 0;
        }
    }
}