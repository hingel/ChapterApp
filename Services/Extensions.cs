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
			var checkResult = true;

			while (!checkResult)
			{
				checkResult = !int.TryParse(s, out var testInt);

				if (checkResult && notValidInt != testInt)
					intArr.Add(testInt);

				else
				{
					Console.WriteLine("Enter a valid number");
					checkResult = false;
				}
			}
		}

		return intArr.ToArray();
	}
}