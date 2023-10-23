using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var dbContext = new ApplicationDbContext();

//Skapa en migrering om det inte finns en databas, automatiskt.

Console.WriteLine("Enter a Chapter nr:");

var chapterNrList = new List<int>();

var chapterNr = Console.ReadLine();
var currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == int.Parse(chapterNr));
currentChapter ??= new Chapter {ChapterId = int.Parse(chapterNr) };
chapterNrList.Add(currentChapter.ChapterId);

do
{
	Console.WriteLine($"Current Chapter: {currentChapter.ChapterId}.");

	if (currentChapter.Links.Count < 1)
	{
		Console.WriteLine("Enter possible ways forward, separate by ','.");
		var linksToAdd = Services.Extensions.InputCheck();

		if (linksToAdd[0] == 0)
			break;
		
		foreach (var linkInt in linksToAdd)
		{
			AddLinkToChapter(currentChapter, linkInt);
		}

		if (!dbContext.Chapters.Any(c => c.ChapterId == currentChapter.ChapterId))
			dbContext.Chapters.Add(currentChapter);

		dbContext.SaveChanges(); //Behövs denna?
	}

	Console.WriteLine($"Where do you want to continue to? {currentChapter}");
	int toGoTo = Services.Extensions.InputCheck().First();

	if (toGoTo == 0)
		break;

	AddLinkToChapter(currentChapter, toGoTo);
	
	//Sen sätta det nya kapitlet.
	currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoTo);
	currentChapter ??= new Chapter { ChapterId = toGoTo };
	 
	dbContext.SaveChanges();

	chapterNrList.Add(currentChapter.ChapterId);
	Services.Extensions.PrintChapterList(chapterNrList);

} while (true); //Hitta något annat sätt att göra detta.

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



