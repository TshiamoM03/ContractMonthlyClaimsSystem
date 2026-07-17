using ContractMonthlyClaimsSystem.Data;
using ContractMonthlyClaimsSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContractMonthlyClaimsSystem.Controllers
{
    public class HrController : Controller
    {
        private const double DEDUCTION_RATE = 0.10; //10% tax deduction
        private readonly AppDbContext _context;
        public HrController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllLecturers() //list of lecturers 
        {
            try
            {
                var lecturers = await _context.Lecturers.ToListAsync();
                return View(lecturers);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["ErrorMessage"] = "An error occured: could not retrieve lecturers";
            }
            return View();
        }


        public IActionResult AddLecturer()
        {
            return View();
        } //go to view

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLecturer(Lecturer lecturer)
        {
            try
            {
                _context.Lecturers.Add(lecturer);
                _context.SaveChanges();

                TempData["SuccessMessage"] = string.Format($"{lecturer.Name} added successfully!"); //will display in AllLecturers View
                return RedirectToAction("AllLecturers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occured, lecturer could not be added";
                Console.WriteLine(ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditLecturer(int lecturerId)
        {
            try
            {
                var lecturer = await _context.Lecturers.FindAsync(lecturerId);
                return View(lecturer);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred, could not retrieve lecturer";
                Console.Write(ex.Message);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLecturer(int lecturerId, [Bind("LecturerId, Name, Email, Phone, Address")] Lecturer lecturer)
        {
            if (lecturerId != lecturer.LecturerId)
            {
                return NotFound();
            }
            try
            {
                var ogLecturer = await _context.Lecturers.FindAsync(lecturerId);
                if (ogLecturer == null)
                {
                    return NotFound();
                }
                ogLecturer.Name = lecturer.Name;
                ogLecturer.Email = lecturer.Email;
                ogLecturer.Phone = lecturer.Phone;
                ogLecturer.Address = lecturer.Address;

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Lecturer details successfully updated";
                return RedirectToAction("AllLecturers");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Could not update lecturer details";
                Console.WriteLine(ex.Message);
            }

            return View(lecturer);
        }

        [HttpGet]
        public async Task<IActionResult> ClaimReports()
        {
            try
            {
                var processedClaims = await _context.Claims
                    .Include(c => c.Lecturer)
                    .Include(c => c.ClaimReview)
                    .ThenInclude(cr => cr.Reviewer)
                    .Include(c => c.SupportingDocumentList)
                    .Where(c => c.Status != "Pending").OrderByDescending(c => c.SubmissionDate)
                    .ToListAsync();

                if (processedClaims == null)
                {
                    TempData["ErrorMessage"] = "No verified claims found";
                    return View();
                }

                return View(processedClaims);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred";
                Console.Write("Could not fetch claims" + ex.Message);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Payroll()
        {
            try
            {
                var approvedClaims = await _context.Claims.Include(c => c.Lecturer)
                    .Where(c => c.Status == "Approved").ToListAsync();

                if (approvedClaims == null)
                {
                    TempData["ErrorMessage"] = "No approved claims found";
                    return View();
                }
                return View(approvedClaims);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to retrieve approved claims" + ex.Message);
            }
            return View();
        }
    }
}
