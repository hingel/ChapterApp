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

	public static int[] InputCheck(int notValidInt)
	{
		var stringArr = Console.ReadLine().Replace(" ", "").Split(',').ToArray();
		var intArr = new List<int>();

		foreach (var s in stringArr)
		{
			bool loop = true;

			while (loop)
			{
				loop = int.TryParse(s, out var testInt);

				if (loop && notValidInt != testInt)
				{
					intArr.Add(testInt);
					break;
				}
				
				Console.WriteLine("Enter a valid number");
			}
		}

		return intArr.ToArray();
	}
}