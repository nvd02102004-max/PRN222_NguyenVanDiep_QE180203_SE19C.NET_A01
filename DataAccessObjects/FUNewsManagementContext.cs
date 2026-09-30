using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DataAccessObjects
{
    public partial class FUNewsManagementContext : DbContext
    {
        public FUNewsManagementContext()
        {
        }

        public FUNewsManagementContext(DbContextOptions<FUNewsManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<NewsArticle> NewsArticles { get; set; } = null!;
        public virtual DbSet<NewsTag> NewsTags { get; set; } = null!;
        public virtual DbSet<SystemAccount> SystemAccounts { get; set; } = null!;
        public virtual DbSet<Tag> Tags { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = GetConnectionString();
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        public static string GetConnectionString()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            var connStr = config.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = "Server=localhost\\SQLEXPRESS;Database=FUNewsManagement;Integrated Security=True;TrustServerCertificate=True;";
            }
            return connStr;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId).HasName("PK_Category");

                entity.ToTable("Category");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CategoryDesciption).HasMaxLength(250);
                entity.Property(e => e.CategoryName).HasMaxLength(100);
                entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");

                entity.HasOne(d => d.ParentCategory)
                    .WithMany(p => p.InverseParentCategory)
                    .HasForeignKey(d => d.ParentCategoryId)
                    .HasConstraintName("FK_Category_Category");
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.HasKey(e => e.NewsArticleId).HasName("PK_NewsArticle");

                entity.ToTable("NewsArticle");

                entity.Property(e => e.NewsArticleId)
                    .HasMaxLength(20)
                    .HasColumnName("NewsArticleID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
                entity.Property(e => e.Headline).HasMaxLength(150);
                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
                entity.Property(e => e.NewsContent).HasMaxLength(4000);
                entity.Property(e => e.NewsSource).HasMaxLength(400);
                entity.Property(e => e.NewsTitle).HasMaxLength(400);
                entity.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.NewsArticles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsArticle_Category");

                entity.HasOne(d => d.CreatedBy)
                    .WithMany(p => p.NewsArticles)
                    .HasForeignKey(d => d.CreatedById)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsArticle_SystemAccount");
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.HasKey(e => new { e.NewsArticleId, e.TagId }).HasName("PK_NewsTag");

                entity.ToTable("NewsTag");

                entity.Property(e => e.NewsArticleId)
                    .HasMaxLength(20)
                    .HasColumnName("NewsArticleID");

                entity.Property(e => e.TagId).HasColumnName("TagID");

                entity.HasOne(d => d.NewsArticle)
                    .WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.NewsArticleId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsTag_NewsArticle");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.NewsTags)
                    .HasForeignKey(d => d.TagId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NewsTag_Tag");
            });

            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.HasKey(e => e.AccountId).HasName("PK_SystemAccount");

                entity.ToTable("SystemAccount");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountEmail).HasMaxLength(70);
                entity.Property(e => e.AccountName).HasMaxLength(100);
                entity.Property(e => e.AccountPassword).HasMaxLength(70);
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.HasKey(e => e.TagId).HasName("PK_HashTag");

                entity.ToTable("Tag");

                entity.Property(e => e.TagId)
                    .ValueGeneratedNever()
                    .HasColumnName("TagID");

                entity.Property(e => e.Note).HasMaxLength(400);
                entity.Property(e => e.TagName).HasMaxLength(50);
            });
        }
    }
}
