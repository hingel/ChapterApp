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

		
		var currentChapter = await _dbContext.Chapters.Include(c => c.Links).FirstOrDefaultAsync();

		var currentRow = 1;
		var currentColumn = maxValue / 2;

		printArray[currentRow - 1, currentColumn] = currentChapter.ChapterId.ToString(); //Sätter 1 i mitten.

		while (true)
		{

			if (currentChapter == null || !currentChapter.Links.Any()) break;

			//Leta upp underliggande kapitel 
			//var childChapters =
			//	_dbContext.Chapters.Include(c => c.Links)
			//		.Where(c => currentChapter.Links.Select(l => l.LinkId).Contains(c.ChapterId));

			//skicka de nya kapitlen, och arrayen till en extern metod som returnerar arrayen,
			//och det på något sätt vilket det nya aktuella kapitlet är.

			//vill skriva ut första kapitlet
			//currentChapter = childChapters.First();


			//Första siffran är rad, Andra siffran är kolumn.


			switch (currentChapter.Links.Count)
			{
				case 1:
				{
					printArray[currentRow, currentColumn] = " | ";
					printArray[currentRow + 1 , currentColumn] = currentChapter.Links.First().LinkId.ToString();
					break;
				}
				case 2:
				{
					printArray[currentRow, currentColumn - 1] = " / ";
					printArray[currentRow + 1, currentColumn - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString();
					printArray[currentRow, currentColumn + 1] = " \\ ";
					printArray[currentRow + 1, currentColumn + 1] = currentChapter.Links.ElementAt(1).LinkId.ToString();
					currentColumn -= 1;
						break;
				}
				case 3:
				{
					printArray[currentRow, currentColumn - 1] = " / ";
					printArray[currentRow + 1, currentColumn - 1] = currentChapter.Links.ElementAt(0).LinkId.ToString();
					printArray[currentRow, currentColumn] = "|";
					printArray[currentRow + 1, currentColumn] = currentChapter.Links.ElementAt(1).LinkId.ToString();
					printArray[currentRow, currentColumn + 1] = " \\ ";
					printArray[currentRow + 1, currentColumn + 1] = currentChapter.Links.ElementAt(2).LinkId.ToString();
					currentColumn -= 1;
					break;
				}
				default: break;
			}

			currentRow += 1;
			
			var resultTuple = FindNextCurrentChapter(printArray, currentRow);
			currentRow = resultTuple.Item1 + 1;
			currentColumn = resultTuple.Item2;
			currentChapter = resultTuple.Item3;


			//Här ska det sättas en ny variabel eller liknande istället.
			if (currentChapter == null)
				break;
		}

		await ConsolePrint(printArray);
	}

	private (int, int, Chapter?) FindNextCurrentChapter(string[,] printArray, int currentRow)
	{
		var currentcolumn = 0;
		var stringToFind = string.Empty;

		for (currentcolumn = 0; currentcolumn < printArray.Length - 2; currentcolumn++)
		{
			stringToFind = printArray[currentRow, currentcolumn]; //Hitta den första siffran i raden
			if (!string.IsNullOrEmpty(stringToFind)) break;
		}
		
		(int currentRow, int currentColumn, Chapter?) resultTuple = (currentRow, currentcolumn, _dbContext.Chapters.Include(c => c.Links).FirstOrDefault(c => c.ChapterId == int.Parse(stringToFind)));

		if (resultTuple.Item3.ChapterId == 1)
			resultTuple.Item3 = null;

		return resultTuple;
	}

	private async Task ConsolePrint(string[,] printArray)
	{
		Console.Clear();

		//TODO: kan köra med set cursor position istället. Spelar inte så stor roll.

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