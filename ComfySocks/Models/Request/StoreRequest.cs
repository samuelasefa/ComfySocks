using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.Request
{
    public partial class StoreRequest
    {
        public int ID { get; set; }
       
        public int StoreRequestInformationID { get; set; }
        [Required]
        [Display(Name = "Item")]
        public int ItemID { get; set; }
        public string ItemCode { get; set; }

        public float Quantity { get; set; }
        public string Remark { get; set; }
        public bool Deliverd { get; set; }


        public float RemaningDelivery { get; set; }
        //references
        public virtual Item Item { get; set; }
        public virtual StoreRequestInformation StoreRequestInformation { get; set; }
    }
    public partial class StoreRequestVM
    {
        public int ID { get; set; }
        public string ItemDescription { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public string Remark { get; set; }
        //referance 
        public virtual StoreRequest StoreRequest { get; set; }
    }

    public class StoreRequstVMForError
    {
        public StoreRequest StoreRequest { get; set; }
        public String   Error { get; set; }
    }
}