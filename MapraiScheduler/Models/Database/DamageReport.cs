using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using MySqlX.XDevAPI.Relational;

namespace MapraiScheduler.Models.Database
{
    [Table("damage_report")]
    public class DamageReport : IEntity
    {
        [Key]
        [Column("DamageReportID")]
        public long DamageReportID { get; set; }

        public long OrganizationID { get; set; }
        public string DamageReportCode { get; set; }
        public int OTDRValue { get; set; }
        public long CompanyID { get; set; }
        public long ProjectID { get; set; }
        public long ProjectReportID { get; set; }
        public long CableID { get; set; }
        public long CablePrice { get; set; }
        public long JointPrice { get; set; }
        public long JointingPrice { get; set; }
        public long MarkerPrice { get; set; }
        public long ConcretingPrice { get; set; }
        public long DiggingPrice { get; set; }
        public long TotalPrice { get; set; }
        public int DamageReportLevel { get; set; }
        public long DamageReportFileID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string LetterNumber { get; set; }
        public DateTime LetterDate { get; set; }
        public long LetterFileID { get; set; }
        public long PreDamageReportID { get; set; }
        public bool IsArchive { get; set; }
    }
}