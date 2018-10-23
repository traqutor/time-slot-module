namespace TimeSlotting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commodities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeliveryTimeSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Tons = c.Int(),
                        DeliveryDate = c.DateTime(nullable: false),
                        ContractId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        StatusTypeId = c.Int(nullable: false),
                        SupplierId = c.Int(nullable: false),
                        TimeSlotId = c.Int(nullable: false),
                        VehicleId = c.Int(nullable: false),
                        VendorId = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                        CommodityId = c.Int(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commodities", t => t.CommodityId, cascadeDelete: false)
                .ForeignKey("dbo.Contracts", t => t.ContractId, cascadeDelete: false)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: false)
                .ForeignKey("dbo.WebUsers", t => t.DriverId, cascadeDelete: false)
                .ForeignKey("dbo.Sites", t => t.SiteId, cascadeDelete: false)
                .ForeignKey("dbo.StatusTypes", t => t.StatusTypeId, cascadeDelete: false)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: false)
                .ForeignKey("dbo.TimeSlots", t => t.TimeSlotId, cascadeDelete: false)
                .ForeignKey("dbo.Vendors", t => t.VendorId, cascadeDelete: false)
                .Index(t => t.ContractId)
                .Index(t => t.CustomerId)
                .Index(t => t.SiteId)
                .Index(t => t.StatusTypeId)
                .Index(t => t.SupplierId)
                .Index(t => t.TimeSlotId)
                .Index(t => t.VehicleId)
                .Index(t => t.VendorId)
                .Index(t => t.DriverId)
                .Index(t => t.CommodityId);
            
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Fleets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Vehicles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Rego = c.String(),
                        CustomerId = c.Int(nullable: false),
                        FleetId = c.Int(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: false)
                .ForeignKey("dbo.Fleets", t => t.FleetId, cascadeDelete: false)
                .Index(t => t.CustomerId)
                .Index(t => t.FleetId);
            
            CreateTable(
                "dbo.VehicleDrivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VehicleId = c.Int(nullable: false),
                        WebUserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Vehicles", t => t.VehicleId, cascadeDelete: false)
                .ForeignKey("dbo.WebUsers", t => t.WebUserId, cascadeDelete: false)
                .Index(t => t.VehicleId)
                .Index(t => t.WebUserId);
            
            CreateTable(
                "dbo.WebUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ASPId = c.String(maxLength: 128),
                        CustomerId = c.Int(),
                        FleetId = c.Int(),
                        SiteId = c.Int(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .ForeignKey("dbo.Fleets", t => t.FleetId)
                .ForeignKey("dbo.Sites", t => t.SiteId)
                .ForeignKey("dbo.AspNetUsers", t => t.ASPId)
                .Index(t => t.ASPId)
                .Index(t => t.CustomerId)
                .Index(t => t.FleetId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.Sites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CustomerId = c.Int(nullable: false),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
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
                "dbo.StatusTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TimeSlots",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StartTime = c.Time(nullable: false, precision: 7),
                        EndTime = c.Time(nullable: false, precision: 7),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Vendors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        EntityStatus = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ModificationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                        ModifiedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WebUserId = c.Int(),
                        Recipient = c.String(),
                        Subject = c.String(),
                        Attachment = c.String(),
                        URL = c.String(),
                        Filename = c.String(),
                        SendError = c.String(),
                        SentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(),
                        StackTrace = c.String(),
                        URL = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        CreatedBy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            DropForeignKey("dbo.DeliveryTimeSlots", "VendorId", "dbo.Vendors");
            DropForeignKey("dbo.DeliveryTimeSlots", "TimeSlotId", "dbo.TimeSlots");
            DropForeignKey("dbo.DeliveryTimeSlots", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.DeliveryTimeSlots", "StatusTypeId", "dbo.StatusTypes");
            DropForeignKey("dbo.VehicleDrivers", "WebUserId", "dbo.WebUsers");
            DropForeignKey("dbo.WebUsers", "ASPId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.WebUsers", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.DeliveryTimeSlots", "SiteId", "dbo.Sites");
            DropForeignKey("dbo.Sites", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.WebUsers", "FleetId", "dbo.Fleets");
            DropForeignKey("dbo.DeliveryTimeSlots", "DriverId", "dbo.WebUsers");
            DropForeignKey("dbo.WebUsers", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.VehicleDrivers", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "FleetId", "dbo.Fleets");
            DropForeignKey("dbo.DeliveryTimeSlots", "VehicleId", "dbo.Vehicles");
            DropForeignKey("dbo.Vehicles", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Fleets", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DeliveryTimeSlots", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.DeliveryTimeSlots", "ContractId", "dbo.Contracts");
            DropForeignKey("dbo.DeliveryTimeSlots", "CommodityId", "dbo.Commodities");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Sites", new[] { "CustomerId" });
            DropIndex("dbo.WebUsers", new[] { "SiteId" });
            DropIndex("dbo.WebUsers", new[] { "FleetId" });
            DropIndex("dbo.WebUsers", new[] { "CustomerId" });
            DropIndex("dbo.WebUsers", new[] { "ASPId" });
            DropIndex("dbo.VehicleDrivers", new[] { "WebUserId" });
            DropIndex("dbo.VehicleDrivers", new[] { "VehicleId" });
            DropIndex("dbo.Vehicles", new[] { "FleetId" });
            DropIndex("dbo.Vehicles", new[] { "CustomerId" });
            DropIndex("dbo.Fleets", new[] { "CustomerId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "CommodityId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "DriverId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "VendorId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "VehicleId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "TimeSlotId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "SupplierId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "StatusTypeId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "SiteId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "CustomerId" });
            DropIndex("dbo.DeliveryTimeSlots", new[] { "ContractId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.EmailLogs");
            DropTable("dbo.Vendors");
            DropTable("dbo.TimeSlots");
            DropTable("dbo.Suppliers");
            DropTable("dbo.StatusTypes");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Sites");
            DropTable("dbo.WebUsers");
            DropTable("dbo.VehicleDrivers");
            DropTable("dbo.Vehicles");
            DropTable("dbo.Fleets");
            DropTable("dbo.Customers");
            DropTable("dbo.Contracts");
            DropTable("dbo.DeliveryTimeSlots");
            DropTable("dbo.Commodities");
        }
    }
}
