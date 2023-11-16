using ChapterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationDbContext : DbContext
{
	public DbSet<Chapter> Chapters { get; set; } = null!;
	public DbSet<ChapterLink> Links { get; set; } = null!;

	public ApplicationDbContext(DbContextOptions options) : base(options)
	{
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