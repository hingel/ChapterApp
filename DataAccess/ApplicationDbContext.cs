using ChapterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationDbContext : DbContext
{
	public virtual DbSet<Chapter> Chapters { get; set; }
	public virtual DbSet<ChapterLink> Links { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

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

		modelBuilder.Entity<ChapterLink>().HasKey(x => x.LinkId);
		modelBuilder.Entity<ChapterLink>(c => c.Property(ch => ch.LinkId).ValueGeneratedNever());
		//modelBuilder.Entity<Child>(ch => ch.Property(p => p.ParentsChapters).IsRequired());
	}
}