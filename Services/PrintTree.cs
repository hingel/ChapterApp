using System.Collections.Immutable;
using ChapterApp.Models;
using DataAccess;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class PrintTree
{
	private readonly ApplicationDbContext _dbContext;

	public PrintTree(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;

		MaxValue = _dbContext.Links.Count() - 1;
		PrintArray = new string[MaxValue, MaxValue + 2]; //Första siffran är rad, Andra siffran är kolumn.
	}

	public string[,] PrintArray { get; private set; }
	public int CurrentRow { get; private set; } = 0;
	public int CurrentColumn { get; private set; } = 0;
	public HashSet<int> PrintedChapters { get; } = new();
	public int MaxValue { get; private set; }

	public async Task PrintTreeMethod()
	{
		var sideSteps = 0;

		var bottomChapter = await _dbContext.Chapters.Include(c => c.Links).FirstOrDefaultAsync();
		if (bottomChapter == null) return;

		//1. Först leta längst ner i trädet. Ta alltid första svaret. eller om det är samma tal som ett redan funnit nummer ta nästa
		bottomChapter = await FindBottomLeftChapter(bottomChapter.Links.First().LinkId);
		
		if (bottomChapter == null)
			return;
		
		while (PrintedChapters.Count <= MaxValue)
		{
			if (!PrintedChapters.Contains(bottomChapter.ChapterId))
				AddChapterToArray(bottomChapter, CurrentRow - 2, CurrentColumn + bottomChapter.Links.Count > 2 ? 2 : 1);

			foreach (var link in bottomChapter.Links)
			{
				if (PrintedChapters.Contains(link.LinkId))
					continue;

				bottomChapter = await FindBottomLeftChapter(link.LinkId);
				AddChapterToArray(bottomChapter, CurrentRow - 2, CurrentColumn + bottomChapter.Links.Count > 2 ? 2 : 1);
				break;
			}

			bottomChapter = await _dbContext.Chapters.Include(c => c.Links)
				.FirstOrDefaultAsync(c => c.Links.Any(l => l.LinkId.Equals(bottomChapter.ChapterId)));
		}

		await ConsolePrint(PrintArray);
	}

	private void AddChapterToArray(Chapter currentChapter, int row, int column)
	{
		PrintedChapters.Add(currentChapter.ChapterId);

		switch (currentChapter.Links.Count)
		{
			case 1:
			{
				PrintArray[row, column] = " | ";
				PrintArray[row + 1, column] = currentChapter.Links.First().LinkId.ToString();
				break;	   
			}			   
			case 2:		   
			{			   
				PrintArray[row, column - 1] = " / ";
				PrintArray[row + 1, column - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString();
				PrintArray[row, column + 1] = " \\ ";
				PrintArray[row + 1, column + 1] = currentChapter.Links.ElementAt(1).LinkId.ToString();
				break;
			}
			case 3:
			{
				PrintArray[row, column - 1] = " / ";
				PrintArray[row + 1, column - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString();
				PrintArray[row, column] = "|";
				PrintArray[row + 1, column] = currentChapter.Links.ElementAt(1).LinkId.ToString();
				PrintArray[row, column + 1] = " \\ ";
				PrintArray[row + 1, column + 1] = currentChapter.Links.ElementAt(2).LinkId.ToString();
				break;
			}
			default: break;
		}
	}

	private async Task<Chapter?> FindBottomLeftChapter(int chapterId)
	{
		var chapter = await _dbContext.Chapters.Include(c => c.Links).FirstOrDefaultAsync(c => c.ChapterId == chapterId);

		while (chapter != null)
		{
			var nextChapter = await _dbContext.Chapters.Include(c => c.Links)
					.FirstOrDefaultAsync(c => c.ChapterId == chapter.Links.First().LinkId);
			
			if (nextChapter == null || PrintedChapters.Contains(nextChapter.ChapterId) || !nextChapter.Links.Any())
			{
				PrintedChapters.Add(chapter.Links.First().LinkId);
				return chapter;
			}

			CurrentRow += 2;
			chapter = nextChapter;
		}

		return null;
	}

	private async Task ConsolePrint(string[,] printArray)
	{
		Console.Clear();

		for (int i = 0; i < printArray.GetLength(0); i++)
		{
			for (int j = 0; j < printArray.GetLength(1); j++)
			{
				Console.Write(string.IsNullOrEmpty(printArray[i,j]) ? "   " : printArray[i, j] );
			}

			Console.WriteLine();
		}
	}
}