using CRUDTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ExManaWeb.Controllers
{
    public class UserTransactionsController : Controller
    {
        private readonly AppDbContext _context;

        public UserTransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserTransactions
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserTransactions.Include(u => u.user_id);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserTransactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions
                .Include(u => u.user_id)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userTransaction == null)
            {
                return NotFound();
            }

            return View(userTransaction);
        }

        // GET: UserTransactions/Create
        public IActionResult Create()
        {
            ViewData["user_id"] = new SelectList(_context.Users, "id", "username");
            return View();
        }

        // POST: UserTransactions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,user_id,date,amount,type,description,category_id,created_at")] UserTransaction userTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["user_id"] = new SelectList(_context.Users, "id", "username", userTransaction.user_id);
            return View(userTransaction);
        }

        // GET: UserTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions.FindAsync(id);
            if (userTransaction == null)
            {
                return NotFound();
            }
            ViewData["user_id"] = new SelectList(_context.Users, "id", "username", userTransaction.user_id);
            return View(userTransaction);
        }

        // POST: UserTransactions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,user_id,date,amount,type,description,category_id,created_at")] UserTransaction userTransaction)
        {
            if (id != userTransaction.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTransactionExists(userTransaction.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["user_id"] = new SelectList(_context.Users, "id", "username", userTransaction.user_id);
            return View(userTransaction);
        }

        // GET: UserTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions
                .Include(u => u.user_id)
                .FirstOrDefaultAsync(m => m.id == id);
            if (userTransaction == null)
            {
                return NotFound();
            }

            return View(userTransaction);
        }

        // POST: UserTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTransaction = await _context.UserTransactions.FindAsync(id);
            _context.UserTransactions.Remove(userTransaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTransactionExists(int id)
        {
            return _context.UserTransactions.Any(e => e.id == id);
        }
    }
}
