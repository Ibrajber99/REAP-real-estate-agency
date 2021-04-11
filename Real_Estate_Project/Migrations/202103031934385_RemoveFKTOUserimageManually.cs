namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFKTOUserimageManually : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OperatingUserImages", "ImageRelatedUser", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "ImageRelatedUser" });
            RenameColumn(table: "dbo.OperatingUserImages", name: "ImageRelatedUser", newName: "OperatingUser_ID");
            AlterColumn("dbo.OperatingUserImages", "OperatingUser_ID", c => c.Int());
            CreateIndex("dbo.OperatingUserImages", "OperatingUser_ID");
            AddForeignKey("dbo.OperatingUserImages", "OperatingUser_ID", "dbo.OperatingUsers", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OperatingUserImages", "OperatingUser_ID", "dbo.OperatingUsers");
            DropIndex("dbo.OperatingUserImages", new[] { "OperatingUser_ID" });
            AlterColumn("dbo.OperatingUserImages", "OperatingUser_ID", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.OperatingUserImages", name: "OperatingUser_ID", newName: "ImageRelatedUser");
            CreateIndex("dbo.OperatingUserImages", "ImageRelatedUser");
            AddForeignKey("dbo.OperatingUserImages", "ImageRelatedUser", "dbo.OperatingUsers", "ID", cascadeDelete: true);
        }
    }
}
