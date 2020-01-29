using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.ProductStock
{
    public partial class TempProductInfo
    {
        public int ID { get; set; }

        [Display(Name ="Product Entred Date:")]
        [Required(ErrorMessage ="Product Enterd Date is required")]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date{ get; set; }
    
        [Display(Name = "Recivied By")]
        public string Reciviedby { get; set; }

        [Display(Name = "Approved By")]
        public string Approvedby { get; set; }

        [Display(Name = "Prepared By")]
        public string ApplicationUserID { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<TempProductStock> TempProductStock { get; set; }
    }
}