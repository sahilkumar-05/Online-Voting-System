using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;

namespace ONLINEVS_Project.Controllers
{
    public class CandidateController : Controller
    {
        private readonly OnlineVSDBContext _context;

        public CandidateController(OnlineVSDBContext context)
        {
            _context = context;
        }

        // ======================
        // ROLE CHECK
        // ======================
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // ======================
        // INDEX
        // ======================
        public async Task<IActionResult> Index()
        {
            var candidates = _context.Candidates.Include(c => c.Election);
            return View(await candidates.ToListAsync());
        }

        // ======================
        // DETAILS
        // ======================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var candidate = await _context.Candidates
                .Include(c => c.Election)
                .FirstOrDefaultAsync(m => m.Candidate_ID == id);

            if (candidate == null) return NotFound();

            return View(candidate);
        }

        // ======================
        // CREATE (GET)
        // ======================
        public IActionResult Create()
        {
            if (!IsAdmin()) return Unauthorized();

            ViewData["ElectionId"] =
                new SelectList(_context.Elections, "ElectionId", "Description");

            return View();
        }

        // ======================
        // CREATE (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Candidate candidate)
        {
            if (!IsAdmin()) return Unauthorized();

            if (!ModelState.IsValid)
            {
                ViewData["ElectionId"] =
                    new SelectList(_context.Elections, "ElectionId", "Description", candidate.ElectionId);

                return View(candidate);
            }

            _context.Candidates.Add(candidate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // EDIT (GET)
        // ======================
        public async Task<IActionResult> Edit(int? id)
        {
            if (!IsAdmin()) return Unauthorized();
            if (id == null) return NotFound();

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null) return NotFound();

            ViewData["ElectionId"] =
                new SelectList(_context.Elections, "ElectionId", "Description", candidate.ElectionId);

            return View(candidate);
        }

        // ======================
        // EDIT (POST)
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Candidate candidate)
        {
            if (!IsAdmin()) return Unauthorized();
            if (id != candidate.Candidate_ID) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewData["ElectionId"] =
                    new SelectList(_context.Elections, "ElectionId", "Description", candidate.ElectionId);

                return View(candidate);
            }

            _context.Update(candidate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ======================
        // DELETE (GET)
        // ======================
        public async Task<IActionResult> Delete(int? id)
        {
            if (!IsAdmin()) return Unauthorized();
            if (id == null) return NotFound();

            var candidate = await _context.Candidates
                .Include(c => c.Election)
                .FirstOrDefaultAsync(m => m.Candidate_ID == id);

            if (candidate == null) return NotFound();

            return View(candidate);
        }

        // ======================
        // DELETE (POST)
        // ======================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!IsAdmin()) return Unauthorized();

            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate != null)
            {
                _context.Candidates.Remove(candidate);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
