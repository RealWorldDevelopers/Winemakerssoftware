using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WMS.Data.SQL.Entities;

namespace WMS.Data.SQL
{
    public partial class WMSContext : DbContext
    {
        public WMSContext()
        {
        }

        public WMSContext(DbContextOptions<WMSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; } = null!;
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; } = null!;
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; } = null!;
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; } = null!;
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; } = null!;
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; } = null!;
        public virtual DbSet<Batch> Batches { get; set; } = null!;
        public virtual DbSet<BatchEntry> BatchEntries { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<LogDiagnostic> LogDiagnostics { get; set; } = null!;
        public virtual DbSet<LogError> LogErrors { get; set; } = null!;
        public virtual DbSet<LogPerf> LogPerves { get; set; } = null!;
        public virtual DbSet<LogUsage> LogUsages { get; set; } = null!;
        public virtual DbSet<MaloCulture> MaloCultures { get; set; } = null!;
        public virtual DbSet<MaloCultureBrand> MaloCultureBrands { get; set; } = null!;
        public virtual DbSet<MaloCultureStyle> MaloCultureStyles { get; set; } = null!;
        public virtual DbSet<PicturesXref> PicturesXrefs { get; set; } = null!;
        public virtual DbSet<Rating> Ratings { get; set; } = null!;
        public virtual DbSet<Recipe> Recipes { get; set; } = null!;
        public virtual DbSet<Target> Targets { get; set; } = null!;
        public virtual DbSet<UnitsOfMeasure> UnitsOfMeasures { get; set; } = null!;
        public virtual DbSet<Variety> Varieties { get; set; } = null!;
        public virtual DbSet<Yeast> Yeasts { get; set; } = null!;
        public virtual DbSet<YeastBrand> YeastBrands { get; set; } = null!;
        public virtual DbSet<YeastPair> YeastPairs { get; set; } = null!;
        public virtual DbSet<YeastStyle> YeastStyles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:realworlddevelopers.database.windows.net,1433;Initial Catalog=WMS;Persist Security Info=False;User ID=ka8kgj;Password=P!n0t_Gr1$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

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

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
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

                entity.HasMany(d => d.Roles)
                    .WithMany(p => p.Users)
                    .UsingEntity<Dictionary<string, object>>(
                        "AspNetUserRole",
                        l => l.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                        r => r.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                        j =>
                        {
                            j.HasKey("UserId", "RoleId");

                            j.ToTable("AspNetUserRoles");

                            j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                        });
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Batch>(entity =>
            {
                entity.Property(e => e.SubmittedBy).HasMaxLength(450);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.MaloCulture)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.MaloCultureId)
                    .HasConstraintName("FK_Batches_MaloCultures");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_Batches_Recipes");

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("FK_Batches_Targets");

                entity.HasOne(d => d.Variety)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.VarietyId)
                    .HasConstraintName("FK_Batches_Varieties");

                entity.HasOne(d => d.VolumeUom)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.VolumeUomId)
                    .HasConstraintName("FK_Batches_UnitsOfMeasure");

                entity.HasOne(d => d.Yeast)
                    .WithMany(p => p.Batches)
                    .HasForeignKey(d => d.YeastId)
                    .HasConstraintName("FK_Batches_Yeasts");
            });

            modelBuilder.Entity<BatchEntry>(entity =>
            {
                entity.Property(e => e.ActionDateTime).HasColumnType("datetime");

                entity.Property(e => e.Additions).HasMaxLength(4000);

                entity.Property(e => e.EntryDateTime).HasColumnType("datetime");

                entity.Property(e => e.PH).HasColumnName("pH");

                entity.Property(e => e.So2).HasColumnName("SO2");

                entity.Property(e => e.Ta).HasColumnName("TA");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Category1)
                    .HasMaxLength(200)
                    .HasColumnName("Category");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LogDiagnostic>(entity =>
            {
                entity.ToTable("Log_Diagnostic");

                entity.Property(e => e.CorrelationId).HasMaxLength(500);

                entity.Property(e => e.Hostname).HasMaxLength(500);

                entity.Property(e => e.Layer).HasMaxLength(500);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Product).HasMaxLength(500);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(500);

                entity.Property(e => e.UserName).HasMaxLength(500);
            });

            modelBuilder.Entity<LogError>(entity =>
            {
                entity.ToTable("Log_Error");

                entity.Property(e => e.ActionId).HasMaxLength(500);

                entity.Property(e => e.ActionName).HasMaxLength(500);

                entity.Property(e => e.Assembly).HasMaxLength(500);

                entity.Property(e => e.EnvironmentName).HasMaxLength(500);

                entity.Property(e => e.EnvironmentUserName).HasMaxLength(500);

                entity.Property(e => e.MachineName).HasMaxLength(500);

                entity.Property(e => e.ParentId).HasMaxLength(500);

                entity.Property(e => e.RequestId).HasMaxLength(500);

                entity.Property(e => e.RequestPath).HasMaxLength(500);

                entity.Property(e => e.SourceContext).HasMaxLength(500);

                entity.Property(e => e.SpanId).HasMaxLength(500);

                entity.Property(e => e.TimeStamp).HasColumnType("datetime");

                entity.Property(e => e.TraceId).HasMaxLength(500);

                entity.Property(e => e.Version).HasMaxLength(500);
            });

            modelBuilder.Entity<LogPerf>(entity =>
            {
                entity.ToTable("Log_Perf");

                entity.Property(e => e.CorrelationId).HasMaxLength(500);

                entity.Property(e => e.Hostname).HasMaxLength(500);

                entity.Property(e => e.Layer).HasMaxLength(500);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Product).HasMaxLength(500);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(500);

                entity.Property(e => e.UserName).HasMaxLength(500);
            });

            modelBuilder.Entity<LogUsage>(entity =>
            {
                entity.ToTable("Log_Usage");

                entity.Property(e => e.CorrelationId).HasMaxLength(500);

                entity.Property(e => e.Hostname).HasMaxLength(500);

                entity.Property(e => e.Layer).HasMaxLength(500);

                entity.Property(e => e.Location).HasMaxLength(500);

                entity.Property(e => e.Product).HasMaxLength(500);

                entity.Property(e => e.Timestamp).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(500);

                entity.Property(e => e.UserName).HasMaxLength(500);
            });

            modelBuilder.Entity<MaloCulture>(entity =>
            {
                entity.Property(e => e.PH).HasColumnName("pH");

                entity.Property(e => e.So2).HasColumnName("SO2");

                entity.Property(e => e.Trademark).HasMaxLength(100);

                entity.HasOne(d => d.BrandNavigation)
                    .WithMany(p => p.MaloCultures)
                    .HasForeignKey(d => d.Brand)
                    .HasConstraintName("FK_MaloCultures_MaloCultureBrand");

                entity.HasOne(d => d.StyleNavigation)
                    .WithMany(p => p.MaloCultures)
                    .HasForeignKey(d => d.Style)
                    .HasConstraintName("FK_MaloCultures_MaloCultureStyle");
            });

            modelBuilder.Entity<MaloCultureBrand>(entity =>
            {
                entity.ToTable("MaloCultureBrand");

                entity.Property(e => e.Brand).HasMaxLength(100);
            });

            modelBuilder.Entity<MaloCultureStyle>(entity =>
            {
                entity.ToTable("MaloCultureStyle");

                entity.Property(e => e.Style).HasMaxLength(100);
            });

            modelBuilder.Entity<PicturesXref>(entity =>
            {
                entity.ToTable("PicturesXref");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PicturesXrefs)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_PicturesXref_Images");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.PicturesXrefs)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_PicturesXref_Recipes");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasIndex(e => e.RecipeId, "IX_RecipeId")
                    .IsUnique();

                entity.Property(e => e.OriginIp).HasColumnName("Origin_Ip");

                entity.Property(e => e.TotalValue).HasColumnName("Total_Value");

                entity.Property(e => e.TotalVotes).HasColumnName("Total_Votes");

                entity.HasOne(d => d.Recipe)
                    .WithOne(p => p.Rating)
                    .HasForeignKey<Rating>(d => d.RecipeId)
                    .HasConstraintName("FK_Ratings_Recipe");
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.Property(e => e.AddDate).HasColumnType("date");

                entity.Property(e => e.SubmittedBy).HasMaxLength(450);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.Target)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.TargetId)
                    .HasConstraintName("FK_Recipes_Targets");

                entity.HasOne(d => d.Variety)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.VarietyId)
                    .HasConstraintName("FK_Recipes_Varieties");

                entity.HasOne(d => d.Yeast)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.YeastId)
                    .HasConstraintName("FK_Recipes_Yeasts");
            });

            modelBuilder.Entity<Target>(entity =>
            {
                entity.Property(e => e.PH).HasColumnName("pH");

                entity.Property(e => e.Ta).HasColumnName("TA");

                entity.HasOne(d => d.EndSugarUom)
                    .WithMany(p => p.TargetEndSugarUoms)
                    .HasForeignKey(d => d.EndSugarUomId)
                    .HasConstraintName("FK_Targets_EndSugar_UnitsOfMeasure");

                entity.HasOne(d => d.StartSugarUom)
                    .WithMany(p => p.TargetStartSugarUoms)
                    .HasForeignKey(d => d.StartSugarUomId)
                    .HasConstraintName("FK_Targets_StartSugar_UnitsOfMeasure");

                entity.HasOne(d => d.TempUom)
                    .WithMany(p => p.TargetTempUoms)
                    .HasForeignKey(d => d.TempUomId)
                    .HasConstraintName("FK_Targets_Temp_UnitsOfMeasure");
            });

            modelBuilder.Entity<UnitsOfMeasure>(entity =>
            {
                entity.ToTable("UnitsOfMeasure");

                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(75);

                entity.Property(e => e.Subset).HasMaxLength(50);

                entity.Property(e => e.UnitOfMeasure).HasMaxLength(50);
            });

            modelBuilder.Entity<Variety>(entity =>
            {
                entity.Property(e => e.Variety1)
                    .HasMaxLength(200)
                    .HasColumnName("Variety");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Varieties)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Varieties_Categories");
            });

            modelBuilder.Entity<Yeast>(entity =>
            {
                entity.Property(e => e.Trademark).HasMaxLength(100);

                entity.HasOne(d => d.BrandNavigation)
                    .WithMany(p => p.Yeasts)
                    .HasForeignKey(d => d.Brand)
                    .HasConstraintName("FK_Yeasts_YeastBrand");

                entity.HasOne(d => d.StyleNavigation)
                    .WithMany(p => p.Yeasts)
                    .HasForeignKey(d => d.Style)
                    .HasConstraintName("FK_Yeasts_YeastStyle");
            });

            modelBuilder.Entity<YeastBrand>(entity =>
            {
                entity.ToTable("YeastBrand");

                entity.Property(e => e.Brand).HasMaxLength(100);
            });

            modelBuilder.Entity<YeastPair>(entity =>
            {
                entity.ToTable("YeastPair");

                entity.HasIndex(e => new { e.Yeast, e.Category, e.Variety }, "IX_YeastCatVar")
                    .IsUnique();

                entity.Property(e => e.Note).HasColumnType("text");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.YeastPairs)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK_YeastPair_Categories");

                entity.HasOne(d => d.VarietyNavigation)
                    .WithMany(p => p.YeastPairs)
                    .HasForeignKey(d => d.Variety)
                    .HasConstraintName("FK_YeastPair_Varieties");

                entity.HasOne(d => d.YeastNavigation)
                    .WithMany(p => p.YeastPairs)
                    .HasForeignKey(d => d.Yeast)
                    .HasConstraintName("FK_YeastPair_Yeasts");
            });

            modelBuilder.Entity<YeastStyle>(entity =>
            {
                entity.ToTable("YeastStyle");

                entity.Property(e => e.Style).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
