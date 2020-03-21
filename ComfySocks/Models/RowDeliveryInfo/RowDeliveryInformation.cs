using ComfySocks.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.RowDeliveryInfo
{
    public partial class RowDeliveryInformation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RowDeliveryInformation()
        {
            RowDeliveries = new HashSet<RowDelivery>();
        }

        public int ID { get; set; }
        public DateTime Date { get; set; }
        [Display(Name ="Section/Department")]
        public string Section { get; set; }
        public string StoreIssueNumber { get; set; }
        public int StoreRequestInformationID { get; set; }
        public string Status { get; set; }

        public string IssuedBy { get; set; }
        public string ApprovedBy { get; set; }
        public string RecivedBy { get; set; }

        public string ApplicationUserID { get; set; }
        //reference
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<RowDelivery> RowDeliveries { get; set; }
        public virtual StoreRequestInformation StoreRequestInformation { get; set; }
    }
}