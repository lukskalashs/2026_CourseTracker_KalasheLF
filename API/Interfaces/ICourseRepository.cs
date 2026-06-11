using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface ICourseRepository
    {
        void AddCourse(Course course);
        void DeleteCourse(Course course);
        Task<Course?> GetCourseByIdAsync(int id);
        Task<IReadOnlyList<CourseDto>> GetCoursesAsync(string username);
        Task<IReadOnlyList<CourseDto>> GetRecentCoursesAsync(string username);
        Task<IReadOnlyList<Platform>> GetPlatfdormsAsync();
        Task<IReadOnlyList<Topic>> GetTopicsAsync();
         Task<bool> SaveAllAsync();
    }
}