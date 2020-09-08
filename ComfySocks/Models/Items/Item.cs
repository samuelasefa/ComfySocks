using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.PurchaseRequestInfo;
using ComfySocks.Models.Repository;
using ComfySocks.Models.Request;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Repository;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ComfySocks.Models.Items
{
    public enum StoreType {
        ProductItem,
        RowMaterial,
        OfficeMaterial
    }
    [Table("Item")]
    public class Item
    {
        public int ID { get; set; }
        [Required]
        [Display(Name ="Item Description")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The StoreType field is required")]
        public StoreType? StoreType { get; set; }
        [Required(ErrorMessage = "The ItemType field is required")]
        public int ItemTypeID { get; set; }
        [Required(ErrorMessage = "The Unit field is required")]
        public int UnitID { get; set; }
        [Display(Name ="ItemCode")]
        public string Code { get; set; }
        //referance
        public virtual RowMaterialRepositery RowMaterialRepositery { get; set; }
        public virtual AvaliableOnStock AvaliableOnStocks { get; set; }
        public virtual ProductMaterialRepository ProductMaterialRepository { get; set; }
        public virtual ProductlogicalAvaliable ProductlogicalAvaliable { get; set; }
        public virtual LogicalOnTransit LogicalOnTransit { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual MonthlyConsumption MonthlyConsumption { get; set; }
        public virtual EndingBalance EndingBalance { get; set; }
        public virtual Recivied Recivied { get; set; }
        //Collection
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<StoreRequest> StoreRequest { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<PurchaseRequest> PurchaseRequest { get; set; }
    }
}