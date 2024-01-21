using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bug.Models;

namespace Bug.Controllers
{
    [Authorize]
    public class BugsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BugsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bugs
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Bugs.Include(b => b.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BugId == id);
            if (bugs == null)
            {
                return NotFound();
            }

            return View(bugs);
        }

        // GET: Bugs/Create
        public IActionResult Create()
        {
            // Pobierz identyfikator zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Przypisz identyfikator zalogowanego użytkownika do ViewData
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Id == userId), "Id", "Email");

            return View();
        }

        // POST: Bugs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BugId,Description,UserId")] Bugs bugs)
        {
            // Pobierz identyfikator zalogowanego użytkownika
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Przypisz identyfikator zalogowanego użytkownika do właściwości UserId
            bugs.UserId = userId;

            if (!ModelState.IsValid)
            {
                _context.Add(bugs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Przypisz identyfikator zalogowanego użytkownika do ViewData
            ViewData["UserId"] = new SelectList(_context.Users.Where(u => u.Id == userId), "Id", "Email", bugs.UserId);
            return View(bugs);
        }

        // GET: Bugs/Edit/5
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs.FindAsync(id);
            if (bugs == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bugs.UserId);
            return View(bugs);
        }

        // POST: Bugs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BugId,Description,UserId")] Bugs bugs)
        {
            if (id != bugs.BugId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(bugs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugsExists(bugs.BugId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", bugs.UserId);
            return View(bugs);
        }

        // GET: Bugs/Delete/5
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugs = await _context.Bugs
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.BugId == id);
            if (bugs == null)
            {
                return NotFound();
            }

            return View(bugs);
        }

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bugs = await _context.Bugs.FindAsync(id);
            if (bugs != null)
            {
                _context.Bugs.Remove(bugs);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BugsExists(int id)
        {
            return _context.Bugs.Any(e => e.BugId == id);
        }
    }
}

