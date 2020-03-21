using ComfySocks.Models.Items;
using ComfySocks.Models.ProductInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.InventoryModel
{
    [Table("Stock")]
    public partial class Stock
    {
        public int ID { get; set; }
        [Required]
        public int ItemID { get; set; }

        public string ItemCode { get; set; }
        public int StockInformationID { get; set; }

        [Display(Name ="Unit Price")]
        public float UnitPrice { get; set; }
        [Required]
        public float Quantity { get; set; }
        [Required]
        [Display(Name = "To Store")]
        public int StoreID { get; set; }

        public float Total { get; set; }
        //added
        public float ProwTotal { get; set; }

        public virtual StockInformation StockInformation { get; set; }
        public virtual Store Store { get; set; }
        public virtual Item Item { get; set; }
    }
    public class StockViewModel
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }

        public virtual Stock Stock { get; set; }
    }
}