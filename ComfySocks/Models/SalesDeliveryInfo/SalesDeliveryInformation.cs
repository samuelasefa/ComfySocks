using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Request;
using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.SalesDeliveryInfo
{
    [Table("SalesDeliveryInformation")]
    public partial class SalesDeliveryInformation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SalesDeliveryInformation()
        {
            SalesDeliveries = new HashSet<SalesDelivery>();
        }

        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Transfer No")]
        public int SalesInformationID { get; set; }

        [Display(Name ="De-No")]
        public string DeliveryNumber { get; set; }

        [Display(Name ="Section/Department")]
        public string From { get; set; }

        [Display(Name ="Issued By")]
        public string Issuedby { get; set; }

        [Display(Name = "Received by")]
        public string Receivedby { get; set; }


        [Display(Name = "Approved By")]
        public string Approvedby { get; set; }

        [Display(Name ="Deliverd by")]
        public string Deliverdby { get; set; }
        public string ApplictionUserID { get; set; }

        public string Status { get; set; }
        //referance
        public virtual ICollection<SalesDelivery> SalesDeliveries { get; set; }
        public virtual SalesInformation SalesInformation { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}