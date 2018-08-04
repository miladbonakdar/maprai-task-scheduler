﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Notifier;

namespace MapraiScheduler.Models.Database
{
    [Table("notify")]
    public class Notify : IEntity
    {
        [Key]
        [Column("NotifyID")]
        public long NotifyID { get; set; }

        public long NotifyTypeID { get; set; }
        public long? ProjectID { get; set; }
        public long? PhoneID { get; set; }
        public long? UserID { get; set; }
        public bool Seen { get; set; }
        public DateTime CreationDate { get; set; }
        public NotifySetting.Priority Priority { get; set; }
    }
}