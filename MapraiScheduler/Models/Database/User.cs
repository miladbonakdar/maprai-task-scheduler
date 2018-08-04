using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Relational;

namespace MapraiScheduler.Models.Database
{
    [Table("user")]
    public class User : IEntity
    {
        [Key]
        [Column("UserID")]
        public long UserID { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public bool ValidSessionID { get; set; }
        public DateTime SignupDate { get; set; }
        public bool Gender { get; set; }
        public string Description { get; set; }
        public long AvatarID { get; set; }
        public string SessionID { get; set; }
        public DateTime LastActiveTime { get; set; }
        public long OrganizationID { get; set; }
    }
}