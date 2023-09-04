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
    public DbSet<MyUserSkill> MyUserSkills { get; set; }
    public DbSet<Skill> Skills { get; set; }
    public DbSet<PortfolioMedia> PortfolioMedias { get; set; }
    public DbSet<Portfolio> Portfolios { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<UsedSoftware> UsedSoftwares { get; set; }
    public DbSet<PortfolioSector> PortfolioSectors { get; set; }
    public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
    public DbSet<TmpMedia> TmpMedias { get; set; }
    public DbSet<CategoryWork> CategoriesWork { get; set; }
    public DbSet<MyUserCategoryWork> MyUserCategoriesWork { get; set; }
    public DbSet<MyUserInfoToShow> MyUserInfosToShow { get; set; }
    public DbSet<MyUserLanguageSpoken> MyUserLanguagesSpoken { get; set; }
    public DbSet<LanguageSpoken> LanguagesSpoken { get; set; }
    public DbSet<InfoToShow> InfosToShow { get; set; }

    #region Required
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PortfolioMedia>().HasKey(a => new { a.PortfolioId, a.MediaFormatId });
        modelBuilder.Entity<PortfolioMedia>().HasOne(a => a.Portfolio).WithMany(a => a.PortfolioMedias).OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<PortfolioMedia>().HasOne(a => a.MediaFormat).WithMany(a => a.PortfolioMedias);
        modelBuilder.Entity<MyUserUsedSoftware>().HasKey(a => new { a.MyUserId, a.UsedSoftwareId });
        modelBuilder.Entity<MyUserSkill>().HasKey(a => new {a.Id, a.MyUserId, a.SkillId });
        modelBuilder.Entity<MyUserCategoryWork>().HasKey(a => new {a.MyUserId, a.CategoryWorkId });
        modelBuilder.Entity<MyUserInfoToShow>().HasKey(a => new {a.MyUserId, a.InfoToShowId });
        modelBuilder.Entity<MyUserLanguageSpoken>().HasKey(a => new {a.MyUserId, a.LanguageSpokenId });
        modelBuilder.Entity<MyUserSector>().HasKey(a => new { a.MyUserId, a.SectorId });
        modelBuilder.Entity<MediaCategory>().HasKey(a => new { a.CategoryId, a.MediaId });
        modelBuilder.Entity<PortfolioSector>().HasKey(a => new { a.PorfolioId, a.SectorId });
        base.OnModelCreating(modelBuilder);
    }
    #endregion
}