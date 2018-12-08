using MySqlX.XDevAPI.Relational;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapraiScheduler.Models.Database
{
    [Table("depo")]
    public class Depo : IEntity
    {
        [Key]
        [Column("DepoID")]
        public long DepoID { get; set; }

        public long OrganPositionID { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public long StationID { get; set; }
    }
}