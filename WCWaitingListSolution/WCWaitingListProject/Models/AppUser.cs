using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WCWaitingListProject.Models
{
    public class AppUser : IdentityUser {
        public string FirstName{ get; set; }
        public string LastName{ get; set; }
        public string AccountType { get; set; }
    }
}