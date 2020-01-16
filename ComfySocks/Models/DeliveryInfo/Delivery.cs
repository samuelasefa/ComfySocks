//using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.DeliveryInfo
{
    [Table("Delivery")]
    public partial class Delivery
    {
        public int ID { get; set; }
        //public int ItemID { get; set; }
        public int DeliveryInfoID { get; set; }

        public float Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remark { get; set; }

        //public int SalesID { get; set; }

        //referance 
        //public virtual Sales Sales { get; set; }
        public virtual DeliveryInformation DeliveryInfo { get; set; }
    }

    //public class DeliveryVM
    //{
    //    public Delivery Delivery { get; set; }
    //    public String ItemDescription { get; set; }
    //    public String ItemCode { get; set; }
    //    public String Unit { get; set; }
    //}
   
}