namespace Services;

public static class Extensions
{ 
	public static void PrintChapterList(List<int> list)
	{
		Console.Clear();
		Console.WriteLine("Current path:");

		Console.Write(list[0]);
		for (var i = 1; i < list.Count; i++)
		{
			Console.Write(" -> " + list[i]);
		}

		Console.WriteLine();
	}

	public static int[]? InputCheck(int notValidInt)
	{
		var stringArr = Console.ReadLine()!.Replace(" ", "").Split(',').ToArray(); //Ersätt med regex

		var intArr = stringArr.Where(s => int.TryParse(s, out var testInt) && testInt != notValidInt).Select(int.Parse);

		return intArr.ToArray();
	}
}