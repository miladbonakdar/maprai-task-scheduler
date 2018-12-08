using MySqlX.XDevAPI.Relational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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