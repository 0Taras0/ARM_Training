using ARM.Constants;
using ARM.Data.Entities;
using ARM.Data.Entities.Identity;
using ARM.Interfaces;
using ARM.Models.Seeder;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ARM.Data
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication webApplication)
        {
            using var scope = webApplication.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();

            var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

            context.Database.Migrate();

            await SeedRoles(context, mapper, scope);

            await SeedUsers(context, mapper, userManager, imageService);

            await SeedTutors(context, imageService);

            await SeedGroups(context, imageService);

            await SeedStudents(context, imageService);

            await SeedSubjects(context, imageService);
        }
        private async static Task SeedRoles(AppDbContext context, IMapper mapper, IServiceScope scope)
        {
            if (!context.Roles.Any())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
                var tutor = new RoleEntity { Name = Roles.Tutor };
                var result = await roleManager.CreateAsync(tutor);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.Tutor} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }

                var student = new RoleEntity { Name = Roles.Student };
                result = await roleManager.CreateAsync(student);
                if (result.Succeeded)
                {
                    Console.WriteLine($"Роль {Roles.Student} створено успішно");
                }
                else
                {
                    Console.WriteLine($"Помилка створення ролі:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"- {error.Code}: {error.Description}");
                    }
                }
            }
        }
        private static async Task SeedSubjects(AppDbContext context, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Subjects.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var subjects = JsonSerializer.Deserialize<List<SeederSubjectModel>>(jsonData);
                    foreach (var model in subjects)
                    {
                        var curator = await context.Tutors
                            .FirstOrDefaultAsync(u => u.User.UserName == model.CuratorUserName);

                        if (curator == null)
                        {
                            Console.WriteLine($"Curator '{model.CuratorUserName}' not found for group '{model.Name}'");
                            continue;
                        }

                        var group = await context.Groups
                            .FirstOrDefaultAsync(g => g.Name == model.GroupName);
                        if (group == null)
                        {
                            Console.WriteLine($"Group '{model.GroupName}' not found for subject '{model.Name}'");
                            continue;
                        }

                        var subjectEntity = new SubjectEntity
                        {
                            Name = model.Name,
                            GroupId = group.Id,
                            TutorId = curator.Id
                        };

                        await context.Subjects.AddAsync(subjectEntity);
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }

        private static async Task SeedTutors(AppDbContext context, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Tutors.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var tutors = JsonSerializer.Deserialize<List<SeederTutorModel>>(jsonData);
                    foreach (var model in tutors)
                    {
                        var user = await context.Users
                            .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                        if (user == null)
                        {
                            Console.WriteLine($"User '{model.UserName}'");
                            continue;
                        }

                        var tutorEntity = new TutorEntity
                        {
                            UserId = user.Id
                        };

                        await context.Tutors.AddAsync(tutorEntity);
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }

        private static async Task SeedStudents(AppDbContext context, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Students.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var students = JsonSerializer.Deserialize<List<SeederStudentModel>>(jsonData);
                    foreach (var model in students)
                    {
                        var user = await context.Users
                            .FirstOrDefaultAsync(u => u.UserName == model.UserName);

                        if (user == null)
                        {
                            Console.WriteLine($"User '{model.UserName}'");
                            continue;
                        }

                        var group = await context.Groups
                            .FirstOrDefaultAsync(g => g.Name == model.GroupName);

                        if (group == null)
                        {
                            Console.WriteLine($"Group '{model.GroupName}' not found for user '{model.UserName}'");
                            continue;
                        }

                        var studentEntity = new StudentEntity
                        {
                            UserId = user.Id,
                            GroupId = group.Id
                        };

                        await context.Students.AddAsync(studentEntity);
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }

        private async static Task<IFormFile> LoadImageAsFormFileAsync(string imagePath, string imageName)
        {
            var fileInfo = new FileInfo(imagePath);

            if (!File.Exists(imagePath))
            {
                return null;
            }

            var memoryStream = new MemoryStream(await File.ReadAllBytesAsync(imagePath));

            return new FormFile(memoryStream, 0, memoryStream.Length, "ImageFile", imageName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/" + fileInfo.Extension.Trim('.')
            };
        }
        private static async Task SeedGroups(AppDbContext context, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Groups.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var groups = JsonSerializer.Deserialize<List<SeederGroupModel>>(jsonData);
                    foreach (var model in groups)
                    {
                        var curator = await context.Tutors
                            .FirstOrDefaultAsync(u => u.User.UserName == model.CuratorUserName);

                        if (curator == null)
                        {
                            Console.WriteLine($"Curator '{model.CuratorUserName}' not found for group '{model.Name}'");
                            continue;
                        }

                        var groupEntity = new GroupEntity
                        {
                            Name = model.Name,
                            CuratorId = curator.Id
                        };

                        await context.Groups.AddAsync(groupEntity);
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }

        private static async Task SeedUsers(AppDbContext context, IMapper mapper, UserManager<UserEntity> userManager, IImageService imageService)
        {
            var jsonFile = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "JsonData", "Users.json");
            if (File.Exists(jsonFile))
            {
                var jsonData = await File.ReadAllTextAsync(jsonFile);
                try
                {
                    var users = JsonSerializer.Deserialize<List<SeederUserModel>>(jsonData);
                    foreach (var model in users)
                    {
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Helpers", "SeedImages", "Users", model.Image);
                        var formFile = await LoadImageAsFormFileAsync(imagePath, model.Image);

                        if (formFile == null)
                        {
                            Console.WriteLine($"Image file not found: {model.Image}");
                            continue;
                        }

                        var entity = mapper.Map<UserEntity>(model);
                        entity.Image = await imageService.SaveImageAsync(formFile);
                        var result = await userManager.CreateAsync(entity, model.Password);

                        if (result.Succeeded)
                        {
                            Console.WriteLine($"Користувача успішно створено {entity.LastName} {entity.FirstName}!");
                            await userManager.AddToRoleAsync(entity, model.Role);
                        }
                        else
                        {
                            Console.WriteLine($"Помилка створення користувача:");
                            foreach (var error in result.Errors)
                            {
                                Console.WriteLine($"- {error.Code}: {error.Description}");
                            }
                        }
                    }
                    await context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Json Parse Data {0}", ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Not Found File Users.json");
            }
        }
    }
}
