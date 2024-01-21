using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bug.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bug.Controllers
{
    [Authorize]
    public class BugFixesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BugFixesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BugFixes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BugFixes.Include(b => b.Bug);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BugFixes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugFix = await _context.BugFixes
                .Include(b => b.Bug)
                .FirstOrDefaultAsync(m => m.FixId == id);
            if (bugFix == null)
            {
                return NotFound();
            }

            return View(bugFix);
        }

        // GET: BugFixes/Create
        public IActionResult Create()
        {
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId");
            return View();
        }

        // POST: BugFixes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FixId,FixDescription,BugId")] BugFix bugFix)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(bugFix);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugFix.BugId);
            return View(bugFix);
        }

        // GET: BugFixes/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugFix = await _context.BugFixes.FindAsync(id);
            if (bugFix == null)
            {
                return NotFound();
            }
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugFix.BugId);
            return View(bugFix);
        }

        // POST: BugFixes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FixId,FixDescription,BugId")] BugFix bugFix)
        {
            if (id != bugFix.FixId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(bugFix);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugFixExists(bugFix.FixId))
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
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugFix.BugId);
            return View(bugFix);
        }

        // GET: BugFixes/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugFix = await _context.BugFixes
                .Include(b => b.Bug)
                .FirstOrDefaultAsync(m => m.FixId == id);
            if (bugFix == null)
            {
                return NotFound();
            }

            return View(bugFix);
        }

        // POST: BugFixes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bugFix = await _context.BugFixes.FindAsync(id);
            if (bugFix != null)
            {
                _context.BugFixes.Remove(bugFix);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BugFixExists(int id)
        {
            return _context.BugFixes.Any(e => e.FixId == id);
        }
    }
}
