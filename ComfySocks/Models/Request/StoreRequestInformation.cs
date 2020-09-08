using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.Request
{
    public partial class StoreRequestInformation
    {
            public int ID { get; set; }
            [Required(ErrorMessage = "Request Date is required")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

            [Display(Name = "SR-No.")]
            public string StoreRequestNumber { get; set; }

            [Required]
            [Display(Name = "Approved By")]
            public string ApprovedBy { get; set; }
            //geting user of application
            [Display(Name = "From")]
            public string From { get; set; }

            public string ApplicationUserID { get; set; }

            [Display(Name = "To")]
            public int StoreID { get; set; }

            public string Status { get; set; }
            //reference
            public virtual ApplicationUser ApplicationUser { get; set; }
            public virtual Store Store { get; set; }
            public virtual ICollection<StoreRequest> StoreRequest { get; set; }
        
    }
}