using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Relational;

namespace MapraiScheduler.Models.Database
{
    [Table("admin")]
    public class Admin : IEntity
    {
        [Key]
        [Column("AdminID")]
        public long AdminID { get; set; }

        public long UserID { get; set; }
        public long PermissionID { get; set; }
    }
}