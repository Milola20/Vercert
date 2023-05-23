using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyMe.Models
{
    public class Students
    {
        public Guid ID { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Sex Sex { get; set; }
        public string DateOfBirth { get; set; }
        public string MatricNumber { get; set; }
        public string Course { get; set; }
        public string YearOfGraduation { get; set; }
        public string YearOfMatriculation { get; set; }
        public string Email { get; set; }
        public virtual Organisation User { get; set; }
        public string CertificateNumber { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }

    public enum Sex
    {
        Male,
        Female
    }
}