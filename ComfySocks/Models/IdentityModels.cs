using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ComfySocks.Models.Request;
using ComfySocks.Models.Order;
using ComfySocks.Models.DeliveryInfo;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.Issue;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Models.ProductStock;
using ComfySocks.Repository;

namespace ComfySocks.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; internal set; }
        public bool IsActive { get; internal set; }

        public virtual ICollection<StoreType> StoreTypes { get; set; }
        public virtual ICollection<ItemType> ItemTypes{ get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<AvaliableOnStock> AvaliableOnStocks { get; set; }
        public virtual ICollection<StoreRequest> StoreRequests { get; set; }
        public virtual ICollection<StoreRequestInfo> StoreRequestInfos { get; set; }
        public virtual ICollection<ProductionOrder> ProductioOrders{ get; set; }
        public virtual ICollection<ProductionOrderInfo> ProductionOrderInfos{ get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<DeliveryInformation> DeliveryInformation { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductInformation> ProductInformation { get; set; }
        public virtual ICollection<Transfer> Transfers{ get; set; }
        public virtual ICollection<TransferInformation> TransferInformation { get; set; }
        public virtual ICollection<StoreIssue> StoreIssues { get; set; }
        public virtual ICollection<StoreIssueInfo> StoreIssueInfos { get; set; }
        //public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<SalesInformation> SalesInformation { get; set; }
        public virtual ICollection<TempProductStock> TempProductStocks { get; set; }
        public virtual ICollection<ProductAvialableOnStock> ProductAvialableOnStock { get; set; }
        public virtual ICollection<RowMaterialRepositery> RowMaterialRepositery { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<ApplicationDbContext>(null);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.Units)
                .WithRequired(e => e.ApplicationUser)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.ItemTypes)
                .WithRequired(e => e.ApplicationUser)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(e => e.Items)
              .WithRequired(e => e.ApplicationUser)
              .WillCascadeOnDelete(false);
        }

        //DB CONTEXT AREA
        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StoreRequest> StoreRequest { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<AvaliableOnStock> AvaliableOnStocks { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }
        public DbSet<StoreRequestInfo> StoreRequestInfo { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet <ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionOrderInfo> ProductionOrderInfos { get; set; }
        public DbSet<DeliveryInformation> DeliveryInformation { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<Product> Products{ get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<ProductInformation> ProductInformation { get; set; }
        public DbSet<TransferInformation> TransferInformation{ get; set; }
        public DbSet<StoreIssue> StoreIssues { get; set; }
        public DbSet<StoreIssueInfo> StoreIssueInfos { get; set; }
        //public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesInformation> SalesInformation { get; set; }
        public DbSet<TempProductStock> TempProductStocks { get; set; }
        public DbSet<ProductCode> ProductCodes { get; set; }
        public DbSet<ProductAvialableOnStock> ProductAvialableOnStock { get; set; }
        public DbSet<RowMaterialRepositery> RowMaterialRepositeries { get; set; }
        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Stock> Stocks { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.StockReferance> StockReferances { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Store> Stores { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Supplier> Suppliers { get; set; }
    }
}