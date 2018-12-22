using MySqlX.XDevAPI.Relational;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DateTime? OTDRTestDate { get; set; }
        public long OTDRTestLocationID { get; set; }
        public int ConcretingMeter { get; set; }
        public int STMDownTime { get; set; }
        public int DownLinkTime { get; set; }
    }
}