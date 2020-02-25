using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.PurchaseRequest
{
    public partial class PurchaseInformation
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string PRNo { get; set; }
        public string To { get; set; }
        [Display(Name ="From")]
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string ItemType { get; set; }
        public string Status { get; set; }

        public bool isNormal { get; set; }
        public bool isUrgent { get; set; }
        [Display(Name ="Very Urgent")]
        public bool isVeryUrgent { get; set; }


        [Display(Name ="Prepared by")]
        public string ApplicationUserID { get; set; }
        public string Checkedby { get; set; }
        public string Approvedby { get; set; }

        //reference
        public virtual Store Store { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Purchase> Purchase { get; set; }

    }
}