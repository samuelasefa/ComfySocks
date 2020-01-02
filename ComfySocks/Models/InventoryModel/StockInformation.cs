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
    [Table("StockReferance")]
    public partial class StockReferance
    {

        public int ID { get; set; }

        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.DateTime)]
        public DateTime Date { get; set; }

        [Display(Name = "Store No.")]
        public string StoreNumber { get; set; }

        [Required]
        [Display(Name = "Supplier's Name")]
        public int SupplierID { get; set; }
        [Display(Name = "Supplier's Invoice No.")]
        public string Invoice { get; set; }

        [Display(Name = "Recived By")]
        public string ApplicationUserID { get; set; }

        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
    [Table("Stock")]
    public partial class Stock {
        public int ID { get; set; }
        [Required]
        public int ItemID { get; set; }
        public int StockReferanceID { get; set; }
        public float Quantity { get; set; }
        [Required]
        [Display(Name ="To Store")]
        public int StoreID  { get; set; }

        public float Total { get; set; }
        
        public virtual StockReferance StockReferance { get; set; }
        public virtual Store Store { get; set; }
        public virtual Item Item { get; set; }
    }
    public class StockViewModel
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Type { get; set; }
        public int Code { get; set; }
        public string Unit { get; set; }

        public virtual Stock Stock { get; set; }
    }
}