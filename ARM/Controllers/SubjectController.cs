using ARM.Data;
using ARM.Data.Entities;
using ARM.Models.Subject;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ARM.Controllers
{
    public class SubjectController(AppDbContext context, IMapper mapper) : Controller
    {
        [HttpGet]
        public IActionResult Index(int id)
        {

            var student = context.Students.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            var group = context.Groups.FirstOrDefault(g => g.Id == student.GroupId);

            if (group == null)
            {
                return NotFound("Group not found");
            }

            var subjects = context.Subjects
                .Where(s => s.GroupId == group.Id)
                .ToList();

            var subjectsModel = mapper.Map<List<SubjectViewModel>>(subjects);
            foreach (var subject in subjectsModel)
            {
                var tutor = context.Tutors.FirstOrDefault(t => t.Id == subject.TutorId);
                if (tutor != null)
                {
                    int userId = tutor.UserId;
                    subject.TutorName = context.Users
                        .Where(u => u.Id == userId)
                        .Select(u => $"{u.LastName} {u.FirstName}")
                        .FirstOrDefault() ?? "Unknown Tutor";
                }
            }

            foreach (var subject in subjectsModel)
            {
                subject.Grades = context.Grades
                    .Where(g => g.StudentId == student.Id && g.SubjectId == subject.Id)
                    .Select(g => g.Mark)
                    .ToList();
            }
            return View(subjectsModel);
        }
    }
}
