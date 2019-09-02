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
         
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<PicturesXref> PicturesXref { get; set; }
        public virtual DbSet<Ratings> Ratings { get; set; }
        public virtual DbSet<Recipes> Recipes { get; set; }
        public virtual DbSet<Varieties> Varieties { get; set; }
        public virtual DbSet<YeastBrand> YeastBrand { get; set; }
        public virtual DbSet<YeastPair> YeastPair { get; set; }
        public virtual DbSet<Yeasts> Yeasts { get; set; }
        public virtual DbSet<YeastStyle> YeastStyle { get; set; }

      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          
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

            modelBuilder.Entity<PicturesXref>(entity =>
            {
                entity.HasOne(d => d.Image)
                    .WithMany(p => p.PicturesXref)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_PicturesXref_Images");

                entity.HasOne(d => d.Recipe)
                    .WithMany(p => p.PicturesXref)
                    .HasForeignKey(d => d.RecipeId)
                    .OnDelete(DeleteBehavior.SetNull)
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
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Ratings_Recipe");
            });

            modelBuilder.Entity<Recipes>(entity =>
            {
                entity.Property(e => e.AddDate).HasColumnType("date");

                entity.Property(e => e.SubmittedBy).HasMaxLength(450);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Variety)
                    .WithMany(p => p.Recipes)
                    .HasForeignKey(d => d.VarietyId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Recipes_Varieties");
            });

            modelBuilder.Entity<Varieties>(entity =>
            {
                entity.Property(e => e.Variety)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Varieties)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.SetNull)
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
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_YeastPair_Yeasts");
            });

            modelBuilder.Entity<Yeasts>(entity =>
            {
                entity.Property(e => e.Trademark).HasMaxLength(100);

                entity.HasOne(d => d.BrandNavigation)
                    .WithMany(p => p.Yeasts)
                    .HasForeignKey(d => d.Brand)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_Yeasts_YeastBrand");

                entity.HasOne(d => d.StyleNavigation)
                    .WithMany(p => p.Yeasts)
                    .HasForeignKey(d => d.Style)
                    .HasConstraintName("FK_Yeasts_YeastStyle");
            });

            modelBuilder.Entity<YeastStyle>(entity =>
            {
                entity.Property(e => e.Style).HasMaxLength(100);
            });
        }
    }
}
