using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.ProductRecivingInfo
{
    public partial class ProductInformation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProductInformation()
        {
            Products = new HashSet<Product>();
        }

        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name ="From")]
        public string From { get; set; }
        public int StoreID { get; set; }
        public string FPRNumber { get; set; }
        public int TransferInformationID { get; set; }
        public string Status { get; set; }

        public string DeliverdBy { get; set; }
        public string RecividBy { get; set; }
        public string ApprovedBy { get; set; }


        public string ApplicationUserID { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Store Store{ get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual TransferInformation TransferInformation { get; set; }
    }
}