using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedData(AppDbContext context, UserManager<AppUser> userManager)
        {
            if(await userManager.Users.AnyAsync())
            {
                return;
            }

            //Platforms
            var platforms = new List<Platform>
            {
                new Platform { Name = "Udemy" },
                new Platform { Name = "Coursera" },
                new Platform { Name = "edX" },
                new Platform { Name = "YouTube" },
                new Platform { Name = "LinkedIn Learning" }
            };
            context.Platforms.AddRange(platforms);
            
            //Topics
            var topics = new List<Topic>
            {
                new Topic { Name = "Web Development" },
                new Topic { Name = "Data Science" },
                new Topic { Name = "Machine Learning" },
                new Topic { Name = "Mobile Development" },
                new Topic { Name = "Cloud Computing" },
                new Topic { Name = "Cybersecurity" },
                new Topic { Name = "Game Development" },
                new Topic { Name = "Artificial Intelligence" },
                new Topic { Name = "DevOps" },
                new Topic { Name = "UI/UX Design" }
            };

            context.Topics.AddRange(topics);

            await context.SaveChangesAsync();

             //Users

             var users = new List<AppUser>
             {
                 new AppUser { UserName = "alice", DisplayName = "Alice Johnson", Email = "alice@example.com" },
                 new AppUser { UserName = "bob", DisplayName = "Bob Smith", Email = "bob@example.com" },
                 new AppUser { UserName = "charlie", DisplayName = "Charlie Brown", Email = "charlie@example.com" },
                 new AppUser { UserName = "dave", DisplayName = "Dave Wilson", Email = "dave@example.com" },
                 new AppUser { UserName = "eve", DisplayName = "Eve Davis", Email = "eve@example.com" }
             };

             foreach (var user in users)
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

            // Courses

            var courses = new List<Course>
            {
                new Course
                {
                    Title = "Complete Web Development Bootcamp",
                    LearnPlatformId = platforms.First(p => p.Name == "Udemy").Id,
                    TopicId = topics.First(t => t.Name == "Web Development").Id,
                    isCompleted = true,
                    Rating = 5,
                    Notes = "Started section 2!",
                    username = "alice",
                    Date = DateTime.UtcNow.AddDays(-10)
                },
                new Course
                {
                    Title = "Data Science Specialization",
                    LearnPlatformId = platforms.First(p => p.Name == "Coursera").Id,
                    TopicId = topics.First(t => t.Name == "Data Science").Id,
                    isCompleted = false,
                    Rating = null,
                    Notes = "Need to finish section 1",
                    username = "bob",
                    Date = DateTime.UtcNow.AddDays(-5)
                },
                new Course
                {
                    Title = "Machine Learning A-Z",
                    LearnPlatformId = platforms.First(p => p.Name == "Udemy").Id,
                    TopicId = topics.First(t => t.Name == "Machine Learning").Id,
                    isCompleted = true,
                    Rating = 4,
                    Notes = "Great course, but some sections were outdated.",
                    username = "charlie",
                    Date = DateTime.UtcNow.AddDays(-20)
                },
                new Course
                {
                    Title = "iOS App Development with Swift",
                    LearnPlatformId = platforms.First(p => p.Name == "LinkedIn Learning").Id,
                    TopicId = topics.First(t => t.Name == "Mobile Development").Id,
                    isCompleted = false,
                    Rating = null,
                    Notes = "Just started, looks promising!",
                    username = "dave",
                    Date = DateTime.UtcNow.AddDays(-2)
                },
                new Course
                {
                    Title = "Cloud Computing with AWS",
                    LearnPlatformId = platforms.First(p => p.Name == "edX").Id,
                    TopicId = topics.First(t => t.Name == "Cloud Computing").Id,
                    isCompleted = true,
                    Rating = 5,
                    Notes = "Highly recommend for anyone interested in cloud technologies.",
                    username = "eve",
                    Date = DateTime.UtcNow.AddDays(-15)
                },
                new Course
                {
                    Title = "Cybersecurity Fundamentals",
                    LearnPlatformId = platforms.First(p => p.Name == "YouTube").Id,
                    TopicId = topics.First(t => t.Name == "Cybersecurity").Id,
                    isCompleted = false,
                    Rating = null,
                    Notes = "Found some great free resources on YouTube.",
                    username = "alice",
                    Date = DateTime.UtcNow.AddDays(-7)
                }
            };
            context.Courses.AddRange(courses);
            await context.SaveChangesAsync();
        }
    }
}