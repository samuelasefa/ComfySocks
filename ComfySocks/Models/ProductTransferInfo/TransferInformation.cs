using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using ComfySocks.Models.InventoryModel;

namespace ComfySocks.Models.ProductTransferInfo
{
    [Table("TransferInformation")]
    public partial class TransferInformation
    {
        public int ID { get; set; }

        [Required(ErrorMessage ="Please Enter Date")]
        [DisplayFormat(DataFormatString ="{0:mm-dd-yyyy}",ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name ="FPT-No")]
        public string FPTNo { get; set; }

        [Display(Name ="To:")]
        [Required(ErrorMessage ="StoreID is required")]
        public int StoreID { get; set; }
        [Display(Name ="Prepared by")]
        public string Preparedby { get; set; }
        [Display(Name = "Recived by")]
        public string Recivedby { get; set; }
        [Display(Name = "Approved by")]
        public string Approvedby { get; set; }
        public string ApplicationUserID { get; set; }

        public string Status { get; set; }

        //referance
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Store Store { get; set; }
        
    }
}