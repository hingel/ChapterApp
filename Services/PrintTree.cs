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
	}

	public async Task PrintTreeMethod()
	{
		var maxValue = _dbContext.Chapters.Count();

		var printArray = new string[maxValue,maxValue + 2]; //Första siffran är rad, Andra siffran är kolumn.

		//Hela detta ska läggas i en while loop

		var currentChapter = _dbContext.Chapters.Include(c => c.Links).First();
		
		printArray[0, maxValue / 2] = currentChapter.ChapterId.ToString();

		switch (currentChapter.Links.Count)
		{
			case 1:
				{
					printArray[1, maxValue / 2] = " | ";
					printArray[0, maxValue / 2] = currentChapter.Links.First().LinkId.ToString(); //Detta element borde inte skrivas ut
					break;
				}
			case 2:
				{
					printArray[1, maxValue / 2 - 1] = " / ";
					printArray[0, maxValue / 2 - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString(); //Detta element borde inte skrivas ut
					printArray[1, maxValue / 2 + 1] = " \\ ";
					printArray[0, maxValue / 2 - 1] = currentChapter.Links.ElementAt(1).LinkId.ToString(); //Detta element borde inte skrivas ut
					break;
				}
			case 3:
				{
					printArray[1, maxValue / 2 - 1] = " / ";
					printArray[2, maxValue / 2 - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString(); //Detta element borde inte skrivas ut
					printArray[1, maxValue / 2] = "|"; 
					printArray[2, maxValue / 2] = currentChapter.Links.ElementAt(1).LinkId.ToString(); //Detta element borde inte skrivas ut 
					printArray[1, maxValue / 2 + 1] = " \\ ";
					printArray[2, maxValue / 2 + 1] = currentChapter.Links.ElementAt(2).LinkId.ToString(); //Detta element borde inte skrivas ut
					break;
				}
			default: break;
		}

		await ConsolePrint(printArray);
	}

	private async Task ConsolePrint(string[,] printArray)
	{
		Console.Clear();

		//TODO: kan köra med set cursor position istället.

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