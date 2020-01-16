using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductInfo
{
    [Table("ProductInformation")]
    public partial class ProductInformation
    {

        public int ID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Product No.")]
        public string ProductNumber { get; set; }
        [Display(Name ="To")]
        public int StoreID { get; set; }
        [Display(Name = "Recived By")]
        public string ApplicationUserID { get; set; }


        //reference
        public virtual Store Store { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
    [Table("Product")]
    public partial class Product
    {
        public int ID { get; set; }
        [Required]
        public int TempProductStockID { get; set; }
        public int ProductInfoID { get; set; }
        
        [Required]
        public float Quantity { get; set; }
        public string Remark { get; set; }

        [Required]
        [Display(Name = "To Store")]
        public int StoreID { get; set; }

        public float Total { get; set; }

        public virtual ProductInformation ProductInformation { get; set; }
        public virtual Store Store { get; set; }
        public virtual TempProductStock TempProductStock { get; set; }
    }
    public class ProductViewModel
    {
        public int ID { get; set; }
        public string ProductName { get; set; }
        public int Code { get; set; }
        public int BatchNo { get; set; }
        public string Unit { get; set; }

        public virtual Product Product { get; set; }
    }
}