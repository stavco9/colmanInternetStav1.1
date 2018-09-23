using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace colmanInternetStav1._1.Models
{
    public partial class ColmanInternetiotContext : DbContext
    {
        public virtual DbSet<Branch> Branch { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Jewelry> Jewelry { get; set; }
        public virtual DbSet<Purchase> Purchase { get; set; }
        public virtual DbSet<Set> Set { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        // Unable to generate entity type for table 'dbo.CategoriesJewelry'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(@"Server=srvcolmaninternetiot.database.windows.net;Database=ColmanInternetiot;User ID=colman;Password=Cc123456;Trusted_Connection=False;Encrypt=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.ManagerId).HasColumnName("ManagerID");

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Branch)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK__Branch__ManagerI__5812160E");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Jewelry>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.SetId).HasColumnName("SetID");

                entity.HasOne(d => d.Set)
                    .WithMany(p => p.Jewelry)
                    .HasForeignKey(d => d.SetId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Jewelry__SetID__619B8048");
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.JewelryId).HasColumnName("JewelryID");

                entity.Property(e => e.Reference).HasMaxLength(255);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Jewelry)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.JewelryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK__Purchase__Jewelr__5FB337D6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Purchase)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Purchase__UserID__59063A47");
            });

            modelBuilder.Entity<Set>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(255);

                entity.Property(e => e.FName)
                    .HasColumnName("fName")
                    .HasMaxLength(255);

                entity.Property(e => e.Gender).HasMaxLength(255);

                entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");

                entity.Property(e => e.LName)
                    .HasColumnName("lName")
                    .HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.NameId)
                    .HasColumnName("NameID")
                    .HasMaxLength(255);
            });
        }
    }
}