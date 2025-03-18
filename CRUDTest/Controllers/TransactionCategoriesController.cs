using CRUDTest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ExManaWeb.Controllers
{
    public class TransactionCategoriesController : Controller
    {
        private readonly AppDbContext _context;

        public TransactionCategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: TransactionCategories
        public async Task<IActionResult> Index()
        {
            return View(await _context.TransactionCategories.ToListAsync());
        }

        // GET: TransactionCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionCategory = await _context.TransactionCategories
                .FirstOrDefaultAsync(m => m.id == id);
            if (transactionCategory == null)
            {
                return NotFound();
            }

            return View(transactionCategory);
        }

        // GET: TransactionCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TransactionCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,type,category")] TransactionCategory transactionCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactionCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transactionCategory);
        }

        // GET: TransactionCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionCategory = await _context.TransactionCategories.FindAsync(id);
            if (transactionCategory == null)
            {
                return NotFound();
            }
            return View(transactionCategory);
        }

        // POST: TransactionCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,type,category")] TransactionCategory transactionCategory)
        {
            if (id != transactionCategory.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactionCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionCategoryExists(transactionCategory.id))
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
            return View(transactionCategory);
        }

        // GET: TransactionCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionCategory = await _context.TransactionCategories
                .FirstOrDefaultAsync(m => m.id == id);
            if (transactionCategory == null)
            {
                return NotFound();
            }

            return View(transactionCategory);
        }

        // POST: TransactionCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionCategory = await _context.TransactionCategories.FindAsync(id);
            _context.TransactionCategories.Remove(transactionCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionCategoryExists(int id)
        {
            return _context.TransactionCategories.Any(e => e.id == id);
        }
    }
}
