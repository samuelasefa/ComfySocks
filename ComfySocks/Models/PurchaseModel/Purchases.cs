using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ComfySocks.Models.PurchaseModel
{
    public class Purchases
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Invoice No")]
        public string ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Display(Name = "Supplier's  Name")]
        public int SupplierID { get; set; }
        [Display(Name = "Supplier's Invoice No.")]
        public string SupplierInvoiceNo { get; set; }
        public decimal Total { get; set; }
        [Display(Name = "VAT 15%")]
        public decimal? Vat { get; set; }
        public decimal GrandTotal { get; set; }

        //referance purchaseItem
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
    }
}