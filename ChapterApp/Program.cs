using System.Net;
using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => 
	options.UseSqlServer("Data source=DESKTOP-T52SIII;Database=ChapterApp;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

builder.Services.AddSingleton<PrintTree>();
builder.Services.AddSingleton<ChapterTracker>();

using var host = builder.Build();

await ChapterApp(host.Services);
await host.RunAsync();

return;

static async Task ChapterApp(IServiceProvider hostProvider)
{
	using var serviceScope = hostProvider.CreateScope();
	
	//var chapters = await JsonDataAccess.ReadData<Chapter>("chapters.json");
	var provider = serviceScope.ServiceProvider;
	var app = provider.GetRequiredService<ChapterTracker>();

	var test = provider.GetRequiredService<ApplicationDbContext>();
	
	await app.RunChapterTracker();

	//Skapa en migrering om det inte finns en databas, automatiskt.

	//Console.WriteLine("Enter a Chapter nr to start at:");

	//var chapterNrList = new List<int>();

	//var chapterNr = Extensions.InputCheck(0).First();
	//var currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == chapterNr);

	//currentChapter ??= new Chapter { ChapterId = chapterNr };
	//chapterNrList.Add(currentChapter.ChapterId);

	//do
	//{
	//	Console.WriteLine($"Current Chapter: {currentChapter.ChapterId}.");

	//	if (currentChapter.Links.Count < 1)
	//	{
	//		Console.WriteLine("Enter possible ways forward, separate by ','.");
	//		var linksToAdd = Extensions.InputCheck(currentChapter.ChapterId);

	//		if (linksToAdd[0] == 0)
	//			break;

	//		foreach (var linkInt in linksToAdd)
	//		{
	//			AddLinkToChapter(currentChapter, linkInt);
	//		}

	//		if (!dbContext.Chapters.Any(c => c.ChapterId == currentChapter.ChapterId))
	//			dbContext.Chapters.Add(currentChapter);

	//		dbContext.SaveChanges(); //Behövs denna ,testa att flytta till slutet?
	//	}

	//	Console.WriteLine($"Where do you want to continue to? {currentChapter}");
	//	var toGoto = Extensions.InputCheck(currentChapter.ChapterId).First();
	//	if (toGoto == 0)
	//		break;

	//	AddLinkToChapter(currentChapter, toGoto);

	//	//Sen sätta det nya kapitlet.
	//	currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoto);
	//	currentChapter ??= new Chapter { ChapterId = toGoto };

	//	dbContext.SaveChanges();

	//	chapterNrList.Add(currentChapter.ChapterId);
	//	Extensions.PrintChapterList(chapterNrList);

	//} while (true);


	//await JsonDataAccess.PrintData(dbContext.Chapters.Include(c => c.Links), "chapter.json");

	//return;

	//void AddLinkToChapter(Chapter chapter, int i)
	//{
	//	if (chapter.Links.All(c => c.LinkId != i) && dbContext.Links.Any(c => c.LinkId == i))
	//	{
	//		chapter.Links.Add(dbContext.Links.First(c => c.LinkId == i));
	//	}

	//	else if (dbContext.Links.All(c => c.LinkId != i))
	//	{
	//		chapter.Links.Add(new ChapterLink { LinkId = i });
	//	}
	//}
}