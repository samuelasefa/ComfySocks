using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.PurchaseRequestInfo
{
    public partial class PurchaseRequest
    {
        public int ID { get; set; }
       
        public int PurchaseRequestInformationID { get; set; }
        [Required]
        [Display(Name = "Description")]
        public int ItemID { get; set; }

        public string ItemCode { get; set; }

        public float Quantity { get; set; }
        public string Remark { get; set; }
        
        //references
        public virtual Item Item { get; set; }
        public virtual PurchaseRequestInformation PurchaseRequestInformation { get; set; }
    }
    public partial class PurchaseRequestVM
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        //referance 
        public virtual PurchaseRequest PurchaseRequest { get; set; }
    }

    public class PurchaseRequstVMForError
    {
        public PurchaseRequest PurchaseRequest { get; set; }
        public String   Error { get; set; }
    }
}