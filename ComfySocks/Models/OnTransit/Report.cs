using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.GenerateReport
{
    public class Report
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Item")]
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public float OnTransit { get; set; }
        public int ReportInformationID { get; set; }
        //refernce
        public virtual Item Item{ get; set; }
        public virtual ReportInformation ReportInformation { get; set; }
    }
    public partial class ReportVM
    {
        public Report Report { get; set; }
        public string ItemDescripton { get; set; }
        public string ItemType { get; set; }
        public string Code { get; set; }
        public float BiginingBalance { get; set; }
        public float OnTransit { get; set; }
    }
}