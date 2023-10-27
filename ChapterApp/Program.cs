using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Services;

var chapters = await JsonDataAccess.ReadData<Chapter>("chapters.json");

var dbContext = new ApplicationDbContext();

//Skapa en migrering om det inte finns en databas, automatiskt.

Console.WriteLine("Enter a Chapter nr to start at:");

var chapterNrList = new List<int>();

var chapterNr = Extensions.InputCheck(0).First();
var currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == chapterNr);

currentChapter ??= new Chapter {ChapterId = chapterNr};
chapterNrList.Add(currentChapter.ChapterId);

do
{
	Console.WriteLine($"Current Chapter: {currentChapter.ChapterId}.");

	if (currentChapter.Links.Count < 1)
	{
		Console.WriteLine("Enter possible ways forward, separate by ','.");
		var linksToAdd = Extensions.InputCheck(currentChapter.ChapterId);

		if (linksToAdd[0] == 0)
			break;
		
		foreach (var linkInt in linksToAdd)
		{
			AddLinkToChapter(currentChapter, linkInt);
		}

		if (!dbContext.Chapters.Any(c => c.ChapterId == currentChapter.ChapterId))
			dbContext.Chapters.Add(currentChapter);

		dbContext.SaveChanges(); //Behövs denna ,testa att flytta till slutet?
	}

	Console.WriteLine($"Where do you want to continue to? {currentChapter}");
	var toGoto = Extensions.InputCheck(currentChapter.ChapterId).First();
	if (toGoto == 0)
		break;

	AddLinkToChapter(currentChapter, toGoto);
	
	//Sen sätta det nya kapitlet.
	currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoto);
	currentChapter ??= new Chapter { ChapterId = toGoto };
	 
	dbContext.SaveChanges();

	chapterNrList.Add(currentChapter.ChapterId);
	Extensions.PrintChapterList(chapterNrList);

} while (true);


await JsonDataAccess.PrintData(dbContext.Chapters.Include(c => c.Links), "chapter.json");

void AddLinkToChapter(Chapter chapter, int i)
{
	if (chapter.Links.All(c => c.LinkId != i) && dbContext.Links.Any(c => c.LinkId == i))
	{
		chapter.Links.Add(dbContext.Links.First(c => c.LinkId == i));
	}

	else if (dbContext.Links.All(c => c.LinkId != i))
	{
		chapter.Links.Add(new ChapterLink { LinkId = i });
	}
}