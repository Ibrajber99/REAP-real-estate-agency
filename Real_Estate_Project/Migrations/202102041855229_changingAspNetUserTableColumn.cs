namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingAspNetUserTableColumn : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AspNetUsers", name: "userToRegister_ID", newName: "registeredUser_ID");
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_userToRegister_ID", newName: "IX_registeredUser_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_registeredUser_ID", newName: "IX_userToRegister_ID");
            RenameColumn(table: "dbo.AspNetUsers", name: "registeredUser_ID", newName: "userToRegister_ID");
        }
    }
}
