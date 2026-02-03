using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ONLINEVS_Project.Controllers
{
    public class AdminController : Controller
    {
        private readonly OnlineVSDBContext _context;

        public AdminController(OnlineVSDBContext context)
        {
            _context = context;
        }

        // GET: Admin Index
        public async Task<IActionResult> Index()
        {
            return View(await _context.Admin.ToListAsync());
        }

        // GET: Login Page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login (Email + Password + Optional CNIC)
        [HttpPost]
        public IActionResult Login(string email, string password, string cnic)
        {
            var admin = _context.Admin
                .FirstOrDefault(a =>
                    a.email == email &&
                    a.PasswordHash == password &&
                    (cnic == null || a.AdminId == cnic)
                );

            if (admin != null)
            {
                HttpContext.Session.SetString("UserRole", "Admin");
                HttpContext.Session.SetString("AdminId", admin.AdminId);

                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid Email, CNIC, or Password";
            return View();
        }

        // GET: Dashboard (Only for Admin)
        public IActionResult Dashboard()
        {
            // 1. Check if the user is logged in
            var adminId = HttpContext.Session.GetString("AdminId");
            if (string.IsNullOrEmpty(adminId))
            {
                return RedirectToAction("Login");
            }

            // 2. Fetch the full Admin object from the database using the ID
            var admin = _context.Admin.FirstOrDefault(a => a.AdminId == adminId);

            // 3. Pass the name and ID to the ViewBag
            if (admin != null)
            {
                ViewBag.AdminName = admin.name; // Make sure 'name' matches your Model property
            }
            ViewBag.AdminId = adminId;

            
            ViewBag.Voters = _context.Voters.ToList();
            ViewBag.Elections = _context.Elections.ToList();
            ViewBag.Candidates = _context.Candidates.Include(c => c.Election).ToList();
            ViewBag.Votings = _context.Votings.ToList();

            return View();
        }

        // GET: Admin Details
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.AdminId == id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // GET: Create Admin
        public IActionResult Create()
        {
            return View();
        }

        // POST: Create Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminId,name,email,PasswordHash")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(admin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Edit Admin
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin.FindAsync(id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Edit Admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AdminId,name,email,PasswordHash")] Admin admin)
        {
            if (id != admin.AdminId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Admin.Any(e => e.AdminId == admin.AdminId))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(admin);
        }

        // GET: Delete Admin
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var admin = await _context.Admin.FirstOrDefaultAsync(a => a.AdminId == id);
            if (admin == null) return NotFound();

            return View(admin);
        }

        // POST: Delete Admin Confirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var admin = await _context.Admin.FindAsync(id);
            if (admin != null)
            {
                _context.Admin.Remove(admin);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/CreateElection
        public IActionResult CreateElection()
        {
            return View("~/Views/Election/Create.cshtml");
        }
 
        // POST: Admin/CreateElection
        [HttpPost]
        public async Task<IActionResult> CreateElection(Election election)
        {
            if (ModelState.IsValid)
            {
                _context.Elections.Add(election); // Save to Elections table
                await _context.SaveChangesAsync();
                return RedirectToAction("Dashboard");
            }
            return View("~/Views/Election/Create.cshtml");
        }

        public IActionResult Logout()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Agar authentication use ho rahi hai (optional)
            if (User.Identity.IsAuthenticated)
            {
                // _signInManager.SignOutAsync().Wait(); // agar Identity use ho
            }

            // Redirect directly starting page
            return Redirect("~/");  // guaranteed root page
        }

        public IActionResult ViewVotesCasted(int electionId)
        {
            // Redirect to VotingController's admin results action
            return RedirectToAction("VotesCastedResults", "Voting", new { electionId = electionId });
        }


    }
}
