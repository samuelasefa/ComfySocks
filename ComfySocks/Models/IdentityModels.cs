﻿using ComfySocks.Models.GenerateReport;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Items;
using ComfySocks.Models.OfficeDeliveryInfo;
using ComfySocks.Models.OfficeRequest;
using ComfySocks.Models.Order;
using ComfySocks.Models.ProductRecivingInfo;
using ComfySocks.Models.ProductTransferInfo;
using ComfySocks.Models.PurchaseRequestInfo;
using ComfySocks.Models.Repository;
using ComfySocks.Models.Request;
using ComfySocks.Models.RowDeliveryInfo;
using ComfySocks.Models.SalesDeliveryInfo;
using ComfySocks.Models.SalesInfo;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComfySocks.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; internal set; }
        public bool IsActive { get; internal set; }

        public virtual ICollection<RowDelivery> RowDeliveries { get; set; }
        public virtual ICollection<RowDeliveryInformation> RowDeliveryInformation { get; set; }
        public virtual ICollection<StoreType> StoreTypes { get; set; }
        public virtual ICollection<ItemType> ItemTypes{ get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<AvaliableOnStock> AvaliableOnStocks { get; set; }
        public virtual ICollection<StoreRequest> StoreRequests { get; set; }
        public virtual ICollection<OfficeMaterialRequest> OfficeMaterialRequest { get; set; }
        public virtual ICollection<StoreRequestInformation> StoreRequestInformation { get; set; }
        public virtual ICollection<ProductionOrder> ProductioOrders{ get; set; }
        public virtual ICollection<ProductionOrderInfo> ProductionOrderInfos{ get; set; }
        public virtual ICollection<Transfer> Transfers{ get; set; }
        public virtual ICollection<TransferInformation> TransferInformation { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
        public virtual ICollection<SalesInformation> SalesInformation { get; set; }
        public virtual ICollection<ProductlogicalAvaliable> ProductlogicalAvaliables { get; set; }
        public virtual ICollection<RowMaterialRepositery> RowMaterialRepositery { get; set; }
        public virtual ICollection<ProductMaterialRepository> ProductMaterialRepositories { get; set; }
        public virtual ICollection<StockInformation> StockInformation { get; set; }
        public virtual ICollection<OfficeMaterialRequestInformation> OfficeMaterialRequestInformation { get; set; }
        public virtual ICollection<SalesDelivery> SalesDeliveries { get; set; }
        public virtual ICollection<SalesDeliveryInformation> SalesDeliveryInformation { get; set; }
        public virtual ICollection<OfficeDelivery> OfficeDeliveries { get; set; }
        public virtual ICollection<OfficeDeliveryInformation> OfficeDeliveryInformation { get; set; }
        public virtual ICollection<PurchaseRequest> PurchaseRequests { get; set; }
        public virtual ICollection<PurchaseRequestInformation> PurchaseRequestInformation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<ProductInformation> ProductInformation { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ReportInformation> ReportInformation { get; set; }
        public virtual ICollection<LogicalOnTransit> LogicalOnTransit { get; set; }
        public virtual ICollection<MonthlyConsumption> MonthlyConsumptions { get; set; }
        public virtual ICollection<Recivied> Recivieds { get; set; }
        public virtual ICollection<EndingBalance> EndingBalances { get; set; }
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
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
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
            
            modelBuilder.Entity<ItemType>()
              .HasMany(e => e.Items)
              .WithRequired(e => e.ItemType)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<Unit>()
              .HasMany(e => e.Item)
              .WithRequired(e => e.Unit)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(e => e.StoreRequestInformation)
              .WithRequired(e => e.ApplicationUser)
              .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
              .HasMany(e => e.TransferInformation)
              .WithRequired(e => e.ApplicationUser)
              .WillCascadeOnDelete(false);
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(e => e.OfficeMaterialRequestInformation)
              .WithRequired(e => e.ApplicationUser)
              .WillCascadeOnDelete(false);

            //modelBuilder.Entity<OfficeStoreRequest>()
            //  .HasMany(e => e.OfficeDeliveries)
            //  .WithRequired(e => e.OfficeStoreRequest)
            //  .WillCascadeOnDelete(false);

        }

        //DB CONTEXT AREA
        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StoreRequest> StoreRequest { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<AvaliableOnStock> AvaliableOnStocks { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }
        public DbSet<StoreRequestInformation> StoreRequestInformation { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet <ProductionOrder> ProductionOrders { get; set; }
        public DbSet<ProductionOrderInfo> ProductionOrderInfos { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<TransferInformation> TransferInformation{ get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesInformation> SalesInformation { get; set; }
        public DbSet<ProductlogicalAvaliable> ProductlogicalAvaliables { get; set; }
        public DbSet<RowMaterialRepositery> RowMaterialRepositeries { get; set; }
        public DbSet<ProductMaterialRepository> ProductMaterialRepositories { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<StockInformation> StockInformation { get; set; }
        public DbSet<OfficeMaterialRequest> OfficeMaterialRequest { get; set; }
        public DbSet<OfficeMaterialRequestInformation> OfficeMaterialRequestInformation { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SalesDelivery> SalesDeliveries { get; set; }
        public DbSet<SalesDeliveryInformation> SalesDeliveryInformation { get; set; }
        public DbSet<RowDelivery> RowDeliveries { get; set; }
        public DbSet<RowDeliveryInformation> RowDeliveryInformation { get; set; }
        public DbSet<OfficeDelivery> OfficeDeliveries { get; set; }
        public DbSet<OfficeDeliveryInformation> OfficeDeliveryInformation  { get; set; }
        public DbSet<PurchaseRequest> PurchaseRequests { get; set; }
        public DbSet<PurchaseRequestInformation> PurchaseRequestInformation { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInformation> ProductInformation { get; set; }
        public DbSet<Report> Report { get; set; }
        public DbSet<ReportInformation> ReportInformation { get; set; }
        public DbSet<LogicalOnTransit> LogicalOnTransit { get; set; }
        public DbSet<MonthlyConsumption> MonthlyConsumptions { get; set; }
        public DbSet<Recivied> Recivieds { get; set; }
        public DbSet<EndingBalance> EndingBalances { get; set; }
    }
}