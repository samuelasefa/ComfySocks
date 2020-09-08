using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.GenerateReport
{
    public class ReportInformation
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string GeneratedBy { get; set; }
        public string Remark { get; set; }
        public String  ApplicationUserID { get; set; }
        //Icollection
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}