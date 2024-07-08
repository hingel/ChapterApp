using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ChapterTracker
{
	private readonly ApplicationDbContext _dbContext;
	private readonly PrintTree _print;

    public ChapterTracker(ApplicationDbContext dbContext, PrintTree tree)
	{
		_dbContext = dbContext;
		_print = tree;
	}

    public async Task RunChapterTracker()
	{
		Console.WriteLine("Enter a Chapter nr to start at:");
		
		var result = Extensions.InputCheck(0) ?? throw new Exception("Wrong input");
		var chapterNr = result[0];

        var currentChapter = _dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == chapterNr);

		currentChapter ??= new Chapter { ChapterId = chapterNr };

		var chapterNrList = new List<int>() { currentChapter.ChapterId };

		while(true)
		{
			Console.WriteLine($"Current Chapter: {currentChapter.ChapterId}.");

			if (!currentChapter.Links.Any())
			{
				Console.WriteLine("Enter possible ways forward, separate by ','. Or press Enter to add Note to Chapter:");
				var linksToAdd = Extensions.InputCheck(currentChapter.ChapterId);

				if (linksToAdd == null || linksToAdd[0] == 0)
					break;

				foreach (var linkInt in linksToAdd)
				{
					AddLinkToChapter(currentChapter, linkInt);
				}

				if (!_dbContext.Chapters.Any(c => c.ChapterId == currentChapter.ChapterId))
					_dbContext.Chapters.Add(currentChapter);

				await _dbContext.SaveChangesAsync(); //Behövs denna ,testa att flytta till slutet?
			}

			Console.WriteLine($"Where do you want to continue to? {currentChapter}");

			var toGoto = Extensions.InputCheck(currentChapter.ChapterId);
			if (toGoto == null || toGoto[0] == 0)
				break;

			Console.WriteLine($"Do you want to add a note to chapter {currentChapter.ChapterId}? Enter text or leave empty. Press enter to continue");

			var noteText = Console.ReadLine();
            _ = string.IsNullOrEmpty(noteText) ? currentChapter.Note.Text = string.Empty : currentChapter.Note.Text = noteText!;

			AddLinkToChapter(currentChapter, toGoto[0]);

			//Sen sätta det nya kapitlet.
			currentChapter = _dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == toGoto[0]);
			currentChapter ??= new Chapter { ChapterId = toGoto[0] };

			await _dbContext.SaveChangesAsync();

			chapterNrList.Add(currentChapter.ChapterId);
			Extensions.PrintChapterList(chapterNrList);

		};

		return;

		void AddLinkToChapter(Chapter chapter, int i)
		{
			if (chapter.Links.All(c => c.LinkId != i) && _dbContext.Links.Any(c => c.LinkId == i))
			{
				chapter.Links.Add(_dbContext.Links.First(c => c.LinkId == i));
			}

			else if (_dbContext.Links.All(c => c.LinkId != i))
			{
				chapter.Links.Add(new ChapterLink { LinkId = i });
			}
		}
	}
}