namespace PetGrooming.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Species", "SpeciesName", c => c.String());
            DropColumn("dbo.Species", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Species", "Name", c => c.String());
            DropColumn("dbo.Species", "SpeciesName");
        }
    }
}
