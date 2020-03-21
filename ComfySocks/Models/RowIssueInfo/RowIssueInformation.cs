using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.RowIssueInfo
{
    public partial class RowIssueInformation
    {
            public int ID { get; set; }
            [Required(ErrorMessage = "Request Date is required")]
            [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
            public DateTime Date { get; set; }

            [Display(Name = "No.")]
            public string RowIssueNumber { get; set; }
            [Required]
            [Display(Name = "Approved By")]
            public string ApprovedBy { get; set; }

            [Display(Name ="Store Request Number")]
            public string RequestNumber { get; set; }
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
            public virtual ICollection<RowIssue> RowIssue { get; set; }
        
    }
}