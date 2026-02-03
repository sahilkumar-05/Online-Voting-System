using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ONLINEVS_Project.Models;
using QuestPDF.Helpers;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;


using System.Collections.Generic;
using System.Linq;

namespace ONLINEVS_Project.Controllers
{
    public class VotingController : Controller
    {
        private readonly OnlineVSDBContext _context;

        public VotingController(OnlineVSDBContext context)
        {
            _context = context;
        }

        // ================= ADMIN: VIEW ALL VOTES =================
        public IActionResult Index()
        {
            var votes = _context.Votings
                .Include(v => v.Voter)
                .Include(v => v.Election)
                .Include(v => v.Candidate)
                .OrderByDescending(v => v.CastAt)
                .ToList();

            return View(votes);
        }

        // ================= CAST VOTE =================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Vote(int electionId, int candidateId)
        {
            int? voterId = HttpContext.Session.GetInt32("VoterId");
            if (voterId == null)
                return RedirectToAction("SignUp", "Voter");

            // Check if already voted in this election
            bool alreadyVoted = _context.Votings
     .Any(v => v.voterid == voterId && v.ElectionId == electionId);


            if (alreadyVoted)
            {
                TempData["Error"] = "You have already voted in this election.";
                return RedirectToAction("Dashboard", "Voter");
            }

            // Validate candidate
            var candidate = _context.Candidates
                .FirstOrDefault(c => c.Candidate_ID == candidateId && c.ElectionId == electionId);

            if (candidate == null)
            {
                TempData["Error"] = "Invalid candidate selection.";
                return RedirectToAction("Dashboard", "Voter");
            }

            // Save vote
            Voting vote = new Voting
            {
                voterid = voterId.Value,
                ElectionId = electionId,
                Candidate_ID = candidateId,
                CastAt = DateTime.Now
            };

            _context.Votings.Add(vote);
            _context.SaveChanges();

            TempData["VoteSuccess"] =
                $"Your vote has been successfully cast for {candidate.Name} ({candidate.Party}).";

            return RedirectToAction("Dashboard", "Voter");
        }

        // =================== ADMIN: Votes Casted Results (All Elections) ===================
        public IActionResult VotesCastedResults(bool downloadPdf = false)
        {
            // 1. Fetch all elections with candidates
            var elections = _context.Elections
                .Include(e => e.Candidates)
                .ToList();

            var allPartyResults = new List<PartyResultViewModel>();

            // Group elections by ElectionDescription (seat)
            var electionsBySeat = elections.GroupBy(e => e.Description);

            foreach (var seatGroup in electionsBySeat)
            {
                // Combine all candidates of this seat
                var seatCandidates = seatGroup.SelectMany(e => e.Candidates)
                    .Select(c => new CandidateResultViewModel
                    {
                        CandidateId = c.Candidate_ID,
                        CandidateName = c.Name,
                        PartyName = c.Party,
                        TotalVotes = _context.Votings
                            .Count(v => v.Candidate_ID == c.Candidate_ID && v.ElectionId == c.ElectionId),
                        ElectionTitle = c.Election.Title,
                        ElectionDescription = c.Election.Description
                    }).ToList();

                // Group by party (sum votes for same party in same seat)
                var partyResults = seatCandidates
                    .GroupBy(c => c.PartyName)
                    .Select(g => new PartyResultViewModel
                    {
                        PartyName = g.Key,
                        Candidates = g.OrderByDescending(c => c.TotalVotes).ToList(),
                        PartyTotalVotes = g.Sum(c => c.TotalVotes),
                        ElectionTitle = g.First().ElectionTitle,
                        ElectionDescription = g.First().ElectionDescription,
                        IsElectionEnded = seatGroup.First().EndDate < DateTime.Now,
                        ElectionEndDate = seatGroup.First().EndDate
                    })
                    .ToList();

                // Determine winner/loser (seat-wise)
                bool electionEnded = partyResults.First().IsElectionEnded;
                if (electionEnded && partyResults.Any())
                {
                    int maxVotes = partyResults.Max(p => p.PartyTotalVotes);
                    foreach (var party in partyResults)
                    {
                        party.IsWinner = party.PartyTotalVotes == maxVotes;
                        party.IsLoser = party.PartyTotalVotes != maxVotes;
                    }
                }

                allPartyResults.AddRange(partyResults);
            }
            // ================= PDF ONLY =================
            if (downloadPdf)
            {
                var document = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);
                        page.Size(PageSizes.A4);

                        page.Header().Text("Votes Casted Results").SemiBold().FontSize(20);

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(h =>
                            {
                                h.Cell().Text("S.No").SemiBold();
                                h.Cell().Text("Party");
                                h.Cell().Text("Election");
                                h.Cell().Text("Seat");
                                h.Cell().Text("Candidate");
                                h.Cell().Text("Votes");
                            });

                            int i = 1;
                            foreach (var party in allPartyResults)
                            {
                                foreach (var c in party.Candidates)
                                {
                                    table.Cell().Text(i++);
                                    table.Cell().Text(party.PartyName);
                                    table.Cell().Text(c.ElectionTitle);
                                    table.Cell().Text(c.ElectionDescription);
                                    table.Cell().Text(c.CandidateName);
                                    table.Cell().Text(c.TotalVotes);
                                }
                            }
                        });
                    });
                });

                return File(document.GeneratePdf(),
                    "application/pdf",
                    "VoteResults.pdf");
            }

            // ================= VIEW =================
            return View(allPartyResults);
        }

        //// =================== PDF Download ===================
        //public IActionResult DownloadVoteResultsPdf()
        //{
        //    // 1. Fetch elections and results (same as VotesCastedResults)
        //    var elections = _context.Elections
        //        .Include(e => e.Candidates)
        //        .ToList();

        //    var allPartyResults = new List<PartyResultViewModel>();

        //    var electionsBySeat = elections.GroupBy(e => e.Description);

        //    foreach (var seatGroup in electionsBySeat)
        //    {
        //        var seatCandidates = seatGroup.SelectMany(e => e.Candidates)
        //            .Select(c => new CandidateResultViewModel
        //            {
        //                CandidateId = c.Candidate_ID,
        //                CandidateName = c.Name,
        //                PartyName = c.Party,
        //                TotalVotes = _context.Votings
        //                    .Count(v => v.Candidate_ID == c.Candidate_ID && v.ElectionId == c.ElectionId),
        //                ElectionTitle = c.Election.Title,
        //                ElectionDescription = c.Election.Description
        //            }).ToList();

        //        var partyResults = seatCandidates
        //            .GroupBy(c => c.PartyName)
        //            .Select(g => new PartyResultViewModel
        //            {
        //                PartyName = g.Key,
        //                Candidates = g.OrderByDescending(c => c.TotalVotes).ToList(),
        //                PartyTotalVotes = g.Sum(c => c.TotalVotes),
        //                ElectionTitle = g.First().ElectionTitle,
        //                ElectionDescription = g.First().ElectionDescription,
        //                IsElectionEnded = seatGroup.First().EndDate < DateTime.Now,
        //                ElectionEndDate = seatGroup.First().EndDate
        //            }).ToList();

        //        if (partyResults.First().IsElectionEnded && partyResults.Any())
        //        {
        //            int maxVotes = partyResults.Max(p => p.PartyTotalVotes);
        //            foreach (var party in partyResults)
        //            {
        //                party.IsWinner = party.PartyTotalVotes == maxVotes;
        //                party.IsLoser = party.PartyTotalVotes != maxVotes;
        //            }
        //        }

        //        allPartyResults.AddRange(partyResults);
        //    }

        //    // 2. Generate PDF using QuestPDF
        //    var document = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Margin(20);
        //            page.Size(PageSizes.A4);
        //            page.DefaultTextStyle(x => x.FontSize(12));

        //            page.Header().Text("Vote Casting Results").SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

        //            page.Content().Table(table =>
        //            {
        //                table.ColumnsDefinition(columns =>
        //                {
        //                    columns.ConstantColumn(30); // S.No
        //                    columns.RelativeColumn();    // Election
        //                    columns.RelativeColumn();    // Seat
        //                    columns.RelativeColumn();    // Party
        //                    columns.RelativeColumn();    // Votes
        //                });

        //                table.Header(header =>
        //                {
        //                    header.Cell().Text("S.No").SemiBold();
        //                    header.Cell().Text("Election");
        //                    header.Cell().Text("Seat");
        //                    header.Cell().Text("Party");
        //                    header.Cell().Text("Votes");
        //                });

        //                int counter = 1;
        //                foreach (var party in allPartyResults)
        //                {
        //                    table.Cell().Text(counter++);
        //                    table.Cell().Text(party.ElectionTitle);
        //                    table.Cell().Text(party.ElectionDescription);
        //                    table.Cell().Text(party.PartyName + (party.IsWinner ? " (Winner)" : ""));
        //                    table.Cell().Text(party.PartyTotalVotes.ToString());
        //                }
        //            });
        //            page.Footer()
        //                        .AlignCenter()
        //                        .Text(txt =>
        //                        {
        //                            txt.Span("Page ");
        //                            txt.CurrentPageNumber();
        //                            txt.Span(" of ");
        //                            txt.TotalPages();
        //                        });
        //        });
        //    });

        //    byte[] pdfBytes;
        //    try
        //    {
        //        pdfBytes = document.GeneratePdf();
        //    }
        //    catch (Exception ex)
        //    {
        //        // Debugging
        //        throw new Exception("PDF generation failed: " + ex.Message);
        //    }
        //    return File(pdfBytes, "application/pdf", "VoteResults.pdf");


        //}


        // ---------------- Updated ViewModels ----------------
        public class CandidateResultViewModel
        {
            public int CandidateId { get; set; }
            public string CandidateName { get; set; } = string.Empty;
            public string PartyName { get; set; } = string.Empty;
            public int TotalVotes { get; set; }
            public string ElectionTitle { get; set; } = string.Empty; // Election title
            public string ElectionDescription { get; set; } = string.Empty; // Election description
        }

        public class PartyResultViewModel
        {
            public string PartyName { get; set; } = string.Empty;
            public List<CandidateResultViewModel> Candidates { get; set; } = new List<CandidateResultViewModel>();
            public int PartyTotalVotes { get; set; }
            public string ElectionTitle { get; set; } = string.Empty; // Election title
            public string ElectionDescription { get; set; } = string.Empty; // Election description


            //WINNER SHOW
            public bool IsWinner { get; set; }
            public bool IsLoser { get; set; }
            public bool IsElectionEnded { get; set; }
            public DateTime ElectionEndDate { get; set; }

        }
    }
}
