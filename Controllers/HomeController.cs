using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;
using System.Diagnostics;




namespace ONLINEVS_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly OnlineVSDBContext _context;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public HomeController(OnlineVSDBContext context)
        //{
        //    _context = context;
        //}

        public IActionResult Index()
        {
      

            return View();
        }
        public IActionResult AdminLogin()
        {
            return RedirectToAction("Login", "Admins");
        }

        public IActionResult VoterLogin()
        {
            return RedirectToAction("Login", "Voters");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //public IActionResult VotesCastedResults(int electionId)
        //{
        //    ViewBag.ElectionId = electionId;

        //    // 1. Fetch election with candidates
        //    var election = _context.Elections
        //        .Include(e => e.Candidates)
        //        .FirstOrDefault(e => e.ElectionId == electionId);

        //    if (election == null) return NotFound();

        //    // 2. Total votes per candidate
        //    var candidateVotes = _context.Votings
        //        .Where(v => v.ElectionId == electionId)
        //        .GroupBy(v => v.Candidate_ID)
        //        .Select(g => new
        //        {
        //            Candidate_ID = g.Key,
        //            TotalVotes = g.Count()
        //        }).ToList();

        //    // 3. Prepare candidate results
        //    var candidateResults = election.Candidates
        //        .Select(c => new CandidateResultViewModel
        //        {
        //            CandidateId = c.Candidate_ID,
        //            CandidateName = c.Name,
        //            PartyName = c.Party,
        //            TotalVotes = candidateVotes.FirstOrDefault(cv => cv.Candidate_ID == c.Candidate_ID)?.TotalVotes ?? 0
        //        }).ToList();

        //    // 4. Group by party and pick top 2 candidates (winner/runner-up)
        //    var partyResults = candidateResults
        //        .GroupBy(c => c.PartyName)
        //        .Select(g => new PartyResultViewModel
        //        {
        //            PartyName = g.Key,
        //            Candidates = g.OrderByDescending(c => c.TotalVotes).Take(2).ToList(),
        //            PartyTotalVotes = g.Sum(c => c.TotalVotes)
        //        }).ToList();

        //    return View(partyResults); // Voting/VotesCastedResults.cshtml
        //}
        //// ---------------- ViewModels ----------------
        //public class CandidateResultViewModel
        //{
        //    public int CandidateId { get; set; }
        //    public string CandidateName { get; set; } = string.Empty;
        //    public string PartyName { get; set; } = string.Empty;
        //    public int TotalVotes { get; set; }
        //}

        //public class PartyResultViewModel
        //{
        //    public string PartyName { get; set; } = string.Empty;
        //    public List<CandidateResultViewModel> Candidates { get; set; } = new List<CandidateResultViewModel>();
        //    public int PartyTotalVotes { get; set; }
        //}
    }
}
