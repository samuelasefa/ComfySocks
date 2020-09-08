using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.Order
{
    [Table("Customer")]
    public partial class Customer
    {
        public int ID { get; set; }
        [Display(Name="TIN Number.")]
        public string TinNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }
        
        public string No { get; set; }

        public string City{ get; set; }
        public string SubCity { get; set; }
        public string woreda { get; set; }
        public string HouseNo { get; set; }
        //referance to Productionorderinfo
        public virtual ICollection<SalesInformation> SalesInformation { get; set; }
        public virtual ICollection<ProductionOrderInfo> ProductionOrderInfos { get; set; }
    }
}