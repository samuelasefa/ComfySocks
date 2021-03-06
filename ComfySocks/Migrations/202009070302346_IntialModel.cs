namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IntialModel : DbMigration
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
                        Code = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Unit", t => t.UnitID)
                .ForeignKey("dbo.ItemType", t => t.ItemTypeID)
                .Index(t => t.ItemTypeID)
                .Index(t => t.UnitID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.EndingBalances",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        EndingQty = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
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
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LogicalOnTransits",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        OnTransitAvaliable = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.MonthlyConsumptions",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Consumption = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.OfficeDelivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OfficeDeliveryInformationID = c.Int(nullable: false),
                        OfficeMaterialRequestID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OfficeDeliveryInformation", t => t.OfficeDeliveryInformationID)
                .ForeignKey("dbo.OfficeMaterialRequests", t => t.OfficeMaterialRequestID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OfficeDeliveryInformationID)
                .Index(t => t.OfficeMaterialRequestID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.OfficeDeliveryInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        OfficeDeliveryNumber = c.String(),
                        Section = c.String(),
                        OfficeMaterialRequestInformationID = c.Int(nullable: false),
                        Receivedby = c.String(),
                        ApprovedBy = c.String(),
                        Issuedby = c.String(),
                        ApplictionUserID = c.String(),
                        Status = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.OfficeMaterialRequestInformations", t => t.OfficeMaterialRequestInformationID)
                .Index(t => t.OfficeMaterialRequestInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.OfficeMaterialRequestInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        From = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.OfficeMaterialRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OfficeMaterialRequestInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.OfficeMaterialRequestInformations", t => t.OfficeMaterialRequestInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.OfficeMaterialRequestInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
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
                "dbo.ProductInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        From = c.String(),
                        StoreID = c.Int(nullable: false),
                        FPRNumber = c.String(),
                        TransferInformationID = c.Int(nullable: false),
                        Status = c.String(),
                        DeliverdBy = c.String(),
                        RecividBy = c.String(),
                        ApprovedBy = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.TransferInformation", t => t.TransferInformationID)
                .Index(t => t.StoreID)
                .Index(t => t.TransferInformationID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransferID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        ProductQuantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ProductInformationID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ProductInformations", t => t.ProductInformationID)
                .ForeignKey("dbo.Transfer", t => t.TransferID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.TransferID)
                .Index(t => t.ProductInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Transfer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TransferInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ProductCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.TransferInformation", t => t.TransferInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.TransferInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.TransferInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        From = c.String(),
                        FPTNo = c.String(),
                        StoreID = c.Int(nullable: false),
                        Recivedby = c.String(),
                        Approvedby = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.StoreID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.PurchaseRequestInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        PurchaseRequestNumber = c.String(),
                        ApprovedBy = c.String(),
                        CheckedBy = c.String(),
                        To = c.String(),
                        StoreRequestNumber = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        ItemType = c.String(),
                        StoreName = c.String(),
                        RequestType = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.PurchaseRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PurchaseRequestInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.PurchaseRequestInformations", t => t.PurchaseRequestInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.PurchaseRequestInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Stock",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        StockInformationID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Single(nullable: false),
                        StoreID = c.Int(nullable: false),
                        OnTransit = c.Single(nullable: false),
                        Total = c.Single(nullable: false),
                        ProwTotal = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.StockInformation", t => t.StockInformationID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .Index(t => t.ItemID)
                .Index(t => t.StockInformationID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StockInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreNumber = c.String(),
                        PurchaseRequestNo = c.String(),
                        SupplierID = c.Int(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                        Deliveredby = c.String(),
                        Approvedby = c.String(),
                        SubTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Tax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        GrandTotal = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID)
                .Index(t => t.SupplierID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FullName = c.String(nullable: false),
                        TinNumber = c.String(nullable: false, maxLength: 10),
                        City = c.String(),
                        SubCity = c.String(),
                        Woreda = c.String(),
                        HouseNo = c.String(),
                        No = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.SalesInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        FsNo = c.String(),
                        SupplierID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        AmountInWord = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                        Checkedby = c.String(),
                        Approvedby = c.String(),
                        Status = c.String(),
                        ExciseTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Service = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VAT = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalSellingPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Customer", t => t.CustomerID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID)
                .Index(t => t.SupplierID)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TinNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        FullName = c.String(nullable: false),
                        No = c.String(),
                        City = c.String(),
                        SubCity = c.String(),
                        woreda = c.String(),
                        HouseNo = c.String(),
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
                        Status = c.String(),
                        From = c.String(),
                        To = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Customer", t => t.CustomerID)
                .Index(t => t.CustomerID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProductionOrder",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ProductSize = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        Date = c.DateTime(),
                        ProductionOrderInfoID = c.Int(nullable: false),
                        deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.ProductionOrderInfo", t => t.ProductionOrderInfoID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.ProductionOrderInfoID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        ProductCode = c.String(),
                        SalesInformationID = c.Int(nullable: false),
                        Quantity = c.Single(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.SalesInformation", t => t.SalesInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.SalesInformationID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.ProductlogicalAvaliable",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        LogicalProductAvaliable = c.Single(nullable: false),
                        RecentlyReduced = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ProductMaterialRepositories",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ProductMaterialAavliable = c.Single(nullable: false),
                        RecentlyReducedProductMaterialAvaliable = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.Recivieds",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        ReciviedQuantity = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.ReportInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        GeneratedBy = c.String(),
                        Remark = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ItemID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        OnTransit = c.Single(nullable: false),
                        ReportInformationID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.ReportInformations", t => t.ReportInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ItemID)
                .Index(t => t.ReportInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RowDelivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StoreRequestID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        DeliveryQuantity = c.Double(nullable: false),
                        Remark = c.String(),
                        RowDeliveryInformationID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.RowDeliveryInformations", t => t.RowDeliveryInformationID)
                .ForeignKey("dbo.StoreRequests", t => t.StoreRequestID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.StoreRequestID)
                .Index(t => t.RowDeliveryInformationID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.RowDeliveryInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Section = c.String(),
                        StoreIssueNumber = c.String(),
                        StoreRequestInformationID = c.Int(nullable: false),
                        Status = c.String(),
                        IssuedBy = c.String(),
                        ApprovedBy = c.String(),
                        RecividBy = c.String(),
                        ApplicationUserID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .ForeignKey("dbo.StoreRequestInformations", t => t.StoreRequestInformationID)
                .Index(t => t.StoreRequestInformationID)
                .Index(t => t.ApplicationUserID);
            
            CreateTable(
                "dbo.StoreRequestInformations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        StoreRequestNumber = c.String(),
                        ApprovedBy = c.String(nullable: false),
                        From = c.String(),
                        ApplicationUserID = c.String(nullable: false, maxLength: 128),
                        StoreID = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Store", t => t.StoreID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserID)
                .Index(t => t.ApplicationUserID)
                .Index(t => t.StoreID);
            
            CreateTable(
                "dbo.StoreRequests",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StoreRequestInformationID = c.Int(nullable: false),
                        ItemID = c.Int(nullable: false),
                        ItemCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        Deliverd = c.Boolean(nullable: false),
                        RemaningDelivery = c.Single(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Item", t => t.ItemID)
                .ForeignKey("dbo.StoreRequestInformations", t => t.StoreRequestInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.StoreRequestInformationID)
                .Index(t => t.ItemID)
                .Index(t => t.ApplicationUser_Id);
            
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
                "dbo.SalesDelivery",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SalesDeliveryInformationID = c.Int(nullable: false),
                        SalesID = c.Int(nullable: false),
                        ProductCode = c.String(),
                        Quantity = c.Single(nullable: false),
                        Remark = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Sales", t => t.SalesID)
                .ForeignKey("dbo.SalesDeliveryInformation", t => t.SalesDeliveryInformationID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.SalesDeliveryInformationID)
                .Index(t => t.SalesID)
                .Index(t => t.ApplicationUser_Id);
            
            CreateTable(
                "dbo.SalesDeliveryInformation",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        SalesInformationID = c.Int(nullable: false),
                        DeliveryNumber = c.String(),
                        From = c.String(),
                        Issuedby = c.String(),
                        Receivedby = c.String(),
                        Approvedby = c.String(),
                        Deliverdby = c.String(),
                        ApplictionUserID = c.String(),
                        Status = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.SalesInformation", t => t.SalesInformationID)
                .Index(t => t.SalesInformationID)
                .Index(t => t.ApplicationUser_Id);
            
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
            DropForeignKey("dbo.Item", "ItemTypeID", "dbo.ItemType");
            DropForeignKey("dbo.Unit", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "UnitID", "dbo.Unit");
            DropForeignKey("dbo.Transfer", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.TransferInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.StoreRequestInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesDelivery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesDeliveryInformation", "SalesInformationID", "dbo.SalesInformation");
            DropForeignKey("dbo.SalesDelivery", "SalesDeliveryInformationID", "dbo.SalesDeliveryInformation");
            DropForeignKey("dbo.SalesDeliveryInformation", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesDelivery", "SalesID", "dbo.Sales");
            DropForeignKey("dbo.RowMaterialRepositery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RowMaterialRepositery", "ID", "dbo.Item");
            DropForeignKey("dbo.RowDelivery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.RowDelivery", "StoreRequestID", "dbo.StoreRequests");
            DropForeignKey("dbo.RowDeliveryInformations", "StoreRequestInformationID", "dbo.StoreRequestInformations");
            DropForeignKey("dbo.StoreRequests", "StoreRequestInformationID", "dbo.StoreRequestInformations");
            DropForeignKey("dbo.StoreRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.StoreRequestInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.RowDelivery", "RowDeliveryInformationID", "dbo.RowDeliveryInformations");
            DropForeignKey("dbo.RowDeliveryInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reports", "ReportInformationID", "dbo.ReportInformations");
            DropForeignKey("dbo.Reports", "ItemID", "dbo.Item");
            DropForeignKey("dbo.ReportInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recivieds", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recivieds", "ID", "dbo.Item");
            DropForeignKey("dbo.PurchaseRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductMaterialRepositories", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductMaterialRepositories", "ID", "dbo.Item");
            DropForeignKey("dbo.ProductlogicalAvaliable", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductlogicalAvaliable", "ID", "dbo.Item");
            DropForeignKey("dbo.ProductionOrder", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Product", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeMaterialRequestInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeMaterialRequests", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeDelivery", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeDelivery", "OfficeMaterialRequestID", "dbo.OfficeMaterialRequests");
            DropForeignKey("dbo.OfficeDeliveryInformation", "OfficeMaterialRequestInformationID", "dbo.OfficeMaterialRequestInformations");
            DropForeignKey("dbo.OfficeMaterialRequestInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Stock", "StoreID", "dbo.Store");
            DropForeignKey("dbo.StockInformation", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.SalesInformation", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Sales", "SalesInformationID", "dbo.SalesInformation");
            DropForeignKey("dbo.Sales", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Sales", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesInformation", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.ProductionOrder", "ProductionOrderInfoID", "dbo.ProductionOrderInfo");
            DropForeignKey("dbo.ProductionOrder", "ItemID", "dbo.Item");
            DropForeignKey("dbo.ProductionOrderInfo", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.ProductionOrderInfo", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.SalesInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stock", "StockInformationID", "dbo.StockInformation");
            DropForeignKey("dbo.StockInformation", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Stock", "ItemID", "dbo.Item");
            DropForeignKey("dbo.PurchaseRequestInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.PurchaseRequests", "PurchaseRequestInformationID", "dbo.PurchaseRequestInformations");
            DropForeignKey("dbo.PurchaseRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.PurchaseRequestInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductInformations", "TransferInformationID", "dbo.TransferInformation");
            DropForeignKey("dbo.ProductInformations", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Product", "TransferID", "dbo.Transfer");
            DropForeignKey("dbo.Transfer", "TransferInformationID", "dbo.TransferInformation");
            DropForeignKey("dbo.TransferInformation", "StoreID", "dbo.Store");
            DropForeignKey("dbo.Transfer", "ItemID", "dbo.Item");
            DropForeignKey("dbo.Product", "ProductInformationID", "dbo.ProductInformations");
            DropForeignKey("dbo.ProductInformations", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Store", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.OfficeMaterialRequests", "OfficeMaterialRequestInformationID", "dbo.OfficeMaterialRequestInformations");
            DropForeignKey("dbo.OfficeMaterialRequests", "ItemID", "dbo.Item");
            DropForeignKey("dbo.OfficeDelivery", "OfficeDeliveryInformationID", "dbo.OfficeDeliveryInformation");
            DropForeignKey("dbo.OfficeDeliveryInformation", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MonthlyConsumptions", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.MonthlyConsumptions", "ID", "dbo.Item");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LogicalOnTransits", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.LogicalOnTransits", "ID", "dbo.Item");
            DropForeignKey("dbo.ItemType", "ApplicationUserID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Item", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EndingBalances", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AvaliableOnStocks", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.EndingBalances", "ID", "dbo.Item");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Unit", new[] { "ApplicationUserID" });
            DropIndex("dbo.SalesDeliveryInformation", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SalesDeliveryInformation", new[] { "SalesInformationID" });
            DropIndex("dbo.SalesDelivery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.SalesDelivery", new[] { "SalesID" });
            DropIndex("dbo.SalesDelivery", new[] { "SalesDeliveryInformationID" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RowMaterialRepositery", new[] { "ID" });
            DropIndex("dbo.StoreRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.StoreRequests", new[] { "ItemID" });
            DropIndex("dbo.StoreRequests", new[] { "StoreRequestInformationID" });
            DropIndex("dbo.StoreRequestInformations", new[] { "StoreID" });
            DropIndex("dbo.StoreRequestInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.RowDeliveryInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.RowDeliveryInformations", new[] { "StoreRequestInformationID" });
            DropIndex("dbo.RowDelivery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.RowDelivery", new[] { "RowDeliveryInformationID" });
            DropIndex("dbo.RowDelivery", new[] { "StoreRequestID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.Reports", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Reports", new[] { "ReportInformationID" });
            DropIndex("dbo.Reports", new[] { "ItemID" });
            DropIndex("dbo.ReportInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.Recivieds", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Recivieds", new[] { "ID" });
            DropIndex("dbo.ProductMaterialRepositories", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductMaterialRepositories", new[] { "ID" });
            DropIndex("dbo.ProductlogicalAvaliable", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductlogicalAvaliable", new[] { "ID" });
            DropIndex("dbo.Sales", new[] { "ApplicationUserID" });
            DropIndex("dbo.Sales", new[] { "SalesInformationID" });
            DropIndex("dbo.Sales", new[] { "ItemID" });
            DropIndex("dbo.ProductionOrder", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ProductionOrder", new[] { "ProductionOrderInfoID" });
            DropIndex("dbo.ProductionOrder", new[] { "ItemID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductionOrderInfo", new[] { "CustomerID" });
            DropIndex("dbo.SalesInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.SalesInformation", new[] { "CustomerID" });
            DropIndex("dbo.SalesInformation", new[] { "SupplierID" });
            DropIndex("dbo.StockInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.StockInformation", new[] { "SupplierID" });
            DropIndex("dbo.Stock", new[] { "StoreID" });
            DropIndex("dbo.Stock", new[] { "StockInformationID" });
            DropIndex("dbo.Stock", new[] { "ItemID" });
            DropIndex("dbo.PurchaseRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.PurchaseRequests", new[] { "ItemID" });
            DropIndex("dbo.PurchaseRequests", new[] { "PurchaseRequestInformationID" });
            DropIndex("dbo.PurchaseRequestInformations", new[] { "StoreID" });
            DropIndex("dbo.PurchaseRequestInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.TransferInformation", new[] { "ApplicationUserID" });
            DropIndex("dbo.TransferInformation", new[] { "StoreID" });
            DropIndex("dbo.Transfer", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Transfer", new[] { "ItemID" });
            DropIndex("dbo.Transfer", new[] { "TransferInformationID" });
            DropIndex("dbo.Product", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Product", new[] { "ProductInformationID" });
            DropIndex("dbo.Product", new[] { "TransferID" });
            DropIndex("dbo.ProductInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.ProductInformations", new[] { "TransferInformationID" });
            DropIndex("dbo.ProductInformations", new[] { "StoreID" });
            DropIndex("dbo.Store", new[] { "ApplicationUserID" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "ItemID" });
            DropIndex("dbo.OfficeMaterialRequests", new[] { "OfficeMaterialRequestInformationID" });
            DropIndex("dbo.OfficeMaterialRequestInformations", new[] { "StoreID" });
            DropIndex("dbo.OfficeMaterialRequestInformations", new[] { "ApplicationUserID" });
            DropIndex("dbo.OfficeDeliveryInformation", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfficeDeliveryInformation", new[] { "OfficeMaterialRequestInformationID" });
            DropIndex("dbo.OfficeDelivery", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.OfficeDelivery", new[] { "OfficeMaterialRequestID" });
            DropIndex("dbo.OfficeDelivery", new[] { "OfficeDeliveryInformationID" });
            DropIndex("dbo.MonthlyConsumptions", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.MonthlyConsumptions", new[] { "ID" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.LogicalOnTransits", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.LogicalOnTransits", new[] { "ID" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ItemType", new[] { "ApplicationUserID" });
            DropIndex("dbo.EndingBalances", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.EndingBalances", new[] { "ID" });
            DropIndex("dbo.Item", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.Item", new[] { "UnitID" });
            DropIndex("dbo.Item", new[] { "ItemTypeID" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.AvaliableOnStocks", new[] { "ID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Unit");
            DropTable("dbo.SalesDeliveryInformation");
            DropTable("dbo.SalesDelivery");
            DropTable("dbo.RowMaterialRepositery");
            DropTable("dbo.StoreRequests");
            DropTable("dbo.StoreRequestInformations");
            DropTable("dbo.RowDeliveryInformations");
            DropTable("dbo.RowDelivery");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Reports");
            DropTable("dbo.ReportInformations");
            DropTable("dbo.Recivieds");
            DropTable("dbo.ProductMaterialRepositories");
            DropTable("dbo.ProductlogicalAvaliable");
            DropTable("dbo.Sales");
            DropTable("dbo.ProductionOrder");
            DropTable("dbo.ProductionOrderInfo");
            DropTable("dbo.Customer");
            DropTable("dbo.SalesInformation");
            DropTable("dbo.Supplier");
            DropTable("dbo.StockInformation");
            DropTable("dbo.Stock");
            DropTable("dbo.PurchaseRequests");
            DropTable("dbo.PurchaseRequestInformations");
            DropTable("dbo.TransferInformation");
            DropTable("dbo.Transfer");
            DropTable("dbo.Product");
            DropTable("dbo.ProductInformations");
            DropTable("dbo.Store");
            DropTable("dbo.OfficeMaterialRequests");
            DropTable("dbo.OfficeMaterialRequestInformations");
            DropTable("dbo.OfficeDeliveryInformation");
            DropTable("dbo.OfficeDelivery");
            DropTable("dbo.MonthlyConsumptions");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.LogicalOnTransits");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ItemType");
            DropTable("dbo.EndingBalances");
            DropTable("dbo.Item");
            DropTable("dbo.AvaliableOnStocks");
        }
    }
}
