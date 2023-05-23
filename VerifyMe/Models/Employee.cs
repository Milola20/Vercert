using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VerifyMe.Models
{
    public class Employee
    {
        public Guid ID { get; set; }
        public string OrgUserId { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpMiddleName { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string YearEnrolled { get; set; }
        public string YearGraduated { get; set; }
        public string CourseOffered { get; set; }
        public string CertificateNumber { get; set; }

        [Required]
        public string MatricNumber { get; set; }
        public string VericationStatus { get; set; }
        public virtual Organisation Organisation { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    //public enum VerificationStatus
    //{
    //    NotVerified,
    //    Verified,
    //    Pending
    //}
}