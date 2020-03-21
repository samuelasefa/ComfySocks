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
        [DisplayFormat(DataFormatString ="{0:MM-dd-yyyy}",ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string From { get; set; }

        [Display(Name = "Type of Product")]
        public int TempProductStockInfoID { get; set; }

        [Display(Name ="No")]
        public string FPTNo { get; set; }

        //public string FGRNo { get; set; }

        [Display(Name ="To:")]
        [Required(ErrorMessage ="StoreID is required")]
        public int StoreID { get; set; }
    
        [Display(Name = "Recived by")]
        public string Recivedby { get; set; }
        [Display(Name = "Approved by")]
        public string Approvedby { get; set; }

        [Display(Name ="Prepared by")]
        public string ApplicationUserID { get; set; }

        public string Status { get; set; }

        //referance
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; }
        public virtual Store Store { get; set; }
        
    }
}