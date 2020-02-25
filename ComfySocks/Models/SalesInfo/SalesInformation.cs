using ComfySocks.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.SalesInfo
{
    [Table("SalesInformation")]
    public partial class SalesInformation
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string FsNo { get; set; }
        public int InvoiceNumber { get; set; }
        [Display(Name ="Customer")]
        public int CustomerID { get; set; }
        [Display(Name ="Prepared By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Checked By")]
        public string Checkedby { get; set; }

        [Display(Name ="Recived By")]
        public string Reciviedby { get; set; }

        [Display(Name ="Approved By")]
        public string Approvedby { get; set; }

        public string Status { get; set; }

        public float SubTotal { get; set; }
        public float Tax { get; set; }
        public float GrandTotal  { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}