using ARM.Areas.Tutor.Models.Subject;
using ARM.Constants;
using ARM.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ARM.Areas.Tutor.Controllers
{
    [Area("Tutor")]
    [Authorize(Roles = Roles.Tutor)]
    public class SubjectController(AppDbContext context, IMapper mapper) : Controller
    {
        [HttpGet]
        public IActionResult GetSubjects(int id)
        {
            int? tutorId = null;

            if (User.IsInRole("Tutor"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                tutorId = context.Tutors
                    .Where(t => t.UserId == userId)
                    .Select(t => t.Id)
                    .FirstOrDefault();
            }

            ViewBag.TutorId = tutorId;

            var subjects = context.Subjects
                .Where(s => s.TutorId == tutorId && s.GroupId == id)
                .ToList();

            var subjectsModel = mapper.Map<List<SubjectInfoViewModel>>(subjects);
            return View(subjectsModel);
        }
    }
}
