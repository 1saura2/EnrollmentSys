using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ABKS_project.Models;
using ABKS_project.ViewModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ABKS_project.Controllers
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
            // Retrieve UserId from claims
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            if (!Guid.TryParse(userIdClaim, out var userId))
            {
                // Handle error or redirect if UserId is invalid
                return BadRequest("Invalid UserId.");
            }

            var evaluations = await _context.Evaluations
                .Include(e => e.UserBatch)
                .ThenInclude(ub => ub.User)
                .Where(e => e.UserBatch.UserId == userId)
                .OrderBy(e => e.EvaluationDate)
                .ToListAsync();

            var attendances = await _context.Attendances
                .Include(a => a.UserBatch)
                .ThenInclude(ub => ub.User)
                .Where(a => a.UserBatch.UserId == userId)
                .OrderBy(a => a.AttendanceDate)
                .ToListAsync();

            // Group evaluations by week
            var weeklyEvaluationData = evaluations
                .GroupBy(e => ISOWeek.GetWeekOfYear(e.EvaluationDate.GetValueOrDefault()))
                .Select(g => new WeeklyEvaluationData
                {
                    Week = g.Key,
                    StartDate = g.Min(e => e.EvaluationDate).GetValueOrDefault(),
                    EndDate = g.Max(e => e.EvaluationDate).GetValueOrDefault(),
                    AverageDisciplineTest = (decimal)g.Average(e => e.DisciplineTest),
                    AverageFitnessTest = (decimal)g.Average(e => e.FitnessTest),
                    AverageWriteTest = (decimal)g.Average(e => e.WriteTest)
                })
                .ToList();

            // Generate labels for the chart
            var labels = weeklyEvaluationData
                .Select(w => $"Week {w.Week} ({w.StartDate:MMM dd} - {w.EndDate:MMM dd})")
                .ToList();

            // Group attendances by evaluation weeks
            var weeklyAttendanceData = weeklyEvaluationData.Select(week => new WeeklyAttendanceData
            {
                Week = week.Week,
                AttendancePercentage = attendances
                    .Where(a => a.AttendanceDate.HasValue &&
                                a.AttendanceDate.Value >= week.StartDate &&
                                a.AttendanceDate.Value <= week.EndDate)
                    .GroupBy(a => a.AttendanceDate.Value)
                    .Select(g => (double)g.Count(a => a.IsPresent == true) / g.Count() * 100)
                    .Average()
            }).ToList();

            var viewModel = new UserEvaluationViewModel
            {
                WeeklyEvaluations = weeklyEvaluationData,
                WeeklyAttendances = weeklyAttendanceData,
                Labels = labels
            };

            return View(viewModel);
        }
    }
}
