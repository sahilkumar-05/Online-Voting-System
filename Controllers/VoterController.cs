using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace ONLINEVS_Project.Controllers
{
    public class VoterController : Controller
    {
        private readonly OnlineVSDBContext _context;

        public VoterController(OnlineVSDBContext context)
        {
            _context = context;
        }
        // GET SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        // ================= SIGN UP =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(Voter voter)
        {
            // CNIC duplicate check
            if (_context.Voters.Any(v => v.votercnic == voter.votercnic))
            {
                ViewBag.Error = "Voter with this CNIC already exists";
                return View(voter);
            }

            // Email duplicate check
            if (_context.Voters.Any(v => v.email == voter.email))
            {
                ViewBag.Error = "Voter with this email already exists";
                return View(voter);
            }

            // Age check
            if (voter.Age < 18)
            {
                ModelState.AddModelError("Age", "You are not eligible to register (Age must be above 18)");
                return View(voter);
            }

            // Model validation
            if (!ModelState.IsValid)
            {
                return View(voter);
            }

            // Save voter
            _context.Voters.Add(voter);
            await _context.SaveChangesAsync();

            // Send email
            await SendEmailAsync(
                voter.email,
                "Voter Registration Successful",
                $"Hello {voter.name},<br><br>Your voter account has been created.<br>CNIC: {voter.votercnic}"
            );

            // Redirect to login page
            return RedirectToAction("Login");
        }

        //GET LOGIN
        public IActionResult Login()
        {
            return View();
        }
        //POST LOGIN
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var voter = _context.Voters
                .FirstOrDefault(v => v.email == email && v.PasswordHash == password);

            if (voter == null)
            {
                ViewBag.Error = "Invalid email or password";
                return View();
            }

            // session set ONLY after login
            HttpContext.Session.SetInt32("VoterId", voter.voterid);
            HttpContext.Session.SetString("VoterName", voter.name);
            HttpContext.Session.SetString("UserRole", "Voter");
            HttpContext.Session.SetString("VoterCNIC", voter.votercnic);

            return RedirectToAction("Dashboard");
        }



        // ================= EMAIL =================
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using MailMessage mail = new MailMessage();
            mail.From = new MailAddress("ar5619866@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(
                "ar5619866@gmail.com",
                "vlow hstl gesj xmyw"
            );
            smtp.EnableSsl = true;

            await smtp.SendMailAsync(mail);
        }

        // ================= DASHBOARD =================
        public IActionResult Dashboard()
        {
            int? voterId = HttpContext.Session.GetInt32("VoterId");
            if (voterId == null)
                return RedirectToAction("Login");


            ViewBag.VoterName = HttpContext.Session.GetString("VoterName");
            ViewBag.VoterId = voterId;

            // Elections + Candidates
            ViewBag.Elections = _context.Elections
                .Include(e => e.Candidates)
                .ToList();

            // Elections where this voter already voted
            var votedElectionIds = _context.Votings
                .Where(v => v.voterid == voterId)
                .Select(v => v.ElectionId)
                .ToList();

            ViewBag.VotedElectionIds = votedElectionIds;

            // Success message after voting
            ViewBag.SuccessMessage = TempData["VoteSuccess"];

            return View();
        }



        // ================= LOGOUT =================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ================= ADMIN: ALL VOTERS =================
        public IActionResult Index()
        {
            var voters = _context.Voters.ToList();
            return View(voters);
        }
    }
}