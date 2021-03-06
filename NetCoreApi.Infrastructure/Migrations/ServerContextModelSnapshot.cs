// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetCoreApi.Infrastructure.Contexts;

namespace NetCoreApi.Infrastructure.Migrations
{
    [DbContext(typeof(ServerContext))]
    partial class ServerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("NetCoreApi.Infrastructure.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .HasComment("系統識別碼")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at")
                        .HasComment("建立時間");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("int")
                        .HasColumnName("created_by")
                        .HasComment("建立人員");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("deleted_at")
                        .HasComment("刪除時間");

                    b.Property<int?>("DeletedBy")
                        .HasColumnType("int")
                        .HasColumnName("deleted_by")
                        .HasComment("刪除人員");

                    b.Property<string>("Enabled")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)")
                        .HasColumnName("enabled")
                        .HasComment("啟用狀態(Y/N/D)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("updated_at")
                        .HasComment("更新時間");

                    b.Property<int>("UpdatedBy")
                        .HasColumnType("int")
                        .HasColumnName("updated_by")
                        .HasComment("更新人員");

                    b.Property<string>("UserEmail")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("user_email")
                        .HasComment("使用者信箱");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("user_name")
                        .HasComment("員工名稱");

                    b.Property<string>("UserPassword")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)")
                        .HasColumnName("user_password")
                        .HasComment("員工密碼");

                    b.Property<string>("UserPhone")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("user_phone")
                        .HasComment("連絡電話");

                    b.Property<string>("UserRemark")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("user_remark")
                        .HasComment("備註");

                    b.HasKey("UserId");

                    b.ToTable("user");

                    b
                        .HasComment("會員資料表");
                });

            modelBuilder.Entity("NetCoreApi.Infrastructure.Models.UserToken", b =>
                {
                    b.Property<int>("TokenId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("token_id")
                        .HasComment("系統識別碼")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("created_at")
                        .HasComment("令牌生效時間");

                    b.Property<string>("CreatedByIp")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("created_by_ip")
                        .HasComment("令牌生效位置");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("expires_at")
                        .HasComment("令牌到期時間");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("refresh_token")
                        .HasComment("使用者的刷新令牌");

                    b.Property<string>("ReplacedByToken")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("replaced_by_token")
                        .HasComment("使用哪個令牌刷新");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2")
                        .HasColumnName("revoked_at")
                        .HasComment("令牌撤銷時間");

                    b.Property<string>("RevokedByIp")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .HasColumnName("revoked_by_ip")
                        .HasComment("令牌撤銷位置");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("user_id")
                        .HasComment("使用者識別碼");

                    b.HasKey("TokenId");

                    b.HasIndex("UserId");

                    b.ToTable("user_token");
                });

            modelBuilder.Entity("NetCoreApi.Infrastructure.Models.UserToken", b =>
                {
                    b.HasOne("NetCoreApi.Infrastructure.Models.User", null)
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NetCoreApi.Infrastructure.Models.User", b =>
                {
                    b.Navigation("Tokens");
                });
#pragma warning restore 612, 618
        }
    }
}
