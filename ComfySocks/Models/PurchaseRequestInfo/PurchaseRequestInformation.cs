using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ComfySocks.Models.PurchaseRequestInfo
{
    public enum RequestType
    {
        [Description("Normal")]
        Normal,
        [Description("Urgent")]
        Urgent,
        [Description("VeryUrgent")]
        VeryUrgent
    }
    public partial class PurchaseRequestInformation
    {
            public int ID { get; set; }
            [Required(ErrorMessage = "Request Date is required")]
            [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
            public DateTime Date { get; set; }
            public string PurchaseRequestNumber{ get; set; }
            [Display(Name = "Approved By")]
            public string ApprovedBy { get; set; }
            [Display(Name = "Checked By")]
            public string CheckedBy{ get; set; }
        //geting user of application
            [Display(Name = "To")]
            public string To { get; set; }

            public string ApplicationUserID { get; set; }

            [Display(Name = "From:Project")]
            public int StoreID { get; set; }

            public string ItemType { get; set; }
            public string StoreName { get; set; }
            public RequestType RequestType { get; set; }
            public string Status { get; set; }
            //reference
            public virtual ApplicationUser ApplicationUser { get; set; }
            public virtual Store Store { get; set; }
            public virtual ICollection<PurchaseRequest> PurchaseRequest { get; set; }
        
    }
}