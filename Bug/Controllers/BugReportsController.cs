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
    public class BugReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BugReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: BugReports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.BugReports.Include(b => b.Bug);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: BugReports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugReport = await _context.BugReports
                .Include(b => b.Bug)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (bugReport == null)
            {
                return NotFound();
            }

            return View(bugReport);
        }

        // GET: BugReports/Create
        public IActionResult Create()
        {
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId");
            return View();
        }

        // POST: BugReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,Report,BugId")] BugReport bugReport)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(bugReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugReport.BugId);
            return View(bugReport);
        }

        // GET: BugReports/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugReport = await _context.BugReports.FindAsync(id);
            if (bugReport == null)
            {
                return NotFound();
            }
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugReport.BugId);
            return View(bugReport);
        }

        // POST: BugReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReportId,Report,BugId")] BugReport bugReport)
        {
            if (id != bugReport.ReportId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(bugReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugReportExists(bugReport.ReportId))
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
            ViewData["BugId"] = new SelectList(_context.Bugs, "BugId", "BugId", bugReport.BugId);
            return View(bugReport);
        }

        // GET: BugReports/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bugReport = await _context.BugReports
                .Include(b => b.Bug)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (bugReport == null)
            {
                return NotFound();
            }

            return View(bugReport);
        }

        // POST: BugReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bugReport = await _context.BugReports.FindAsync(id);
            if (bugReport != null)
            {
                _context.BugReports.Remove(bugReport);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BugReportExists(int id)
        {
            return _context.BugReports.Any(e => e.ReportId == id);
        }
    }
}
