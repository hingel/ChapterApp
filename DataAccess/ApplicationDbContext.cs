using ChapterApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess;

public class ApplicationDbContext : DbContext
{
	public DbSet<Chapter> Chapters { get; set; } = null!;
	public DbSet<ChapterLink> Links { get; set; } = null!;

	public ApplicationDbContext() //DbContextOptions options) : base(options)
	{
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.UseSqlServer("Data source=DESKTOP-T52SIII;Database=ChapterApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<Chapter>().HasKey(x => x.ChapterId);
		modelBuilder.Entity<Chapter>(ch =>
		{
			ch.Property(p => p.ChapterId).ValueGeneratedNever();
		});
		modelBuilder.Entity<Chapter>().OwnsOne(c => c.Note).Property(p => p.Text).HasMaxLength(255);

		modelBuilder.Entity<ChapterLink>().HasKey(x => x.LinkId);
		modelBuilder.Entity<ChapterLink>(c => c.Property(ch => ch.LinkId).ValueGeneratedNever());
	}
}