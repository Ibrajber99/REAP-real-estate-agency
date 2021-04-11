namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCustomerAddressTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustomerAddresses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StreetAddress = c.String(nullable: false),
                        Municipality = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Customers", "AddressID", c => c.Int(nullable: false));
            CreateIndex("dbo.Customers", "AddressID");
            AddForeignKey("dbo.Customers", "AddressID", "dbo.CustomerAddresses", "ID", cascadeDelete: true);
            DropColumn("dbo.Customers", "StreetAddress");
            DropColumn("dbo.Customers", "Municipality");
            DropColumn("dbo.Customers", "Province");
            DropColumn("dbo.Customers", "PostalCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Customers", "PostalCode", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "Province", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "Municipality", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "StreetAddress", c => c.String(nullable: false));
            DropForeignKey("dbo.Customers", "AddressID", "dbo.CustomerAddresses");
            DropIndex("dbo.Customers", new[] { "AddressID" });
            DropColumn("dbo.Customers", "AddressID");
            DropTable("dbo.CustomerAddresses");
        }
    }
}
