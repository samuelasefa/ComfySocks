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
            modelBuilder.Entity<ApplicationUser>()
              .HasMany(e => e.StoreTypes)
              .WithRequired(e => e.ApplicationUser)
              .WillCascadeOnDelete(false);
        }

        //DB CONTEXT AREA
        public DbSet<Unit> Units { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<StoreRequest> StoreRequest { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<StoreType> StoreTypes { get; set; }
        public DbSet<AvaliableOnStock> AvaliableOnStocks { get; set; }
        public IEnumerable ApplicationUsers { get; internal set; }
        public DbSet<StoreRequestInfo> StoreRequestInfo { get; set; }


        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Stock> Stocks { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.StockReferance> StockReferances { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Store> Stores { get; set; }

        public System.Data.Entity.DbSet<ComfySocks.Models.InventoryModel.Supplier> Suppliers { get; set; }
    }
}