using ComfySocks.Models.OfficeRequest;
using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.OfficeDeliveryInfo
{
    [Table("OfficeDelivery")]
    public partial class OfficeDelivery
    {
        public int ID { get; set; }
        public int OfficeDeliveryInformationID { get; set; }

        [Required]
        [Display(Name ="Request ID")]
        public int OfficeMaterialRequestID { get; set; }
        public string ItemCode { get; set; }
        [Required]
        [Display(Name ="Deliverd Quantity")]
        public float Quantity { get; set; }

        public string Remark { get; set; }

        //referance
        public virtual OfficeMaterialRequest OfficeMaterialRequest { get; set; }
        public virtual OfficeDeliveryInformation OfficeDeliveryInformation { get; set; }
    }

    public class OfficeDeliveryVM
    {
        public OfficeDelivery OfficeDelivery { get; set; }
        public string ItemDescription { get; set; }
        public string Itemtype { get; set; }
        public string ItemCode { get; set; }
        public string Unit { get; set; }
        public float Remaining { get; set; }
    }

    public class OfficeDeliveryVMForError
    {
        public OfficeDelivery OfficeDelivery{ get; set; }
        public String Error { get; set; }
    }
}
