using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
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
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Product No.")]
        public string ProductNumber { get; set; }
        [Display(Name ="To")]
        public int StoreID { get; set; }
        [Display(Name = "Recived By")]
        public string ApplicationUserID { get; set; }
        [Display(Name ="Deliverd  By")]
        public string Deliverdby { get; set; }


        //reference
        public virtual Store Store { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
 
}