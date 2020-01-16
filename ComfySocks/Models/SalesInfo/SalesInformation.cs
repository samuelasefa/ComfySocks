using ComfySocks.Models.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.SalesInfo
{
    [Table("SalesInformation")]
    public partial class SalesInformation
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public int FsNo { get; set; }
        public int InvoiceNumber { get; set; }
        public int CustomerID { get; set; }


        //reference
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}