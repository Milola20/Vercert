using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VerifyMe.Models;

namespace VerifyMe.Controllers
{
    [Authorize(Users = "Ruk60@yahoo.com")]
    public class AllStudentsController : Controller
    {
        public ApplicationDbContext db = new ApplicationDbContext();
        // GET: AllStudents
        public async Task<ActionResult> Index()
        {
            // Retrieve all the student records from the Student table
            var students = db.Student;

            // Convert the student records to a list asynchronously and pass it to the view
            return View(await students.ToListAsync());
        }

        [HttpGet]
        public ActionResult AddStudent()
        {
            return View();
        }

        public async Task<ActionResult> AddStudent(Students student)
        {
            // Get the user ID of the currently authenticated user
            var AppUser = User.Identity.GetUserId();

            // Find the corresponding organization in the database based on the user ID
            var user = db.Organisations.Where(p => p.ApplicationUserId.ToString() == AppUser).FirstOrDefault();

            if (ModelState.IsValid)
            {
                // Create a new Students object with the provided data
                Students newStudent = new Students
                {
                    ID = Guid.NewGuid(),
                    ApplicationUserId = user.ApplicationUserId,
                    FirstName = student.FirstName,
                    Date = DateTime.Now,
                    MiddleName = student.MiddleName,
                    LastName = student.LastName,
                    DateOfBirth = student.DateOfBirth,
                    Email = student.Email,
                    Sex = student.Sex,
                    Course = student.Course,
                    MatricNumber = student.MatricNumber,
                    CertificateNumber = student.CertificateNumber,
                    YearOfGraduation = student.YearOfGraduation,
                    YearOfMatriculation = student.YearOfMatriculation
                };

                // Add the new student to the Students table
                db.Student.Add(newStudent);
                await db.SaveChangesAsync();

                // Redirect to the StudentDetails action of the same controller with the ID of the newly created student
                return RedirectToAction("StudentDetails", new { id = newStudent.ID });
            }

            // If the model state is not valid, return the view
            return View();
        }


        public ActionResult EditStudent(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditStudent([Bind(Include = "ID,ApplicationUserId,FirstName,LastName,MiddleName,Sex,DateOfBirth,MatricNumber,Course,YearOfGraduation,YearOfMatriculation,Email,CertificateNumber")] Students student)
        {
            if (ModelState.IsValid)
            {
                // Set the state of the student object as Modified in the context
                db.Entry(student).State = EntityState.Modified;
                // Save the changes to the database
                db.SaveChanges();
                // Redirect to the Index action
                return RedirectToAction("Index");
            }

            // If the model state is not valid, return the view with the student object
            return View(student);
        }



        public async Task<ActionResult> StudentDetails(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Students student = await db.Student.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        public ActionResult DeleteStudent(Guid? id)
        {
            if (id == null)
            {
                // If the ID is not provided, return a BadRequest status
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Find the student with the given ID in the database
            Students student = db.Student.Find(id);

            if (student == null)
            {
                // If the student is not found, return a HttpNotFound status
                return HttpNotFound();
            }

            // Pass the student object to the view
            return View(student);
        }


        // POST: Admin/Student/Delete/5
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteStudentConfirmed(Guid id)
        {
            Students student = db.Student.Find(id);
            db.Student.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}