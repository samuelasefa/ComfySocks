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
        public StoreType StoreType { get; set; }
        public int ItemTypeID { get; set; }
        public int UnitID { get; set; }
        [Required]
        [Display(Name ="ItemCode")]
        public string Code { get; set; }
        public string ApplicationUserID { get; set; }

        //referance
        public virtual RowMaterialRepositery RowMaterialRepositery { get; set; }
        public virtual AvaliableOnStock AvaliableOnStocks { get; set; }
        public virtual ProductMaterialRepository ProductMaterialRepository { get; set; }
        public virtual ProductlogicalAvaliable ProductlogicalAvaliable { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ItemType ItemType { get; set; }
        public virtual Unit Unit { get; set; }

        //Collection
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<StoreRequest> StoreRequest { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<PurchaseRequest> PurchaseRequest { get; set; }
    }
}