using ARM.Areas.Tutor.Models.Group;
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
    public class DashboardController(AppDbContext context, IMapper mapper) : Controller
    {
        public IActionResult Index()
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

            var groups = context.Groups
                .Where(g => g.CuratorId == tutorId)
                .ToList();

            var groupModels = mapper.Map<List<GroupInfoViewModel>>(groups);
            return View(groupModels);
        }
    }
}
