namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserImageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OperatingUserImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        ContentType = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                        FileType = c.Int(nullable: false),
                        IsArchived = c.Boolean(nullable: false),
                        UserCreatorId = c.Int(),
                        UserUpdatorId = c.Int(),
                        DateAdded = c.DateTime(nullable: false),
                        DateArchived = c.DateTime(),
                        OperatingUser_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OperatingUsers", t => t.OperatingUser_ID)
                .ForeignKey("dbo.OperatingUsers", t => t.UserCreatorId)
                .ForeignKey("dbo.OperatingUsers", t => t.UserUpdatorId)
                .Index(t => t.UserCreatorId)
                .Index(t => t.UserUpdatorId)
                .Index(t => t.OperatingUser_ID);
            
            AddColumn("dbo.OperatingUsers", "ProfileImage_ID", c => c.Int());
            CreateIndex("dbo.OperatingUsers", "ProfileImage_ID");
            AddForeignKey("dbo.OperatingUsers", "ProfileImage_ID", "dbo.OperatingUserImages", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUsers", "ProfileImage_ID", "dbo.OperatingUserImages");
            DropForeignKey("dbo.OperatingUserImages", "UserUpdatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.OperatingUserImages", "UserCreatorId", "dbo.OperatingUsers");
            DropForeignKey("dbo.OperatingUserImages", "OperatingUser_ID", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "OperatingUser_ID" });
            DropIndex("dbo.OperatingUserImages", new[] { "UserUpdatorId" });
            DropIndex("dbo.OperatingUserImages", new[] { "UserCreatorId" });
            DropIndex("dbo.OperatingUsers", new[] { "ProfileImage_ID" });
            DropColumn("dbo.OperatingUsers", "ProfileImage_ID");
            DropTable("dbo.OperatingUserImages");
        }
    }
}
