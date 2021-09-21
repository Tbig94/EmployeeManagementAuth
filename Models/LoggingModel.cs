using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementAuth.Models
{
    public class LoggingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string LogType { get; set; }

        [Required]
        public string ObjType { get; set; }

        public LoggingModel(int Id, string Title, string Body, DateTime Date, string LogType, string ObjType)
        {
            this.Id = Id;
            this.Title = Title;
            this.Body = Body;
            this.Date = Date;
            this.LogType = LogType;
            this.ObjType = ObjType;
        }
    }
}
