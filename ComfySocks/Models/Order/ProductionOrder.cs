using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Models.Order
{
    [Table("ProductionOrder")]
    public partial class ProductionOrder
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Product Code")]
        public int Code { get; set; }

        [Required]
        [Display(Name ="Item")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name = "Product Size")]
        public ProductSize ProductSize { get; set; }

        [Required]
        [Display(Name ="Product Order Quantity")]
        public float Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name ="Expected Delivery Date")]
        public DateTime? Date { get; set; }

        public int ProductionOrderInfoID { get; set; }

        public bool deliverd { get; set; }
        public float RemaningDelivery { get; set; }
        //reference
        public virtual Item Item { get; set; }
        public virtual ProductionOrderInfo ProductionOrderInfo { get; set; }
    }

    public partial class ProductionOrderVM
    {
        public int ID { get; set; }
        public string ProductCode { get; set; }
        //referance 
        public virtual ProductionOrder ProductionOrder { get; set; }
    }


    public class ProductionOrderVMForError
    {
        public ProductionOrder ProductionOrder { get; set; }

        public String Error { get; set; }
    }
}