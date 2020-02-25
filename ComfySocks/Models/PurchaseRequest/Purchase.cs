using ComfySocks.Models.Items;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Models.PurchaseRequest
{
    public partial class Purchase
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Description")]
        public int ItemID { get; set; }
        [Display(Name ="Purchase ID")]
        public int PurchaseInformtionID { get; set; }
        [Required]
        public int Quantity { get; set; }
        public string SRNo { get; set; }
        public string Remark { get; set; }


        //referance
        public virtual Item Item { get; set; }
        public virtual PurchaseInformation PurchaseInformation { get; set; }

        //ViewModel
        public class PurchaseRequestVM
        {
            public int ID { get; set; }
            public string Description { get; set; }
            public string ItemType { get; set; }
            public string Unit { get; set; }
            //purchase reference
            public virtual Purchase Purchase { get; set; }
        }

        //ErrorList
        public class PurchaseRequstVMForError
        {
            public Purchase Purchase { get; set; }
            public String Error { get; set; }
        }
    }
}