namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingCustomerChange : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        MiddleName = c.String(),
                        StreetAddress = c.String(nullable: false),
                        Municipality = c.String(nullable: false),
                        Province = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        Email = c.String(),
                        dateOfBirth = c.DateTime(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(nullable: false),
                        UserCreatorId = c.Int(),
                        UserUpdatorId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OperatingUsers", t => t.UserCreatorId)
                .ForeignKey("dbo.OperatingUsers", t => t.UserUpdatorId)
                .Index(t => t.UserCreatorId)
                .Index(t => t.UserUpdatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Customers", new[] { "UserUpdatorId" });
            DropIndex("dbo.Customers", new[] { "UserCreatorId" });
            DropTable("dbo.Customers");
        }
    }
}
