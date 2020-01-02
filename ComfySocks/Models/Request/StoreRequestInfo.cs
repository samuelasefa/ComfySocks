using ComfySocks.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Request
{
    [Table("StoreRequestInfo")]
    public partial class StoreRequestInfo
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "SR-No.")]
        public string StoreRequestNumber { get; set; }

        [Required]
        [Display(Name = "Requested By")]
        public string RequestedBy { get; set; }
        [Required]
        [Display(Name = "Approved By")]
        public string ApprovedBy { get; set; }
        //geting user of application
        [Display(Name ="From")]
        public string ApplicationUserID { get; set; }

        [Display(Name = "To")]
        public string StoreID { get; set; }

        public string Status { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<StoreRequest> StoreRequest { get; set; }
    }
}