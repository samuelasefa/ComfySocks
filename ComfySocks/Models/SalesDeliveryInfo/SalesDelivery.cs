//using ComfySocks.Models.SalesInfo;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Request;
using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.SalesDeliveryInfo
{
    [Table("SalesDelivery")]
    public partial class SalesDelivery
    {
        public int ID { get; set; }
        public int SalesDeliveryInformationID { get; set; }

        [Required]
        [Display(Name ="SalesID")]
        public int SalesID { get; set; }

        [Required]
        [Display(Name ="Deliverd Quantity")]
        public float Quantity { get; set; }


        public string Remark { get; set; }
        //referance
        public virtual Sales Sales { get; set; }
        public virtual SalesDeliveryInformation SalesDeliveryInformation { get; set; }
    }

    public class SalesDeliveryVM
    {
        public SalesDelivery SalesDelivery { get; set; }
        public string ItemDescription { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public float UnitPrice { get; set; }
        public string Remark { get; set; }
        public float Remaining { get; set; }
    }

    public class DeliveryVMForError
    {
        public SalesDelivery SalesDelivery { get; set; }
        public String Error { get; set; }
    }
}
