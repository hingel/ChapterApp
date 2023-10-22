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

	//Facktorisera detta nedan:
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

	//Med detta:
	Console.WriteLine($"Where do you want to continue to? {currentChapter}");
	int toGoTo = Services.Extensions.InputCheck().First();

	if (toGoTo == 0)
		break;

	AddLinkToChapter(currentChapter, toGoTo);

	////Om den aktuella sökvägen inte finns i kapitlet men att den finns i databasen
	//if (currentChapter.Links.All(c => c.LinkId != toGoTo) && dbContext.Links.Any(c => c.LinkId == toGoTo))
	//{
	//	//lägg till barnet till aktuellt kapitel
	//	currentChapter.Links.Add(dbContext.Links.First(c => c.LinkId == toGoTo));
	//}
	////annars om länken inte finns alls. Då skapa ett nytt och lägg till det till kapitlet
	//else if (dbContext.Links.All(c => c.LinkId != toGoTo))
	//{
	//	currentChapter.Links.Add(new ChapterLink { LinkId = toGoTo });
	//}

	//Sen sätta det ny akapitlet.
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



