using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NetCoreApi.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "系統識別碼")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "員工名稱"),
                    user_password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, comment: "員工密碼"),
                    user_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "使用者信箱"),
                    user_phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "連絡電話"),
                    user_remark = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "備註"),
                    enabled = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false, comment: "啟用狀態(Y/N/D)"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "建立時間"),
                    created_by = table.Column<int>(type: "int", nullable: false, comment: "建立人員"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "更新時間"),
                    updated_by = table.Column<int>(type: "int", nullable: false, comment: "更新人員"),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "刪除時間"),
                    deleted_by = table.Column<int>(type: "int", nullable: true, comment: "刪除人員")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                },
                comment: "會員資料表");

            migrationBuilder.CreateTable(
                name: "user_token",
                columns: table => new
                {
                    token_id = table.Column<int>(type: "int", nullable: false, comment: "系統識別碼")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "使用者識別碼"),
                    refresh_token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "使用者的刷新令牌"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "令牌生效時間"),
                    created_by_ip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "令牌生效位置"),
                    expires_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "令牌到期時間"),
                    replaced_by_token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, comment: "使用哪個令牌刷新"),
                    revoked_at = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "令牌撤銷時間"),
                    revoked_by_ip = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "令牌撤銷位置")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_token", x => x.token_id);
                    table.ForeignKey(
                        name: "FK_user_token_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_token_user_id",
                table: "user_token",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_token");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
