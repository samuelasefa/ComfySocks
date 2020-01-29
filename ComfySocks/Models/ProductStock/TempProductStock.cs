using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.Repository;
using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductStock
{
    public enum ProductSize
    {
        Small,
        Medium,
        Large
    }
    public partial class TempProductStock
    {
        public int ID { get; set; }

        public int TempProductInfoID { get; set; }

        [Display(Name ="Type of Product ")]
        public int ItemID { get; set; }

        [Display(Name = "Product Size")]
        public ProductSize ProductSize { get; set; }

        public int PStoreID { get; set; }
        
        [Required]
        public float Quantity { get; set; }
        
        public float TempProductTotal { get; set; }

        //reference
        public virtual TempProductInfo TempProductInfo { get; set; }
        public virtual Store Store { get; set; }
        public virtual Item Item { get; set; }

    }
    public class TempProductViewModel
    {
        public string TypeOfProduct { get; set; }
        public string ProductCode { get; set; }
        public string ProductUnit { get; set; }

        public TempProductStock TempProductStock { get; set; }
    }
    public class TemProductVMForError
    {
        public TempProductStock Product { get; set; }

        public String Error { get; set; }
    }

}