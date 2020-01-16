using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Order
{
    [Table("ProductionOrderInfo")]
    public partial class ProductionOrderInfo
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }

        [DisplayFormat(DataFormatString ="{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public string OrderNumber { get; set; }

        [Display(Name="From")]
        public string ApplicationUserID { get; set; }
        
        //referance
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual  Customer Customer { get; set; }
        public virtual ICollection<ProductionOrder> ProductionOrders { get; set; }
    }
}