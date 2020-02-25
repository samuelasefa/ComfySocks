using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;
using ComfySocks.Models.Items;
using System.Web.Mvc;
using ComfySocks.Models.ProductInfo;

namespace ComfySocks.Models.ProductStock
{
    [Table("ProStockInformation")]
    public partial class ProStockInformation
    {
        public int ID { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Store No.")]
        public string StoreNumber { get; set; }
        
        [Display(Name = "Requsted By")]
        public string ApplicationUserID { get; set; }

        [Display(Name ="Recivied By")]
        public string Reciviedby { get; set; }

        public string Approvedby { get; set; }


        public string Status { get; set; }

        public virtual ICollection<ProStock> ProStocks { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }
}