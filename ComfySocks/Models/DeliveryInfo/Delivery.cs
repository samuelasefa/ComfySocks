//using ComfySocks.Models.SalesInfo;
using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.DeliveryInfo
{
    [Table("Delivery")]
    public partial class Delivery
    {
        public int ID { get; set; }
        public int DeliveryInformationID { get; set; }

        [Required]
        [Display(Name ="RowMaterl Request")]
        public int StoreRequestID { get; set; }

        [Required]
        [Display(Name ="Deliverd Quantity")]
        public float Quantity { get; set; }


       
        //referance
        public virtual StoreRequest StoreRequest { get; set; }
        public virtual DeliveryInformation DeliveryInformation { get; set; }
    }

    public class DeliveryVM
    {
        public Delivery Delivery { get; set; }
        public string ItemDescription { get; set; }
        public string Itemtype { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        public float Remaining { get; set; }
    }

    public class DeliveryVMForError
    {
        public Delivery Delivery{ get; set; }
        public String Error { get; set; }
    }
}
