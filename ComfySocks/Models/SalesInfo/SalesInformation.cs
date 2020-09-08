using ComfySocks.Models.InventoryModel;
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
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string FsNo { get; set; }

        [Display(Name ="From")]
        public int SupplierID { get; set; }

        [Display(Name ="To")]
        public int CustomerID { get; set; }

        [Display(Name ="AmountInWord")]
        public string AmountInWord { get; set; }

        [Display(Name ="Prepared By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Checked By")]
        public string Checkedby { get; set; }

        [Display(Name ="Approved By")]
        public string Approvedby { get; set; }

        public string Status { get; set; }

        public decimal ExciseTax { get; set; }
        public decimal Service { get; set; }
        public decimal Total { get; set; }
        public decimal VAT { get; set; }
        [Display(Name ="Total Selling Price Including")]
        public decimal TotalSellingPrice  { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}