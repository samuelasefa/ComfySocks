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
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string FsNo { get; set; }

        [Display(Name ="From")]
        public int SupplierID { get; set; }

        [Display(Name ="Supplier TIN")]
        public int SupplierTin { get; set; }

        [Display(Name ="To")]
        public int CustomerID { get; set; }

        [Display(Name="Supplier's VAT Reg. No")]
        public int SupplierVatRegistrationNo { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of VAT Registration")]
        public DateTime? DateOfVatRegistration { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Of VAT Registration")]
        public DateTime? CDateOfVatRegistration { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Date Of Service")]
        public DateTime? DateOfService { get; set; }

        [Display(Name ="Customer VAT Reg. No")]
        public int CustomerVatRegistrationNo { get; set; }

        [Display(Name ="AmountInWord")]
        public string AmountInWord { get; set; }

        [Display(Name ="Prepared By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Checked By")]
        public string Checkedby { get; set; }

        [Display(Name ="Recived By")]
        public string Reciviedby { get; set; }

        [Display(Name ="Approved By")]
        public string Approvedby { get; set; }

        public string Status { get; set; }

        public float ExciseTax { get; set; }
        public float Service { get; set; }
        public float Total { get; set; }
        public float VAT { get; set; }
        [Display(Name ="Total Selling Price Including")]
        public float TotalSellingPrice  { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}