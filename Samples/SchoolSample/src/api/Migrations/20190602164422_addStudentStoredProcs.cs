using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolApi.Migrations
{
    public partial class addStudentStoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var deleteSp = @"CREATE PROCEDURE [dbo].[DeleteStudent]
                        @studentId INT
                    AS
                    BEGIN
                        DELETE FROM [dbo].[Students] WHERE [Id] = @studentId;
                    END;";
            migrationBuilder.Sql(deleteSp);

            var getSp = @"CREATE PROCEDURE [dbo].[GetStudent]
                            @studentId INT
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                            SELECT [Id] [StudentId], [Name] [StudentName] FROM [Students] WHERE [Id] = @studentId;
                            SET NOCOUNT OFF;
                        END;";
            migrationBuilder.Sql(getSp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var dropDel = "DROP PROCEDURE [DeleteStudent];";
            migrationBuilder.Sql(dropDel);

            var dropGet = "DROP PROCEDURE [GetStudent]";
            migrationBuilder.Sql(dropGet);
        }
    }
}
