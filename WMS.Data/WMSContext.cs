using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WMS.Data.Entities;

namespace WMS.Data
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
         
        public virtual DbSet<BatchEntries> BatchEntries { get; set; }
        public virtual DbSet<Batches> Batches { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<MaloCultureBrand> MaloCultureBrand { get; set; }
        public virtual DbSet<MaloCultureStyle> MaloCultureStyle { get; set; }
        public virtual DbSet<MaloCultures> MaloCultures { get; set; }
        public virtual DbSet<PicturesXref> PicturesXref { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Recipes> Recipes { get; set; }
        public virtual DbSet<Targets> Targets { get; set; }
        public virtual DbSet<UnitsOfMeasure> UnitsOfMeasure { get; set; }
        public virtual DbSet<Varieties> Varieties { get; set; }
        public virtual DbSet<YeastBrand> YeastBrand { get; set; }
        public virtual DbSet<YeastPair> YeastPair { get; set; }
        public virtual DbSet<YeastStyle> YeastStyle { get; set; }
        public virtual DbSet<Yeasts> Yeasts { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<BatchEntries>(entity =>
            {
                entity.Property(e => e.ActionDateTime).HasColumnType("datetime");

                entity.Property(e => e.Additions).HasMaxLength(4000);

                entity.Property(e => e.EntryDateTime).HasColumnType("datetime");

                entity.Property(e => e.PH).HasColumnName("pH");

                entity.Property(e => e.So2).HasColumnName("SO2");

                entity.Property(e => e.Ta).HasColumnName("TA");
            });

            modelBuilder.Entity<Batches>(entity =>
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

            modelBuilder.Entity<Categories>(entity =>
            {
                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.FileName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MaloCultureBrand>(entity =>
            {
                entity.Property(e => e.Brand).HasMaxLength(100);
            });

            modelBuilder.Entity<MaloCultureStyle>(entity =>
            {
                entity.Property(e => e.Style).HasMaxLength(100);
            });

            modelBuilder.Entity<MaloCultures>(entity =>
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

            modelBuilder.Entity<PicturesXref>(entity =>
            {
                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PicturesXref)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK_PicturesXref_Images");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.PicturesXref)
                    .HasForeignKey(d => d.RecipeId)
                    .HasConstraintName("FK_PicturesXref_Recipes");
            });

            modelBuilder.Entity<Ratings>(entity =>
            {
                entity.HasIndex(e => e.RecipeId)
                    .HasName("IX_RecipeId")
                    .IsUnique();

                entity.Property(e => e.OriginIp).HasColumnName("Origin_Ip");

                entity.Property(e => e.TotalValue).HasColumnName("Total_Value");

                entity.Property(e => e.TotalVotes).HasColumnName("Total_Votes");

                entity.HasOne(d => d.Recipe)
                    .WithOne(p => p.Ratings)
                    .HasForeignKey<Ratings>(d => d.RecipeId)
                    .HasConstraintName("FK_Ratings_Recipe");
            });

            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.Property(e => e.AddDate).HasColumnType("date");

                entity.Property(e => e.SubmittedBy).HasMaxLength(450);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

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

            modelBuilder.Entity<Targets>(entity =>
            {
                entity.Property(e => e.PH).HasColumnName("pH");

                entity.Property(e => e.Ta).HasColumnName("TA");

                entity.HasOne(d => d.EndSugarUom)
                    .WithMany(p => p.TargetsEndSugarUom)
                    .HasForeignKey(d => d.EndSugarUomId)
                    .HasConstraintName("FK_Targets_EndSugar_UnitsOfMeasure");

                entity.HasOne(d => d.StartSugarUom)
                    .WithMany(p => p.TargetsStartSugarUom)
                    .HasForeignKey(d => d.StartSugarUomId)
                    .HasConstraintName("FK_Targets_StartSugar_UnitsOfMeasure");

                entity.HasOne(d => d.TempUom)
                    .WithMany(p => p.TargetsTempUom)
                    .HasForeignKey(d => d.TempUomId)
                    .HasConstraintName("FK_Targets_Temp_UnitsOfMeasure");
            });

            modelBuilder.Entity<UnitsOfMeasure>(entity =>
            {
                entity.Property(e => e.Abbreviation).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(75);

                entity.Property(e => e.Subset).HasMaxLength(50);

                entity.Property(e => e.UnitOfMeasure).HasMaxLength(50);
            });

            modelBuilder.Entity<Varieties>(entity =>
            {
                entity.Property(e => e.Variety)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Varieties)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Varieties_Categories");
            });

            modelBuilder.Entity<YeastBrand>(entity =>
            {
                entity.Property(e => e.Brand).HasMaxLength(100);
            });

            modelBuilder.Entity<YeastPair>(entity =>
            {
                entity.HasIndex(e => new { e.Yeast, e.Category, e.Variety })
                    .HasName("IX_YeastCatVar")
                    .IsUnique();

                entity.Property(e => e.Note).HasColumnType("text");

                entity.HasOne(d => d.CategoryNavigation)
                    .WithMany(p => p.YeastPair)
                    .HasForeignKey(d => d.Category)
                    .HasConstraintName("FK_YeastPair_Categories");

                entity.HasOne(d => d.VarietyNavigation)
                    .WithMany(p => p.YeastPair)
                    .HasForeignKey(d => d.Variety)
                    .HasConstraintName("FK_YeastPair_Varieties");

                entity.HasOne(d => d.YeastNavigation)
                    .WithMany(p => p.YeastPair)
                    .HasForeignKey(d => d.Yeast)
                    .HasConstraintName("FK_YeastPair_Yeasts");
            });

            modelBuilder.Entity<YeastStyle>(entity =>
            {
                entity.Property(e => e.Style).HasMaxLength(100);
            });

            modelBuilder.Entity<Yeasts>(entity =>
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
