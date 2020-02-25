using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using ComfySocks.Models.Items;
using System.Web.Mvc;
using ComfySocks.Models.ProductInfo;

namespace ComfySocks.Models.InventoryModel
{
    [Table("StockInformation")]
    public partial class StockInformation
    {
        public int ID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Store No.")]
        public string StoreNumber { get; set; }

        [Required]
        [Display(Name = "Supplier's Name")]
        public int SupplierID { get; set; }

        [Display(Name = "Requsted By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Recivied By")]
        public string Reciviedby { get; set; }

        public string Approvedby { get; set; }

        public float SubTotal { get; set; }
        public float Tax { get; set; }
        public float GrandTotal { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}