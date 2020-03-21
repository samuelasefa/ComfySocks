using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.OfficeIssueInfo
{
    public partial class OfficeIssueInformation
    {
            public int ID { get; set; }
            [Required(ErrorMessage = "Request Date is required")]
            [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
            public DateTime Date { get; set; }

            [Display(Name = "OR-No.")]
            public string OfficeIssueNumber { get; set; }

            [Display(Name ="Reqisition No.")]
            public string RequestNumber { get; set; }
            [Display(Name ="Section/Department")]
            public string Section { get; set; }
            [Required]
            [Display(Name = "Approved By")]
            public string ApprovedBy { get; set; }

            public string IssuedBy { get; set; }
            public string ReciviedBy { get; set; }
            //geting user of application
            public string ApplicationUserID { get; set; }
        
            public string Status { get; set; }
            //reference
            public virtual ApplicationUser ApplicationUser { get; set; }
            public virtual Store Store { get; set; }
            public virtual ICollection<OfficeIssue> OfficeIssue { get; set; }
        
    }
}