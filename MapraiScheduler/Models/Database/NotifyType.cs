using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MapraiScheduler.Notifier;

namespace MapraiScheduler.Models.Database
{
    [Table("notify_type")]
    public class NotifyType : IEntity
    {
        [Key]
        [Column("NotifyTypeID")]
        public long NotifyTypeID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string UniqueName { get; set; }
        public NotifySetting.Priority Priority { get; set; }
        public string NotifyColor { get; set; }
    }
}