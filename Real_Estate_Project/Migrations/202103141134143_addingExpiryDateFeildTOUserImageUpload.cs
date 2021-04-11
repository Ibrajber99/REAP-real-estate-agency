namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingExpiryDateFeildTOUserImageUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OperatingUserImages", "LicenseExpiryDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OperatingUserImages", "LicenseExpiryDate");
        }
    }
}
