using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VerifyMe.Models;

namespace VerifyMe.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboard
        public ActionResult Index()
        {
            // Get the user ID of the currently authenticated user
            var id = User.Identity.GetUserId();

            // Find the corresponding organization in the database based on the user ID
            var user = db.Organisations.Where(c => c.ApplicationUserId.ToString() == id).FirstOrDefault(); 

            if (user != null)
            {
                // Retrieve the last five employees' records associated with the organization, sorted by ID in descending order
                var vh = db.Employees.Where(e => e.OrgUserId == user.ApplicationUserId).OrderByDescending(e => e.ID).Take(5).ToList();
               
                // Store the list of employees in the ViewBag
                ViewBag.VerificationList = vh;
                
                // Store the first employee in the ViewBag
                ViewBag.vh = vh.FirstOrDefault();
                
                // Store the user profile in the ViewBag
                ViewBag.UserProfile = user; 
            }

            // Count the number of employees with "Verified" verification status
            var VerifiedEmp = db.Employees.Where(d => d.OrgUserId == user.ApplicationUserId).Count(c => c.VericationStatus == "Verified");

            // Store the count in the ViewBag
            ViewBag.VerifiedEmployee = VerifiedEmp;

            // Count the number of employees with "Not Verified" verification status
            var NVerifiedEmp = db.Employees.Where(d => d.OrgUserId == user.ApplicationUserId).Count(c => c.VericationStatus == "Not Verified");        
            ViewBag.NVerifiedEmployee = NVerifiedEmp;


            // Count the number of employees with "Pending" verification status
            var PVerifiedEmp = db.Employees.Where(d => d.OrgUserId == user.ApplicationUserId).Count(c => c.VericationStatus == "Pending");
           
            ViewBag.PVerifiedEmployee = PVerifiedEmp;

            return View();
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddEmployee(Employee employees, Students student)
        {
            // Get the user ID of the currently authenticated user
            var orgUser = User.Identity.GetUserId();

            // Find the corresponding organization in the database based on the user ID
            var user = db.Organisations.Where(p => p.ApplicationUserId == orgUser).FirstOrDefault();

            // Find the existing student record by ID
            Students existingStudents = db.Student.Find(student.ID);

            if (ModelState.IsValid)
            {
                // Create a new Employee object with the provided data
                Employee newEmp = new Employee
                {
                    ID = Guid.NewGuid(),
                    OrgUserId = user.ApplicationUserId,
                    EmpFirstName = employees.EmpFirstName,
                    EmpLastName = employees.EmpLastName,
                    EmpMiddleName = employees.EmpMiddleName,
                    DateOfBirth = employees.DateOfBirth,
                    Gender = employees.Gender,
                    YearEnrolled = employees.YearEnrolled,
                    CertificateNumber = employees.CertificateNumber,
                    CourseOffered = employees.CourseOffered,
                    MatricNumber = employees.MatricNumber,
                    YearGraduated = employees.YearGraduated
                };

                // Check if there is a student record with the same certificate number, matric number, first name, and last name
                var certRecord = db.Student.FirstOrDefault(c => c.CertificateNumber == newEmp.CertificateNumber &&
                                                                c.MatricNumber == newEmp.MatricNumber &&
                                                                c.FirstName == newEmp.EmpFirstName &&
                                                                c.LastName == newEmp.EmpLastName);

                // Check if there is a student record with different certificate number, matric number, first name, and same last name
                var certRecord2 = db.Student.FirstOrDefault(c => c.CertificateNumber != newEmp.CertificateNumber || c.CertificateNumber == null &&
                                                                 c.MatricNumber != newEmp.MatricNumber || c.MatricNumber == null &&
                                                                 c.FirstName != newEmp.EmpFirstName || c.FirstName == null &&
                                                                 c.LastName == newEmp.EmpLastName || c.LastName == null);

                // Check if there is a student record with different certificate number, matric number, or first name, or same last name
                var certRecord3 = db.Student.FirstOrDefault(c => c.CertificateNumber != newEmp.CertificateNumber ||
                                                                 c.MatricNumber != newEmp.MatricNumber ||
                                                                 c.FirstName != newEmp.EmpFirstName ||
                                                                 c.LastName == newEmp.EmpLastName);

                if (certRecord != null)
                {
                    newEmp.VericationStatus = "Verified";

                    // Set a status message for the view
                    TempData["Status"] = "Certificate verified Successfully!";
                }
                else if (certRecord2 != null)
                {
                    newEmp.VericationStatus = "Not Verified";
                    // Set a status message for the view
                    TempData["Status"] = "Verification Failed";
                }
                else if (certRecord3 != null)
                {
                    newEmp.VericationStatus = "Pending";
                    // Set a status message for the view
                    TempData["Status"] = "Verification Pending!!";
                }

                // Add the new employee to the Employees table
                db.Employees.Add(newEmp);
                await db.SaveChangesAsync();

                // Redirect to the VerificationHistory action of the Dashboard controller
                return RedirectToAction("VerificationHistory", "Dashboard");
            }

            // If the model state is not valid, return the view
            return View();
        }


        public ActionResult VerificationHistory(DateTime? startdate, DateTime? enddate)
        {
            // Get the user ID of the currently authenticated user
            string userId = User.Identity.GetUserId();

            // Find the corresponding organization in the database based on the user ID
            var user = db.Organisations.FirstOrDefault(c => c.ApplicationUserId.ToString() == userId);

            ViewBag.UserProfile = user; // Store the user profile in the ViewBag

            var vh = db.Employees.Where(e => e.OrgUserId == user.ApplicationUserId); // Retrieve the employees associated with the organization

            if (user != null)
            {
                // Filter the verification history based on the provided start date and end date, if any
                var verification = vh.Where(v => (!startdate.HasValue || v.Date >= startdate) && (!enddate.HasValue || v.Date <= enddate)).ToList();

                ViewBag.VerificationHistory = verification; // Store the filtered verification history in the ViewBag

                ViewBag.vh = vh.FirstOrDefault(); // Store the first employee in the ViewBag
            }

            return View(user); // Return the view with the user as the model
        }
    }
}