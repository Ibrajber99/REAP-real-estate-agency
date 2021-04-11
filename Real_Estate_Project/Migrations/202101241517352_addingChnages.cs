namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingChnages : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers");
            DropIndex("dbo.Customers", new[] { "UserCreatorId" });
            DropIndex("dbo.Customers", new[] { "UserUpdatorId" });
            DropTable("dbo.Customers");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.ID);
            
            CreateIndex("dbo.Customers", "UserUpdatorId");
            CreateIndex("dbo.Customers", "UserCreatorId");
            AddForeignKey("dbo.Customers", "UserUpdatorId", "dbo.OperatingUsers", "ID");
            AddForeignKey("dbo.Customers", "UserCreatorId", "dbo.OperatingUsers", "ID");
        }
    }
}
