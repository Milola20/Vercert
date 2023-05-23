using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VerifyMe.Models
{
    public class Organisation
    {
        public Organisation()
        {
            this.Student = new List<Students>();
            this.Employees = new List<Employee>();

        }


        public Guid ID { get; set; }
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegNumber { get; set;}
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Students> Student { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}