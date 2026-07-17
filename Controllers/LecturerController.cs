using ContractMonthlyClaimsSystem.Data;
using ContractMonthlyClaimsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Claim = ContractMonthlyClaimsSystem.Models.Claim;

namespace ContractMonthlyClaimsSystem.Controllers
{
    public class LecturerController : Controller
    {
        private readonly AppDbContext _context;
        private const long MAX_FILE_SIZE = 2147483648; //2GB

        //added .txt type for testing -- remove later
        private readonly string[] permittedExtensions = { ".pdf", ".docx", ".xlsx", ".txt" };
        public LecturerController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Home() //lecturer home screen
        {
            return View();
        }

        public IActionResult SubmitClaim() //display claim submission form
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitClaim(Claim claim)
        {
            //find lecturer by their email
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Email == Request.Form["Email"].ToString());
            if (lecturer == null)
            {
                TempData["ErrorMessage"] = "Lecturer not found. Please enter a valid email address.";
                return View(claim);
            }

            try
            {
                claim.LecturerId = lecturer.LecturerId;
                claim.SubmissionDate = DateTime.Now;
                claim.ClaimAmount = claim.HourlyRate * claim.HoursWorked;

                _context.Claims.Add(claim);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Claim submitted successfully!";
                return RedirectToAction("UploadDocument", new { claimId = claim.ClaimId });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "An error occurred while submitting the claim. Please try again.";
                return View(claim);
            }
        }

        public IActionResult UploadDocument(int claimId)
        {
            ViewBag.ClaimId = claimId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadDocument(List<IFormFile> file, int claimId)
        {
            foreach (var doc in file)
            {
                //verify validity of document for submission
                //maybe make separate method for validation? 
                if (doc.Length == 0)
                {
                    Console.WriteLine("File is empty.");
                    TempData["ErrorMessage"] = "File is empty.";
                }
                else if (doc.Length > MAX_FILE_SIZE)
                {
                    Console.WriteLine("File size exceeds limit.");
                    TempData["ErrorMessage"] = "File size must not exceed 2G";
                }
                else if (!permittedExtensions.Contains(Path.GetExtension(doc.FileName).ToLower()))
                {
                    Console.WriteLine("Invalid file type.");
                    TempData["ErrorMessage"] = "Invalid file type. Only PDF, DOCX, and XLSX are allowed.";
                }


                try
                {
                    //file upload: https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-8.0
                    using (var memoryStream = new MemoryStream())
                    {
                        Console.WriteLine("saving document to database");
                        await doc.CopyToAsync(memoryStream);

                        //create document object
                        var document = new SupportingDocument
                        {
                            ClaimId = claimId,
                            FileName = doc.FileName,
                            FileData = memoryStream.ToArray()
                        };

                        //save document to database
                        _context.SupportingDocuments.Add(document);
                        await _context.SaveChangesAsync();

                        Console.WriteLine("Document saved successfully.");
                    }
                    //assume all documents uploaded at one time, avoiding passing claimId repeatedly 
                    TempData["SuccessMessage"] += string.Format($"{doc.FileName} uploaded successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["ErrorMessage"] = "An error occurred while submitting the claim. Please try again.";
                }
            }
            return View();
        }

        //claim tracking page
        public IActionResult TrackClaim()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrackClaim(string email)
        {
            var lecturer = await _context.Lecturers.FirstOrDefaultAsync(l => l.Email == Request.Form["Email"].ToString());

            if (lecturer == null)
            {
                TempData["ErrorMessage"] = "Lecturer not found. Please enter a valid email address.";
                return View();
            }

            var claims = await _context.Claims
                .Where(c => c.LecturerId == lecturer.LecturerId)
                .OrderByDescending(c => c.SubmissionDate).ToListAsync();

            return View(claims);
        }


        public IActionResult Create()
        {
            var lecturer = new Lecturer();
            return View(lecturer);
        }

        public IActionResult Create(Lecturer lecturer)
        {
            try
            {
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(lecturer);
        }
    }
}
