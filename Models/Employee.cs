using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EmployeeManagementAuth.Models
{
public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Range(1, 120, ErrorMessage = "Age must be between 1 and 120")]
        public int Age { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Salary must be greater than 0!")]
        public double Salary { get; set; }

        //  FK for Department model
        [Required]
        [Display(Name = "Department Name")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a Department!")]
        public int DepartmentId { get; set; }

        //  Using Department model with DepartmentId FK
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }
    }
}
