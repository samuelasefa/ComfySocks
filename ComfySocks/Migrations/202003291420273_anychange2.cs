namespace ComfySocks.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class anychange2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "ProductCode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "ProductCode");
        }
    }
}
