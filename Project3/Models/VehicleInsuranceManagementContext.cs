using Microsoft.EntityFrameworkCore;

namespace Project3.Models;

public partial class VehicleInsuranceManagementContext : DbContext
{
    public VehicleInsuranceManagementContext()
    {
    }

    public VehicleInsuranceManagementContext(DbContextOptions<VehicleInsuranceManagementContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<ClaimDetail> ClaimDetails { get; set; }

    public virtual DbSet<CompanyBillingPolicy> CompanyBillingPolicies { get; set; }

    public virtual DbSet<CompanyExpense> CompanyExpenses { get; set; }

    public virtual DbSet<ContactUs> ContactUs { get; set; }

    public virtual DbSet<Estimate> Estimates { get; set; }

    public virtual DbSet<InsuranceProcess> InsuranceProcesses { get; set; }

    public virtual DbSet<InsuranceProduct> InsuranceProducts { get; set; }

    public virtual DbSet<NameRole> NameRoles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<VehicleInformation> VehicleInformations { get; set; }

    public virtual DbSet<VehiclePolicyType> VehiclePolicyTypes { get; set; }

    public virtual DbSet<VehicleWarranty> VehicleWarranties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=HUY;Initial Catalog=VehicleInsuranceManagement;Persist Security Info=True;User ID=sa;Password=123;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AspNetUserRoles_1");

            entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetUserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AspNetUserRoles__Role_id__NameRole_id");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<ClaimDetail>(entity =>
        {
            entity.ToTable("claim_details");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ClaimNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("claim_number");
            entity.Property(e => e.ClaimableAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("claimable_amount");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.DateOfAccident).HasColumnName("date_of_accident");
            entity.Property(e => e.InsuredAmount)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("insured_amount");
            entity.Property(e => e.PlaceOfAccident).HasColumnName("place_of_accident");
            entity.Property(e => e.PolicyEndDate).HasColumnName("policy_end_date");
            entity.Property(e => e.PolicyNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_number");
            entity.Property(e => e.PolicyStartDate).HasColumnName("policy_start_date");
        });

        modelBuilder.Entity<CompanyBillingPolicy>(entity =>
        {
            entity.ToTable("company_billing_policy");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.BillNo).HasColumnName("bill_no");
            entity.Property(e => e.CustomerAddProve).HasColumnName("customer_add_prove");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(450)
                .HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhoneNumber).HasColumnName("customer_phone_number");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.PolicyNumber).HasColumnName("policy_number");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleRate).HasColumnName("vehicle_rate");

            entity.HasOne(d => d.Customer).WithMany(p => p.CompanyBillingPolicies)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("company_billing_policy_user_id_AspNetUsers");
        });

        modelBuilder.Entity<CompanyExpense>(entity =>
        {
            entity.ToTable("company_expenses");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AmountOfExpense)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("amount_of_expense");
            entity.Property(e => e.DateOfExpenses).HasColumnName("date_of_expenses");
            entity.Property(e => e.TypeOfExpense).HasColumnName("type_of_expense");
        });

        modelBuilder.Entity<ContactUs>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Topic).HasMaxLength(50);
        });

        modelBuilder.Entity<Estimate>(entity =>
        {
            entity.HasKey(e => e.EstimateNumber).HasName("PK__Estimate__0B3C03911EA713CC");

            entity.ToTable("Estimate");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CustomerPhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.PolicyTypeId).HasColumnName("PolicyTypeID");
            entity.Property(e => e.VehicleModel)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.VehicleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.WarrantyId).HasColumnName("WarrantyID");

            entity.HasOne(d => d.PolicyType).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.PolicyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Estimate__Policy__540C7B00");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Estimate_vehicle_information");

            entity.HasOne(d => d.Warranty).WithMany(p => p.Estimates)
                .HasForeignKey(d => d.WarrantyId)
                .HasConstraintName("FK__Estimate__Warran__531856C7");
        });

        modelBuilder.Entity<InsuranceProcess>(entity =>
        {
            entity.ToTable("insurance_process");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerAdd).HasColumnName("customer_add");
            entity.Property(e => e.CustomerAddProve).HasColumnName("customer_add_prove");
            entity.Property(e => e.CustomerId)
                .HasMaxLength(450)
                .HasColumnName("customer_id");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.CustomerPhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("customer_phone_number");
            entity.Property(e => e.PolicyDate).HasColumnName("policy_date");
            entity.Property(e => e.PolicyDuration)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("policy_duration");
            entity.Property(e => e.PolicyNumber).HasColumnName("policy_number");
            entity.Property(e => e.PolicyTypeId).HasColumnName("PolicyTypeID");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleNumber)
                .HasColumnType("numeric(18, 0)")
                .HasColumnName("vehicle_number");
            entity.Property(e => e.VehicleRate).HasColumnName("vehicle_rate");
            entity.Property(e => e.VehicleVersion).HasColumnName("vehicle_version");
            entity.Property(e => e.VehicleWarranty).HasColumnName("vehicle_warranty");

            entity.HasOne(d => d.Customer).WithMany(p => p.InsuranceProcesses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("insurance_process_user_id_AspNetUsers");

            entity.HasOne(d => d.PolicyType).WithMany(p => p.InsuranceProcesses)
                .HasForeignKey(d => d.PolicyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_insurance_process_VehiclePolicyType");

            entity.HasOne(d => d.Vehicle).WithMany(p => p.InsuranceProcesses)
                .HasForeignKey(d => d.VehicleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_insurance_process_vehicle_information");

            entity.HasOne(d => d.Warranty).WithMany(p => p.InsuranceProcesses)
                .HasForeignKey(d => d.WarrantyId)
                .HasConstraintName("FK_insurance_process_VehicleWarranty");
        });

        modelBuilder.Entity<InsuranceProduct>(entity =>
        {
            entity.HasKey(e => e.InsuranceProductId).HasName("PK__Insuranc__FC79FCCF979D818B");

            entity.ToTable("InsuranceProduct");

            entity.HasOne(d => d.PolicyType).WithMany(p => p.InsuranceProducts)
                .HasForeignKey(d => d.PolicyTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insurance__Polic__02925FBF");

            entity.HasOne(d => d.Warranty).WithMany(p => p.InsuranceProducts)
                .HasForeignKey(d => d.WarrantyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Insurance__Warra__038683F8");
        });

        modelBuilder.Entity<NameRole>(entity =>
        {
            entity.ToTable("NameRole");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.NameRole1).HasColumnName("name_role");
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.ToTable("Role_Permissions");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PermissionsId)
                .HasMaxLength(450)
                .HasColumnName("permissions_id");
            entity.Property(e => e.RoleId)
                .HasMaxLength(450)
                .HasColumnName("role_id");
        });

        modelBuilder.Entity<VehicleInformation>(entity =>
        {
            entity.ToTable("vehicle_information");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.VehicleBodyNumber).HasColumnName("vehicle_body_number");
            entity.Property(e => e.VehicleEngineNumber).HasColumnName("vehicle_engine_number");
            entity.Property(e => e.VehicleModel).HasColumnName("vehicle_model");
            entity.Property(e => e.VehicleName).HasColumnName("vehicle_name");
            entity.Property(e => e.VehicleNumber).HasColumnName("vehicle_number");
            entity.Property(e => e.VehicleOwnerName).HasColumnName("vehicle_owner_name");
            entity.Property(e => e.VehicleRate).HasColumnName("vehicle_rate");
            entity.Property(e => e.VehicleVersion).HasColumnName("vehicle_version");
        });

        modelBuilder.Entity<VehiclePolicyType>(entity =>
        {
            entity.HasKey(e => e.PolicyTypeId).HasName("PK__VehicleP__90DE2D5EDDAC1272");

            entity.ToTable("VehiclePolicyType");

            entity.Property(e => e.PolicyTypeId).HasColumnName("PolicyTypeID");
            entity.Property(e => e.PolicyDetails)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PolicyName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VehicleWarranty>(entity =>
        {
            entity.HasKey(e => e.WarrantyId).HasName("PK__VehicleW__2ED318F3BB5B91D9");

            entity.ToTable("VehicleWarranty");

            entity.Property(e => e.WarrantyId)
                .ValueGeneratedNever()
                .HasColumnName("WarrantyID");
            entity.Property(e => e.WarrantyDetails)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.WarrantyDuration)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WarrantyType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
