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

            var incomeTransactions = await _context.UserTransactions
                .Where(t => t.user_id == userId && t.type == "Income")
                .ToListAsync();

            var outcomeTransactions = await _context.UserTransactions
                .Where(t => t.user_id == userId && t.type == "Outcome")
                .ToListAsync();

            decimal totalIncome = incomeTransactions.Sum(t => t.amount);
            decimal totalExpenses = outcomeTransactions.Sum(t => t.amount);

            ViewBag.TotalIncome = totalIncome;
            ViewBag.TotalExpenses = totalExpenses;

            var outcomeCategoryData = _context.UserTransactions
                .Where(t => t.user_id == userId && t.type == "Outcome")
                .GroupBy(t => new { t.category_id, t.TransactionCategory.category })
                .Select(group => new
                {
                    CategoryName = group.Key.category ?? "Unknown",
                    TotalAmount = group.Sum(t => t.amount)
                })
                .ToList();

            ViewBag.OutcomeCategoryData = outcomeCategoryData;

            var recentTransactions = await _context.UserTransactions
                .Where(t => t.user_id == userId)
                .OrderByDescending(t => t.date)
                .Take(5)
                .ToListAsync();

            foreach (var transaction in recentTransactions)
            {
                if (transaction.category_id != 0)
                {
                    var category = await _context.TransactionCategories.FindAsync(transaction.category_id);
                    transaction.CategoryName = category?.category ?? "Uncategorized";
                }
                else
                {
                    transaction.CategoryName = "Uncategorized";
                }
            }

            return View(recentTransactions);
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(DateTime date, string description, decimal amount, string type, int? category_id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;

            var newTransaction = new UserTransaction
            {
                user_id = userId,
                date = date,
                description = description,
                amount = amount,
                type = type,
                created_at = DateTime.Now,
            };

            if (category_id.HasValue)
            {
                newTransaction.category_id = category_id.Value;
            }

            _context.UserTransactions.Add(newTransaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Transaction added successfully!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditTransaction(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var transaction = await _context.UserTransactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            // Ensure the logged-in user owns the transaction
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            if (transaction.user_id != userId)
            {
                return Unauthorized();
            }

            ViewBag.Categories = await _context.TransactionCategories.ToListAsync();
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> EditTransaction(int id, DateTime date, string description, decimal amount, string type, int? category_id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var transaction = await _context.UserTransactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            if (transaction.user_id != userId)
            {
                return Unauthorized();
            }

            transaction.date = date;
            transaction.description = description;
            transaction.amount = amount;
            transaction.type = type;
            transaction.category_id = category_id ?? 0;

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Transaction updated successfully!";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var transaction = await _context.UserTransactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            int userId = HttpContext.Session.GetInt32("UserId").Value;
            if (transaction.user_id != userId)
            {
                return Unauthorized();
            }

            _context.UserTransactions.Remove(transaction);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Transaction deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}