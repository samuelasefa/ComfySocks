using ComfySocks.Models.Items;
using ComfySocks.Models.ProductStock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

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
        [Display(Name ="ItemCode")]
        public int ItemID { get; set; }

        [Required]
        [Display(Name ="Product Size")]
        public ProductSize ProductSize{ get; set; }

        [Required]
        [Display(Name ="Product Order Quantity")]
        public float Quantity { get; set; }

        [Display(Name ="Expected Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public int ProductionOrderInfoID { get; set; }

        public float RemaningDelivery { get; set; }
        //reference
        public virtual ProductCode ProductCode { get; set; }
        public virtual Item Item { get; set; }
        public virtual ProductionOrderInfo ProductionOrderInfo { get; set; }
    }
}