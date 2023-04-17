namespace bigschool.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendance",
                c => new
                {
                    CourseId = c.Int(nullable: false),
                    Attendee = c.String(nullable: false, maxLength: 128),
                })
                .PrimaryKey(t => new { t.CourseId, t.Attendee })
                .ForeignKey("dbo.Course", t => t.CourseId, cascadeDelete: true)
                .Index(t => t.CourseId);

            CreateTable(
                "dbo.Course",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    LecturerId = c.String(nullable: false, maxLength: 128),
                    Place = c.String(nullable: false, maxLength: 255),
                    DateTime = c.DateTime(nullable: false),
                    CategoryId = c.Int(nullable: false),
                    Fee = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .Index(t => t.CategoryId);

            CreateTable(
                "dbo.Category",
                c => new
                {
                    Id = c.Int(nullable: false),
                    Name = c.String(nullable: false, maxLength: 255),
                })
                .PrimaryKey(t => t.Id);



        }
        
        public override void Down()
        {
            
            DropForeignKey("dbo.Attendance", "CourseId", "dbo.Course");
            DropForeignKey("dbo.Course", "CategoryId", "dbo.Category");
            DropIndex("dbo.Course", new[] { "CategoryId" });
            DropIndex("dbo.Attendance", new[] { "CourseId" });
            DropTable("dbo.Category");
            DropTable("dbo.Course");
            DropTable("dbo.Attendance");
        }
    }
}
