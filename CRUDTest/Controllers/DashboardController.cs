using CRUDTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExManaWeb.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

            // Example: Get total income and expenses for the user
            var totalIncome = await _context.UserTransactions
                .Where(t => t.user_id == userId && t.type == "Income") // Corrected user_id, type
                .SumAsync(t => t.amount);

            var totalExpenses = await _context.UserTransactions
                .Where(t => t.user_id == userId && t.type == "Expense") // Corrected user_id, type
                .SumAsync(t => t.amount);

            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpenses = totalExpenses;

            // Example: Get recent transactions
            var recentTransactions = await _context.UserTransactions
                .Where(t => t.user_id == userId) // Corrected user_id
                .OrderByDescending(t => t.date)
                .Take(5)
                .ToListAsync();

            return View(recentTransactions);
        }
    }
}