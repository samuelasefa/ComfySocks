namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialCreation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvaliableOnStocks",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Avaliable = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Item", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StoreType = c.Int(nullable: false),
                        ItemTypeID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Code = c.Int(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeID, cascadeDelete: true)
                .ForeignKey("dbo.Unit", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.ItemTypeID)
                .Index(t => t.UnitID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        DeliveryInfoID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.DeliveryInformation", t => t.DeliveryInfoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.DeliveryInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.DeliveryInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DeliveryNumber = c.String(),
                        DeliverdTo = c.String(),
                        InvoiceNo = c.String(),
                        Issuedby = c.String(),
                        Approvedby = c.String(),
                        Deliverdby = c.String(),
                        Receivedby = c.String(),
                        ApplictionUserID = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ItemType",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProductAvialableOnStock",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ProductAvaliable = c.Single(nullable: false),
                        RecentlyReducedProduct = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.TempProductStocks", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.TempProductStocks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductCodeID = c.Int(nullable: false),
                        UnitID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.ProductCode", t => t.ProductCodeID, cascadeDelete: true)
                .ForeignKey("dbo.Unit", t => t.UnitID, cascadeDelete: true)
                .Index(t => t.ProductCodeID)
                .Index(t => t.UnitID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProductCode",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        ProductSize = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Unit",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProductInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        ProductNumber = c.String(),
                        StoreID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Supplier_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.Supplier_ID)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.Supplier_ID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TempProductStockID = c.Int(nullable: false),
                        ProductInfoID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        StoreID = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        ProductInformation_ID = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        Item_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductInformation", t => t.ProductInformation_ID)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .ForeignKey("dbo.TempProductStocks", t => t.TempProductStockID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Item", t => t.Item_ID)
                .Index(t => t.TempProductStockID)
                .Index(t => t.StoreID)
                .Index(t => t.ProductInformation_ID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Item_ID);
            
            CreateTable(
                "dbo.Store",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Location = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        StockReferanceID = c.Int(nullable: false),
                        ProductInfoID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        StoreID = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        ProwTotal = c.Single(nullable: false),
                        ProductInformation_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.ProductInformation", t => t.ProductInformation_ID)
                .ForeignKey("dbo.StockReferance", t => t.StockReferanceID, cascadeDelete: true)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .Index(t => t.ItemID)
                .Index(t => t.StockReferanceID)
                .Index(t => t.StoreID)
                .Index(t => t.ProductInformation_ID);
            
            CreateTable(
                "dbo.StockReferance",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreNumber = c.String(),
                        SupplierID = c.Int(nullable: false),
                        InvoiceID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Reciviedby = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.SupplierID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        No = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductionOrderInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        OrderNumber = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TinNumber = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(),
                        City = c.String(),
                        SubCity = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductionOrder",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ProductSize = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ProductionOrderInfoID = c.Int(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ProductCode_ID = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.ProductCode", t => t.ProductCode_ID)
                .ForeignKey("dbo.ProductionOrderInfo", t => t.ProductionOrderInfoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.ProductionOrderInfoID)
                .Index(t => t.ProductCode_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RowMaterialRepositery",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        RowMaterialAavliable = c.Single(nullable: false),
                        RecentlyReducedRowMaterialAvaliable = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SalesInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FsNo = c.Int(nullable: false),
                        InvoiceNumber = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductCodeID = c.Int(nullable: false),
                        SalesInfoID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUserID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        SalesInformation_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.ProductCode", t => t.ProductCodeID, cascadeDelete: true)
                .ForeignKey("dbo.SalesInformation", t => t.SalesInformation_ID)
                .Index(t => t.ProductCodeID)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.SalesInformation_ID);
            
            CreateTable(
                "dbo.StoreIssueInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        IssueNumber = c.Int(nullable: false),
                        StoreRequstionID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreRequestInfo_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.StoreRequestInfo", t => t.StoreRequestInfo_ID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreRequestInfo_ID);
            
            CreateTable(
                "dbo.StoreIssue",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        StoreIssueInfoID = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        UnitPrice = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.StoreIssueInformation", t => t.StoreIssueInfoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.StoreIssueInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.StoreRequestInfo",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StoreRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        StoreRequestInfoID = c.Int(nullable: false),
                        Deliverd = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID, cascadeDelete: true)
                .ForeignKey("dbo.StoreRequestInfo", t => t.StoreRequestInfoID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.StoreRequestInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.TransferInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FPTNo = c.String(),
                        StoreID = c.Int(nullable: false),
                        Preparedby = c.String(),
                        Recivedby = c.String(),
                        Approvedby = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID, cascadeDelete: true)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProductTransfer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TempProductStockID = c.Int(nullable: false),
                        TransferInformationID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ProductionOrder_ID = c.Int(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductionOrder", t => t.ProductionOrder_ID)
                .ForeignKey("dbo.TransferInformation", t => t.TransferInformationID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.TransferInformationID)
                .Index(t => t.ProductionOrder_ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AvaliableOnStocks", "ID", "dbo.Item");
            DropForeignKey("dbo.Product", "Item_ID", "dbo.Item");
            DropForeignKey("dbo.Unit", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductTransfer", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductTransfer", "TransferInformationID", "dbo.TransferInformation");
            DropForeignKey("dbo.ProductTransfer", "ProductionOrder_ID", "dbo.ProductionOrder");
            DropForeignKey("dbo.TransferInformation", "StoreID", "dbo.Store");
            DropForeignKey("dbo.TransferInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreIssue", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "StoreRequestInfoID", "dbo.StoreRequestInfo");
            DropForeignKey("dbo.StoreRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.StoreIssueInformation", "StoreRequestInfo_ID", "dbo.StoreRequestInfo");
            DropForeignKey("dbo.StoreRequestInfo", "StoreID", "dbo.Store");
            DropForeignKey("dbo.StoreRequestInfo", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreIssue", "StoreIssueInfoID", "dbo.StoreIssueInformation");
            DropForeignKey("dbo.StoreIssue", "ItemID", "dbo.Item");
            DropForeignKey("dbo.StoreIssueInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesInformation", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Sales", "SalesInformation_ID", "dbo.SalesInformation");
            DropForeignKey("dbo.Sales", "ProductCodeID", "dbo.ProductCode");
            DropForeignKey("dbo.Sales", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesInformation", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.RowMaterialRepositery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RowMaterialRepositery", "ID", "dbo.Item");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Product", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductionOrder", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductionOrder", "ProductionOrderInfoID", "dbo.ProductionOrderInfo");
            DropForeignKey("dbo.ProductionOrder", "ProductCode_ID", "dbo.ProductCode");
            DropForeignKey("dbo.ProductionOrder", "ItemID", "dbo.Item");
            DropForeignKey("dbo.ProductionOrderInfo", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.ProductionOrderInfo", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductInformation", "Supplier_ID", "dbo.Supplier");
            DropForeignKey("dbo.ProductInformation", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Product", "TempProductStockID", "dbo.TempProductStocks");
            DropForeignKey("dbo.Product", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Stock", "StoreID", "dbo.Store");
            DropForeignKey("dbo.StockReferance", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Stock", "StockReferanceID", "dbo.StockReferance");
            DropForeignKey("dbo.StockReferance", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stock", "ProductInformation_ID", "dbo.ProductInformation");
            DropForeignKey("dbo.Stock", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Store", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Product", "ProductInformation_ID", "dbo.ProductInformation");
            DropForeignKey("dbo.ProductInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductAvialableOnStock", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductAvialableOnStock", "ID", "dbo.TempProductStocks");
            DropForeignKey("dbo.TempProductStocks", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.Item", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.TempProductStocks", "ProductCodeID", "dbo.ProductCode");
            DropForeignKey("dbo.TempProductStocks", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ItemType", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "ItemTypeID", "dbo.ItemType");
            DropForeignKey("dbo.Item", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delivery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Delivery", "DeliveryInfoID", "dbo.DeliveryInformation");
            DropForeignKey("dbo.DeliveryInformation", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvaliableOnStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ProductTransfer", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductTransfer", new[] { "ProductionOrder_ID" });
            DropIndex("dbo.ProductTransfer", new[] { "TransferInformationID" });
            DropIndex("dbo.TransferInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.TransferInformation", new[] { "StoreID" });
            DropIndex("dbo.StoreRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StoreRequests", new[] { "StoreRequestInfoID" });
            DropIndex("dbo.StoreRequests", new[] { "ItemID" });
            DropIndex("dbo.StoreRequestInfo", new[] { "StoreID" });
            DropIndex("dbo.StoreRequestInfo", new[] { "ApplicationUserID" });
            DropIndex("dbo.StoreIssue", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StoreIssue", new[] { "StoreIssueInfoID" });
            DropIndex("dbo.StoreIssue", new[] { "ItemID" });
            DropIndex("dbo.StoreIssueInformation", new[] { "StoreRequestInfo_ID" });
            DropIndex("dbo.StoreIssueInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.Sales", new[] { "SalesInformation_ID" });
            DropIndex("dbo.Sales", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Sales", new[] { "ProductCodeID" });
            DropIndex("dbo.SalesInformation", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SalesInformation", new[] { "CustomerID" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ProductionOrder", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductionOrder", new[] { "ProductCode_ID" });
            DropIndex("dbo.ProductionOrder", new[] { "ProductionOrderInfoID" });
            DropIndex("dbo.ProductionOrder", new[] { "ItemID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "CustomerID" });
            DropIndex("dbo.StockReferance", new[] { "ApplicationUserID" });
            DropIndex("dbo.StockReferance", new[] { "SupplierID" });
            DropIndex("dbo.Stock", new[] { "ProductInformation_ID" });
            DropIndex("dbo.Stock", new[] { "StoreID" });
            DropIndex("dbo.Stock", new[] { "StockReferanceID" });
            DropIndex("dbo.Stock", new[] { "ItemID" });
            DropIndex("dbo.Store", new[] { "ApplicationUserID" });
            DropIndex("dbo.Product", new[] { "Item_ID" });
            DropIndex("dbo.Product", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Product", new[] { "ProductInformation_ID" });
            DropIndex("dbo.Product", new[] { "StoreID" });
            DropIndex("dbo.Product", new[] { "TempProductStockID" });
            DropIndex("dbo.ProductInformation", new[] { "Supplier_ID" });
            DropIndex("dbo.ProductInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductInformation", new[] { "StoreID" });
            DropIndex("dbo.Unit", new[] { "ApplicationUserID" });
            DropIndex("dbo.TempProductStocks", new[] { "ApplicationUserID" });
            DropIndex("dbo.TempProductStocks", new[] { "UnitID" });
            DropIndex("dbo.TempProductStocks", new[] { "ProductCodeID" });
            DropIndex("dbo.ProductAvialableOnStock", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductAvialableOnStock", new[] { "ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.ItemType", new[] { "ApplicationUserID" });
            DropIndex("dbo.DeliveryInformation", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Delivery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Delivery", new[] { "DeliveryInfoID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Item", new[] { "ApplicationUserID" });
            DropIndex("dbo.Item", new[] { "UnitID" });
            DropIndex("dbo.Item", new[] { "ItemTypeID" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ProductTransfer");
            DropTable("dbo.TransferInformation");
            DropTable("dbo.StoreRequests");
            DropTable("dbo.StoreRequestInfo");
            DropTable("dbo.StoreIssue");
            DropTable("dbo.StoreIssueInformation");
            DropTable("dbo.Sales");
            DropTable("dbo.SalesInformation");
            DropTable("dbo.RowMaterialRepositery");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.ProductionOrder");
            DropTable("dbo.Customer");
            DropTable("dbo.ProductionOrderInfo");
            DropTable("dbo.Supplier");
            DropTable("dbo.StockReferance");
            DropTable("dbo.Stock");
            DropTable("dbo.Store");
            DropTable("dbo.Product");
            DropTable("dbo.ProductInformation");
            DropTable("dbo.Unit");
            DropTable("dbo.ProductCode");
            DropTable("dbo.TempProductStocks");
            DropTable("dbo.ProductAvialableOnStock");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.ItemType");
            DropTable("dbo.DeliveryInformation");
            DropTable("dbo.Delivery");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Item");
            DropTable("dbo.AvaliableOnStocks");
        }
    }
}
