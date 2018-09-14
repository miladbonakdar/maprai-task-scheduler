using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Relational;

namespace MapraiScheduler.Models.Database
{
    [Table("project_report")]
    public class ProjectReport : IEntity
    {
        [Key]
        [Column("ProjectReportID")]
        public long ProjectReportID { get; set; }

        public int HandDigging { get; set; }
        public int MachineDigging { get; set; }
        public int CableMeter { get; set; }
        public int JointBoxCount { get; set; }
        public DateTime OTDRTestDate { get; set; }
        public long OTDRTestLocationID { get; set; }
        public int ConcretingMeter { get; set; }
        public int STMDownTime { get; set; }
        public int DownLinkTime { get; set; }
    }
}