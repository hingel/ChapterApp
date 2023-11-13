using ChapterApp.Models;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class PrintTree
{
	private readonly ApplicationDbContext _dbContext;

	public PrintTree(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;

		MaxValue = _dbContext.Links.Count() - 1;
		PrintArray = new string[MaxValue + 2, MaxValue + 4]; //Första siffran är rad, Andra siffran är kolumn.
	}

	public string[,] PrintArray { get; private set; }
	public int CurrentRow { get; private set; } = 0;
	public int CurrentColumn { get; private set; } = 0;
	public HashSet<int> PrintedChapters { get; } = new();
	public int MaxValue { get; private set; }

	public async Task PrintTreeMethod()
	{
		var bottomChapter = await _dbContext.Chapters.Include(c => c.Links).FirstOrDefaultAsync();
		if (bottomChapter == null) return;

		//1. Först leta längst ner i trädet. Ta alltid första svaret. eller om det är samma tal som ett redan funnit nummer ta nästa
		bottomChapter = await FindBottomLeftChapter(bottomChapter.Links.First().LinkId);
		
		if (bottomChapter == null)
			return;

		var extraRemove = 1;

		while (PrintedChapters.Count <= MaxValue)
		{
			if (!PrintedChapters.Contains(bottomChapter.ChapterId))
				AddChapterToArray(bottomChapter);

			foreach (var link in bottomChapter.Links)
			{
				if (PrintedChapters.Contains(link.LinkId))
				{
					if (bottomChapter.Links.Count != 1)
					{
						CurrentColumn += 2;
						extraRemove = 2;
					}

					continue;
				}
				
				bottomChapter = await FindBottomLeftChapter(link.LinkId);
				AddChapterToArray(bottomChapter);
				break;
			}

			bottomChapter = await _dbContext.Chapters.Include(c => c.Links)
				.FirstOrDefaultAsync(c => c.Links.Any(l => l.LinkId.Equals(bottomChapter.ChapterId)));

			if (bottomChapter.Links.Count != 1)
				CurrentColumn += 1;

			CurrentRow -= 2;
		}

		await ConsolePrint(PrintArray);
	}

	private async Task<Chapter?> FindBottomLeftChapter(int chapterId)
	{
		var chapter = await _dbContext.Chapters.Include(c => c.Links).FirstOrDefaultAsync(c => c.ChapterId == chapterId);
		CurrentRow += 2;

		//Beroende på antal länkar ska den gå till vänster.
		if (chapter.Links.Count != 1)
			CurrentColumn -= 2;

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
			if (chapter.Links.Count != 1)
				CurrentColumn -= 2;

			chapter = nextChapter;
		}

		return null;
	}

	private void AddChapterToArray(Chapter currentChapter)
	{
		PrintedChapters.Add(currentChapter.ChapterId);

		if (CurrentColumn <= 0)
			CurrentColumn = 2;

		Console.Clear();
		Console.WriteLine($"Column: {CurrentColumn}, Row: {CurrentRow}");

		switch (currentChapter.Links.Count)
		{
			case 1:
			{
				PrintArray[CurrentRow, CurrentColumn] = $" {currentChapter.ChapterId} ";
				PrintArray[CurrentRow + 1 , CurrentColumn] = " | ";
				PrintArray[CurrentRow + 2, CurrentColumn] = currentChapter.Links.First().LinkId.ToString();
				break;	   
			}			   
			case 2:		   
			{
				PrintArray[CurrentRow, CurrentColumn] = $" {currentChapter.ChapterId} ";
				PrintArray[CurrentRow + 1, CurrentColumn - 1] = " / ";
				PrintArray[CurrentRow + 2, CurrentColumn - 2] = $" {currentChapter.Links.ElementAt(0).LinkId.ToString()} ";
				PrintArray[CurrentRow + 1, CurrentColumn + 1] = " \\ ";
				PrintArray[CurrentRow + 2, CurrentColumn + 2] = $" {currentChapter.Links.ElementAt(1).LinkId.ToString()} ";
				break;
			}
			case 3:
			{
				PrintArray[CurrentRow, CurrentColumn] = $" {currentChapter.ChapterId} ";
				PrintArray[CurrentRow + 1, CurrentColumn - 1] = " / ";
				PrintArray[CurrentRow + 2, CurrentColumn - 2] = $" {currentChapter.Links.ElementAt(0).LinkId.ToString()} ";
				PrintArray[CurrentRow + 1, CurrentColumn] = " | ";
				PrintArray[CurrentRow + 2, CurrentColumn] = $" {currentChapter.Links.ElementAt(1).LinkId.ToString()} ";
				PrintArray[CurrentRow + 1, CurrentColumn + 1] = " \\ ";
				PrintArray[CurrentRow + 2, CurrentColumn + 2] = $" {currentChapter.Links.ElementAt(2).LinkId.ToString()} ";
				break;
			}
			default: break;
		}

		ConsolePrint(PrintArray);
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