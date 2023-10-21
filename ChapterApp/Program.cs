using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

var dbContext = new ApplicationDbContext();

//Skapa en migrering om det inte finns en databas, automatiskt.

Console.WriteLine("Enter a Chapter nr:");
var chapterNr = Console.ReadLine();
var currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == int.Parse(chapterNr));
currentChapter ??= new Chapter() {ChapterId = int.Parse(chapterNr) };

do
{
	if (currentChapter.Links.Count < 1)
	{
		Console.WriteLine("Enter possible ways forward, separate by ','.");
		var links = Console.ReadLine().Replace(" ", "").Split(',').ToArray();

		currentChapter.Links = links.Select(l => new ChapterLink { LinkId = int.Parse(l) }).ToArray();
		dbContext.Chapters.AddRange(currentChapter); //TODO: Måste kolla att kapitlet inte redan är inlagt.
		dbContext.SaveChanges();
	}
	
	Console.WriteLine($"Where do you want to continue to? {currentChapter}");
	int toGoTo = int.Parse(Console.ReadLine().Replace(" ", ""));

	//Om den aktuella sökvägen inte finns i kapitlet men att den finns i databasen
	if (currentChapter.Links.All(c => c.LinkId != toGoTo) && dbContext.Links.Any(c => c.LinkId == toGoTo))
	{
		//lägg till barnet till aktuellt kapitel
		currentChapter.Links.Add(dbContext.Links.First(c => c.LinkId == toGoTo));
		dbContext.SaveChanges();
	}
	//annars om länken inte finns alls. Då skapa ett nytt och lägg till det till kapitlet
	else if (dbContext.Links.All(c => c.LinkId != toGoTo))
	{
		currentChapter.Links.Add(new ChapterLink { LinkId = toGoTo });
		dbContext.SaveChanges();
	}

	//Sen sätta det ny akapitlet.
	currentChapter = dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoTo);
	currentChapter ??= new Chapter { ChapterId = toGoTo };

	dbContext.SaveChanges(); //Denna kanske inte alltid ska ligga o spara.

} while (currentChapter.Links != null);


dbContext.SaveChanges();