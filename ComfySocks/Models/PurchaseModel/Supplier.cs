using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ComfySocks.Models.PurchaseModel
{
    public class Supplier
    {
        public int ID { get; set; }
        [Required]
        [Display(Name="Supplier'S Name")]
        [StringLength(50, ErrorMessage ="Only 50 characters allowed")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Supplier's Invoice No.")]
        public string InvoiceNumber { get; set; }

        public virtual ICollection<Purchases> PurchaseLists { get; set; }
    }
}