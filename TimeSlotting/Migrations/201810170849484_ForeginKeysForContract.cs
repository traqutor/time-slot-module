namespace TimeSlotting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeginKeysForContract : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commodities", "MaxTonsPerDay", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "VendorId", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "SupplierId", c => c.Int(nullable: false));
            AddColumn("dbo.Contracts", "CommodityId", c => c.Int(nullable: false));
            CreateIndex("dbo.Contracts", "VendorId");
            CreateIndex("dbo.Contracts", "SupplierId");
            CreateIndex("dbo.Contracts", "CommodityId");
            AddForeignKey("dbo.Contracts", "CommodityId", "dbo.Commodities", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Contracts", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: false);
            AddForeignKey("dbo.Contracts", "VendorId", "dbo.Vendors", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contracts", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.Contracts", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.Contracts", "CommodityId", "dbo.Commodities");
            DropIndex("dbo.Contracts", new[] { "CommodityId" });
            DropIndex("dbo.Contracts", new[] { "SupplierId" });
            DropIndex("dbo.Contracts", new[] { "VendorId" });
            DropColumn("dbo.Contracts", "CommodityId");
            DropColumn("dbo.Contracts", "SupplierId");
            DropColumn("dbo.Contracts", "VendorId");
            DropColumn("dbo.Commodities", "MaxTonsPerDay");
        }
    }
}
