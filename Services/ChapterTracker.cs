using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Services;

public class ChapterTracker
{
	private readonly ApplicationDbContext _dbContext;
	private readonly PrintTree _print;

	public ChapterTracker(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task RunChapterTracker()
	{
		Console.WriteLine("Enter a Chapter nr to start at:");
		
		var chapterNrList = new List<int>();

		var chapterNr = Extensions.InputCheck(0).First();
		var currentChapter = _dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == chapterNr);

		currentChapter ??= new Chapter { ChapterId = chapterNr };

		chapterNrList.Add(currentChapter.ChapterId);

		do
		{
			//Console.WriteLine($"Current Chapter: {currentChapter.ChapterId}.");

			//if (currentChapter.Links.Count < 1)
			//{
			//	Console.WriteLine("Enter possible ways forward, separate by ','. Or press Enter to add Note to Chapter:");
			//	var linksToAdd = Extensions.InputCheck(currentChapter.ChapterId);

			//	if (linksToAdd[0] == 0)
			//		break;

			//	foreach (var linkInt in linksToAdd)
			//	{
			//		AddLinkToChapter(currentChapter, linkInt);
			//	}

			//	if (!_dbContext.Chapters.Any(c => c.ChapterId == currentChapter.ChapterId))
			//		_dbContext.Chapters.Add(currentChapter);

			//	_dbContext.SaveChanges();
			//}

			Console.WriteLine($"Where do you want to continue to? {currentChapter}");
			var toGoto = Extensions.InputCheck(currentChapter.ChapterId).First();
			if (toGoto == 0)
				break;

			Console.WriteLine($"Do you want to add a note to chapter {currentChapter.ChapterId}? Enter text or leave empty. Press enter to continue");

			var noteText = Console.ReadLine();
			_ = noteText.IsNullOrEmpty() ? currentChapter.Note.Text = string.Empty : currentChapter.Note.Text = noteText!;

			AddLinkToChapter(currentChapter, toGoto);

			//Sen sätta det nya kapitlet.
			currentChapter = _dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoto);
			currentChapter ??= new Chapter { ChapterId = toGoto };

			await _dbContext.SaveChangesAsync();

			chapterNrList.Add(currentChapter.ChapterId);
			Extensions.PrintChapterList(chapterNrList);

		} while (true);


		await JsonDataAccess.PrintData(_dbContext.Chapters.Include(c => c.Links), "chapter.json");

		return;

		void AddLinkToChapter(Chapter chapter, int i)
		{
			if (chapter.Links!.All(c => c.LinkId != i) && _dbContext.Links.Any(c => c.LinkId == i))
			{
				chapter.Links!.Add(_dbContext.Links.First(c => c.LinkId == i));
			}

			else if (_dbContext.Links.All(c => c.LinkId != i))
			{
				chapter.Links!.Add(new ChapterLink { LinkId = i });
			}
		}
	}
}