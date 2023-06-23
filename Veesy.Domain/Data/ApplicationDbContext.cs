using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Veesy.Domain.Models;

namespace Veesy.Domain.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<MyUser> MyUsers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Format> Formats { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<MediaCategory> MediaCategories { get; set; }
    public DbSet<MediaFormat> MediaFormats { get; set; }
    public DbSet<MediaTag> MediaTags { get; set; }
    public DbSet<MyUserSector> MyUserSectors { get; set; }
    public DbSet<MyUserUsedSoftware> MyUserUsedSoftwares { get; set; }
    public DbSet<PortfolioMedia> PortfolioMedias { get; set; }
    public DbSet<Portofolio> Portofolios { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<UsedSoftware> UsedSoftwares { get; set; }
    
    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PortfolioMedia>().HasKey(a => new { a.PortfolioId, a.MediaFormatId });
        modelBuilder.Entity<MyUserUsedSoftware>().HasKey(a => new { a.MyUserId, a.UsedSoftwareId });
        modelBuilder.Entity<MyUserSector>().HasKey(a => new { a.MyUserId, a.SectorId });
        modelBuilder.Entity<MediaCategory>().HasKey(a => new { a.CategoryId, a.MediaId });
        modelBuilder.Entity<MediaFormat>().HasKey(a => new { a.FormatId, a.MediaId });
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}