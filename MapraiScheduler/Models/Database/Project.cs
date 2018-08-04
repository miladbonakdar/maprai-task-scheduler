using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MapraiScheduler.Models.Database
{
    [Table("project")]
    public class Project : IEntity
    {
        [Key]
        [Column("ProjectID")]
        public long ProjectID { get; set; }

        public DateTime CreationDate { get; set; }
        public long UserID { get; set; }
        public long PhoneID { get; set; }
        public int? RemainingTime { get; set; }
        public long? FailurePositionID { get; set; }
        public string Description { get; set; }
        public long ProjectPhaseID { get; set; }
        public long? StartStationID { get; set; }
        public long? EndStationID { get; set; }
        public DateTime FinishDate { get; set; }
        public DateTime StartDate { get; set; }
        public long ProjectFailureCauseID { get; set; }
        public long CableID { get; set; }
        public int? OTDR { get; set; }
        public long ReportImageFileID { get; set; }
        public long ReportRawFileID { get; set; }
        public long ReportFileID { get; set; }
        public long ProjectReportID { get; set; }
    }
}