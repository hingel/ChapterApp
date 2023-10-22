namespace Services;

public static class Extensions
{ 
	public static void PrintChapterList(List<int> list)
	{
		Console.Clear();
		Console.WriteLine("Current path:");

		Console.Write(list[0]);
		for (int i = 1; i < list.Count; i++)
		{
			Console.Write(" -> " + list[i]);
		}

		Console.WriteLine();
	}

	public static int[] InputCheck()
	{
		var stringArr = Console.ReadLine().Replace(" ", "").Split(',').ToArray();
		var intArr = new List<int>();

		foreach (var s in stringArr)
		{
			if (int.TryParse(s, out var testInt))
			{
				intArr.Add(testInt);
			}
		}

		return intArr.ToArray();
	}
}