namespace WebApiAssessment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCountryToDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Capital = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Countries");
        }
    }
}
