using ComfySocks.Models.SalesInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.InventoryModel
{
    [Table("Supplier")]
    public partial class Supplier
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required(ErrorMessage ="*Please Enter Name to the field?")]
        [Display(Name ="Full Name/Name of Company")]
        public string FullName { get; set; }
        [Required(ErrorMessage ="*Please Enter the Tin Number?")]
        [Display(Name ="TIN Number")]
        [StringLength(10, ErrorMessage ="*The Tin Number Must be 10?")]
        public string TinNumber { get; set; }
        public string City { get; set; }
        public string SubCity { get; set; }
        public string Woreda { get; set; }
        public string HouseNo { get; set; }

        [Display(Name ="Invoice No.")]
        public string No { get; set; }

        //referance
        public virtual ICollection<StockInformation> StockInformation { get; set; }
        public virtual ICollection<SalesInformation> SalesInformation { get; set; }
    }
}