using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.DeliveryInfo
{
    [Table("DeliveryInformation")]
    public partial class DeliveryInformation
    {
        public int ID { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name ="De-No")]
        public string DeliveryNumber { get; set; }

        [Display(Name ="For")]
        public string DeliverdTo { get; set; }

        [Display(Name ="Sales Invoice No")]
        public string InvoiceNo { get; set; }

        [Display(Name ="Issued by")]
        public string Issuedby { get; set; }

        [Display(Name = "Approved by")]
        public string Approvedby { get; set; }

        [Display(Name = "Deliverd by")]
        public string Deliverdby { get; set; }

        [Display(Name = "Received by")]
        public string Receivedby { get; set; }

        public string ApplictionUserID { get; set; }


        //referance
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}