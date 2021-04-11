namespace Real_Estate_Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingDefaultImageColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ListingImages", "IsDefaultImage", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ListingImages", "IsDefaultImage");
        }
    }
}
