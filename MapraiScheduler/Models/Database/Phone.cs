using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapraiScheduler.Models.Database
{
    [Table("phone")]
    public class Phone : IEntity
    {
        [Key]
        [Column("PhoneID")]
        public long PhoneID { get; set; }

        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public bool ValidSessionID { get; set; }
        public long DepoID { get; set; }
        public long LastPositionID { get; set; }
        public int Battery { get; set; }
        public bool GpsState { get; set; }
        public long LastActiveTimeUnix { get; set; }
        public DateTime LastActiveTime { get; set; }
        public int? SmsValidationCode { get; set; }
    }
}