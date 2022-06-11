using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WCWaitingListProject.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; }
        public string? Limits { get; set; }
        [Display(Name = "Status")]
        public bool? AppointmentMade { get; set; }
        public string? Notes { get; set; }
    }
}