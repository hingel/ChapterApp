using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => 
	options.UseSqlServer("Data source=DESKTOP-T52SIII;Database=ChapterApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

builder.Services.AddScoped<PrintTree>();
builder.Services.AddScoped<ChapterTracker>();

using var host = builder.Build();

await ChapterApp(host.Services);
await host.RunAsync();

return;

static async Task ChapterApp(IServiceProvider hostProvider)
{
	using var serviceScope = hostProvider.CreateScope();
	
	//var chapters = await JsonDataAccess.ReadData<Chapter>("chapters.json");

	var provider = serviceScope.ServiceProvider;
	var chapterTracker = provider.GetRequiredService<ChapterTracker>();
	var printTree = provider.GetRequiredService<PrintTree>();

	var test = provider.GetRequiredService<ApplicationDbContext>();
	
	//await chapterTracker.RunChapterTracker();

	await printTree.PrintTreeMethod();


	//TODO:Skapa en migrering om det inte finns en databas, automatiskt.
}