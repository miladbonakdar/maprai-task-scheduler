using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MapraiScheduler.Models.Database
{
    [Table("station")]
    public class Station : IEntity
    {
        [Key]
        [Column("StattionID")]
        public long StationID { get; set; }

        public long StationCode { get; set; }
        public long OrganizationID { get; set; }
        public string StationName { get; set; }
        public string PreStationID { get; set; }
        public double PreStationDistance { get; set; }
        public string PostStationID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Altitude { get; set; }
        public long STM { get; set; }
    }
}