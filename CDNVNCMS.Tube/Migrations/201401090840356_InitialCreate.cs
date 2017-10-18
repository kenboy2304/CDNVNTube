namespace CDNVNCMS.Tube.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShowOnHome = c.Boolean(nullable: false),
                        CategoryParentId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        MetaKeyword = c.String(),
                        MetaDescription = c.String(),
                        SEOName = c.String(),
                        Published = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryParentId);
            
            CreateTable(
                "dbo.Films",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(),
                        IsTrainler = c.Boolean(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        MetaKeyword = c.String(),
                        MetaDescription = c.String(),
                        SEOName = c.String(),
                        Published = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmParts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        YoutubeId = c.String(),
                        Image = c.String(),
                        Url = c.String(),
                        isError = c.Boolean(nullable: false),
                        FilmId = c.Int(nullable: false),
                        SEOName = c.String(),
                        Published = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedDate = c.DateTime(nullable: false),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.String(),
                        Keyword = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FilmToCatelogy",
                c => new
                    {
                        CategoryId = c.Int(nullable: false),
                        FilmId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.CategoryId, t.FilmId })
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Films", t => t.FilmId, cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilmToCatelogy", "FilmId", "dbo.Films");
            DropForeignKey("dbo.FilmToCatelogy", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.FilmParts", "FilmId", "dbo.Films");
            DropForeignKey("dbo.Categories", "CategoryParentId", "dbo.Categories");
            DropTable("dbo.FilmToCatelogy");
            DropTable("dbo.Settings");
            DropTable("dbo.FilmParts");
            DropTable("dbo.Films");
            DropTable("dbo.Categories");
        }
    }
}
