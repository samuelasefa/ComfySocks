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
        [Display(Name="TIN No.")]
        public int TinNumber { get; set; }
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [Display(Name ="City")]
        public string City{ get; set; }
        [Display(Name ="Sub City")]
        public string SubCity { get; set; }

        //referance to Productionorderinfo
        public virtual ICollection<SalesInformation> SalesInformation { get; set; }
        public virtual ICollection<ProductionOrderInfo> ProductionOrderInfos { get; set; }
    }
}