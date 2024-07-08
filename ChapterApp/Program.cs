using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

var builder = Host.CreateApplicationBuilder(args);


var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>().UseSqlite("Data Source=Test.db");
var dbContext = new ApplicationDbContext(optionsBuilder.Options);
await dbContext.Database.EnsureCreatedAsync();

builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite("Data Source=Test.db"));

builder.Services.AddScoped<PrintTree>();
builder.Services.AddScoped<ChapterTracker>();

using var host = builder.Build();

await ChapterApp(host.Services);
await host.RunAsync();

return;

static async Task ChapterApp(IServiceProvider hostProvider)
{
	using var serviceScope = hostProvider.CreateScope();

	var provider = serviceScope.ServiceProvider;
	var chapterTracker = provider.GetRequiredService<ChapterTracker>();
	var printTree = provider.GetRequiredService<PrintTree>();

	//var test = provider.GetRequiredService<ApplicationDbContext>();
	
	await chapterTracker.RunChapterTracker();

	//await printTree.PrintTreeMethod();
}