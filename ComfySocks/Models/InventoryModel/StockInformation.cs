using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using ComfySocks.Models.Items;
using System.Web.Mvc;

namespace ComfySocks.Models.InventoryModel
{
    [Table("StockInformation")]
    public partial class StockInformation
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Store No.")]
        public string StoreNumber { get; set; }

        [Display(Name = "Purchase Request No.")]
        public string PurchaseRequestNo { get; set; }

        [Required]
        [Display(Name = "Supplier's Name")]
        public int SupplierID { get; set; }

        [Display(Name = "Requsted By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Delivered By")]
        public string Deliveredby { get; set; }

        public string Approvedby { get; set; }

        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}