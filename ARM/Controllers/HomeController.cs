using ARM.Data;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ARM.Controllers
{
    public class HomeController(AppDbContext context) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            int? studentId = null;

            if (User.IsInRole("Student"))
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                studentId = context.Students
                    .Where(s => s.UserId == userId)
                    .Select(s => s.Id)
                    .FirstOrDefault();
            }

            ViewBag.StudentId = studentId;

            return View();
        }
    }
}
