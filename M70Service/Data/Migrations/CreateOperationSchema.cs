using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace M70Service.Data.Migrations
{
    /* Class is used to create the tables used for operation recording*/
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("CreateOperationSchema")]
    public class CreateOperationSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                name: "OperationHistory",
                columns: table => new {
                    JobID = table.Column<Guid>(nullable: false),
                    JobType = table.Column<int>(nullable: false),
                    BranchID = table.Column<Guid>(nullable: true),
                    ClientName = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Cost = table.Column<float>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_OperationHistory", x => x.JobID);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentLog",
                columns: table => new {
                    EquipmentID = table.Column<Guid>(nullable: false),
                    JobID = table.Column<Guid>(nullable: false),
                    Equipment = table.Column<int>(nullable: true),
                    Amount = table.Column<float>(nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_EquipmentLog", x => x.EquipmentID);
                    table.ForeignKey(name: "FK_JobRef", column: x => x.JobID, principalTable: "OperationHistory",
                                    principalColumn: "JobID", onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(name: "OperationHistory");
            migrationBuilder.DropTable(name: "EquipmentLog");
        }
    }
}
