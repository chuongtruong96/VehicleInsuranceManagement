using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "claim_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    claim_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    policy_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    policy_start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    policy_end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    customer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    place_of_accident = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date_of_accident = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    insured_amount = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    claimable_amount = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_claim_details", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company_expenses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date_of_expenses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type_of_expense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    amount_of_expense = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_expenses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "NameRole",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name_role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameRole", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_information",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    vehicle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_owner_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    vehicle_body_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_engine_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_number = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    usage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    warranty_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    policy_type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_information", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_billing_policy",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    customer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    policy_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    customer_add_prove = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_phone_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    bill_no = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_rate = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_body_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_engine_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime", nullable: true),
                    amount = table.Column<decimal>(type: "numeric(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_billing_policy", x => x.id);
                    table.ForeignKey(
                        name: "company_billing_policy_user_id_AspNetUsers",
                        column: x => x.customer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "estimate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    estimate_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    customer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_phone_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_rate = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    VehicleBodyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleEngineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_number = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    vehicle_warranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_policy_type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverAge = table.Column<int>(type: "int", nullable: false),
                    DriverGender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Usage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AntiTheftDevice = table.Column<bool>(type: "bit", nullable: false),
                    SelectedCoverages = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DrivingHistory = table.Column<int>(type: "int", nullable: false),
                    MultiPolicy = table.Column<bool>(type: "bit", nullable: true),
                    SafeDriver = table.Column<bool>(type: "bit", nullable: true),
                    estimated_cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estimate", x => x.id);
                    table.ForeignKey(
                        name: "estimate_user_id_AspNetUsers",
                        column: x => x.customer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "insurance_process",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    customer_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_add = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_phone_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    policy_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    policy_date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    policy_duration = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_number = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_rate = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    vehicle_warranty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_body_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    vehicle_engine_number = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    customer_add_prove = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_insurance_process", x => x.id);
                    table.ForeignKey(
                        name: "insurance_process_user_id_AspNetUsers",
                        column: x => x.customer_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles_1", x => x.id);
                    table.ForeignKey(
                        name: "AspNetUserRoles__Role_id__NameRole_id",
                        column: x => x.RoleId,
                        principalTable: "NameRole",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Role_Permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    permissions_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    role_id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role_Permissions", x => x.id);
                    table.ForeignKey(
                        name: "Role_Permissions__Role_id__NameRole_id",
                        column: x => x.role_id,
                        principalTable: "NameRole",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Role_Permissions__role_id__AspNetRoles_Id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "([NormalizedName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "([NormalizedUserName] IS NOT NULL)");

            migrationBuilder.CreateIndex(
                name: "IX_company_billing_policy_customer_id",
                table: "company_billing_policy",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_estimate_customer_id",
                table: "estimate",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_insurance_process_customer_id",
                table: "insurance_process",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Permissions_role_id",
                table: "Role_Permissions",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "claim_details");

            migrationBuilder.DropTable(
                name: "company_billing_policy");

            migrationBuilder.DropTable(
                name: "company_expenses");

            migrationBuilder.DropTable(
                name: "estimate");

            migrationBuilder.DropTable(
                name: "insurance_process");

            migrationBuilder.DropTable(
                name: "Role_Permissions");

            migrationBuilder.DropTable(
                name: "vehicle_information");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "NameRole");

            migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}
