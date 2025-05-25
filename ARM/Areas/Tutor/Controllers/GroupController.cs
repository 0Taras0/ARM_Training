using ARM.Areas.Tutor.Models.Group;
using ARM.Areas.Tutor.Models.Student;
using ARM.Constants;
using ARM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ARM.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = Roles.Tutor)]
    public class GroupController(AppDbContext context) : Controller
    {
        [HttpGet]
        public IActionResult GetGroup(int subjectId, int groupId)
        {
            var group = context.Groups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
                return NotFound("Групу не знайдено.");

            var students = context.Students
                .Include(s => s.User)
                .Where(s => s.GroupId == group.Id)
                .ToList();

            var grades = context.Grades
                .Where(g => g.SubjectId == subjectId && students.Select(s => s.Id).Contains(g.StudentId))
                .ToList();

            var studentGrades = students.Select(s => new StudentViewModel
            {
                Id = s.Id,
                Name = s.User != null ? $"{s.User.LastName} {s.User.FirstName}" : "Невідомий студент",
                Grades = grades.Where(g => g.StudentId == s.Id).Select(g => g.Mark).ToList(),
                Image = s.User?.Image
            }).ToList();

            var groupModel = new GroupViewModel
            {
                Id = group.Id,
                Name = group.Name,
                Students = studentGrades
            };

            return View(groupModel);
        }
    }
}
