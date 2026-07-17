using ContractMonthlyClaimsSystem.Data;
using ContractMonthlyClaimsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Claim = ContractMonthlyClaimsSystem.Models.Claim;

namespace ContractMonthlyClaimsSystem.Controllers
{
    public class ReviewerController : Controller
    {
        private readonly AppDbContext _context;
        public ReviewerController(AppDbContext context)
        {
            _context = context;
        }

        //reviewer home screen
        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PendingClaims()
        {
            try
            {
                var pendingClaims = await _context.Claims.Where(c => c.Status == "Pending")
                .Include(c => c.Lecturer).ToListAsync();
                if (pendingClaims == null)
                {
                    TempData["ErrorMessage"] = "No pending claims found.";
                    return View();
                }
                return View(pendingClaims);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not retrieve pending reviews");
                return View();
            }
        }

        //method returns selected claim, its lecturer & supporting docs
        private async Task<Claim?> GetClaim(int claimId)
        {
            return await _context.Claims.Include(c => c.Lecturer).Include(c => c.SupportingDocumentList).FirstOrDefaultAsync(c => c.ClaimId == claimId);
        }

        [HttpGet]
        public async Task<IActionResult> Review(int claimId)
        {
            var claim = await GetClaim(claimId);
            if (claim == null)
            {
                TempData["ErrorMessage"] = "Claim Not Found";
                return RedirectToAction("PendingClaims");
            }

            return View(claim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int claimId, string decision)
        {
            //get reviewer id from email
            var reviewer = await _context.Reviewers.FirstOrDefaultAsync(l => l.Email == Request.Form["Email"].ToString());
            if (reviewer == null)
            {
                TempData["ErrorMessage"] = "Reviewer Not Found";
                return View(await GetClaim(claimId)); //to return page with selected claim details still displayed
            }

            try
            {
                var claim = await GetClaim(claimId);
                if (claim == null)
                {
                    TempData["ErrorMessage"] = "Claim not found.";
                    return RedirectToAction("PendingClaims");
                }

                //update claim status
                claim.Status = decision;

                //create claimreview obj to put in db
                var claimReview = new ClaimReview
                {
                    ClaimId = claimId,
                    ReviewerId = reviewer.ReviewerId,
                    ReviewDate = DateTime.Now,
                    Decision = decision,
                    Reason = Request.Form["Reason"].ToString()
                };

                //save to db
                _context.ClaimReviews.Add(claimReview);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Claim has been {claimReview.Decision} successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                TempData["ErrorMessage"] = "An Error Occurred. Please try again later.";
                return RedirectToAction("PendingClaims");
            }

            return RedirectToAction("PendingClaims");
        }

        [HttpGet]


        [HttpGet]
        public async Task<IActionResult> DownloadDocument(int supportingDocumentId)
        {
            try
            {
                var document = await _context.SupportingDocuments.FindAsync(supportingDocumentId);
                if (document == null)
                {
                    TempData["ErrorMessage"] = "Document Not Found.";
                    return RedirectToAction("Review");
                }

                //file upload: https://stackoverflow.com/questions/23822058/view-and-download-file-from-sql-db-using-entity-framework
                return File(document.FileData, "application/octet-stream", document.FileName);
            }
            catch (Exception)
            {
                return RedirectToAction("Review");
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> ProcessedClaims()
        //{
        //    var processedClaims = await _context.Claims.Include(c => c.Lecturer).Include(c => c.ClaimReview)
        //        .ThenInclude(cr => cr.Reviewer)
        //        .Where(c => c.Status != "Pending").OrderByDescending(c => c.SubmissionDate)
        //        .ToListAsync();

        //    if (processedClaims == null)
        //    {
        //        TempData["ErrorMessage"] = "No Claims Have Been Processed.";
        //        return View();
        //    }

        //    return View(processedClaims);
        //}

    }
}
