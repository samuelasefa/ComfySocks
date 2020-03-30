namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "Transfer_ID", "dbo.Transfer");
            DropIndex("dbo.Product", new[] { "Transfer_ID" });
            AddColumn("dbo.ProductInformation", "FPTNO", c => c.Int(nullable: false));
            CreateIndex("dbo.Product", "ItemID");
            AddForeignKey("dbo.Product", "ItemID", "dbo.Item", "ID");
            DropColumn("dbo.Product", "Transfer_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "Transfer_ID", c => c.Int());
            DropForeignKey("dbo.Product", "ItemID", "dbo.Item");
            DropIndex("dbo.Product", new[] { "ItemID" });
            DropColumn("dbo.ProductInformation", "FPTNO");
            CreateIndex("dbo.Product", "Transfer_ID");
            AddForeignKey("dbo.Product", "Transfer_ID", "dbo.Transfer", "ID");
        }
    }
}
