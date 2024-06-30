using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABKS_project.Models;
using ABKS_project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ABKS_project.Areas.UserEvaluation.Controllers
{
    [Area("UserEvaluation")]
    public class EvaluationController : Controller
    {
        private readonly abksContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EvaluationController(abksContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> FetchGraph()
        {
           
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid UserId.");
            }

            var evaluations = await _context.Evaluations
                .Include(e => e.UserBatch)
                .Where(e => e.UserBatch.UserId == userId)
                .OrderBy(e => e.EvaluationDate)
                .ToListAsync();

            var attendances = await _context.Attendances
                .Include(a => a.UserBatch)
                .Where(a => a.UserBatch.UserId == userId)
                .OrderBy(a => a.AttendanceDate)
                .ToListAsync();

            var weeklyData = new List<WeeklyData>();
            for (int i = 0; i < evaluations.Count; i++)
            {
                var evaluation = evaluations[i];
                var nextEvaluationDate = i + 1 < evaluations.Count ? evaluations[i + 1].EvaluationDate.GetValueOrDefault() : DateTime.MaxValue;

                var weeklyAttendances = attendances
                    .Where(a => a.AttendanceDate.HasValue &&
                                a.AttendanceDate.Value >= evaluation.EvaluationDate.GetValueOrDefault() &&
                                a.AttendanceDate.Value < nextEvaluationDate)
                    .ToList();

                double attendancePercentage = 0;
                if (weeklyAttendances.Any())
                {
                    attendancePercentage = weeklyAttendances
                        .GroupBy(a => a.AttendanceDate.Value)
                        .Select(g => (double)(g.Count(a => a.IsPresent == true) / (double)g.Count() * 100))
                        .Average();
                }

                weeklyData.Add(new WeeklyData
                {
                    EvaluationDate = evaluation.EvaluationDate.GetValueOrDefault(),
                    DisciplineTest = evaluation.DisciplineTest.GetValueOrDefault(),
                    FitnessTest = evaluation.FitnessTest.GetValueOrDefault(),
                    WriteTest = evaluation.WriteTest.GetValueOrDefault(),
                    AttendancePercentage = attendancePercentage
                });
            }

            var labels = weeklyData
                .Select(w => $"Week of {w.EvaluationDate:MMM dd}")
                .ToList();

            var viewModel = new UserEvaluationViewModel
            {
                WeeklyData = weeklyData,
                Labels = labels
            };

            return View(viewModel);
        }
    }
}
