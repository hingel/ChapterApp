using System.Text.Json;
using System.Text.Json.Serialization;
using ChapterApp.Models;

namespace Services;

public static class JsonDataAccess
{
	public static async Task PrintData<T>(IEnumerable<T> data, string fileName) where T : class
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), fileName); // Environment.GetFolderPath(Environment.SpecialFolder.Desktop), fileName);

		await using StreamWriter sw = new StreamWriter(path);

		var jsOptions = new JsonSerializerOptions
		{
			WriteIndented = true,
			ReferenceHandler = ReferenceHandler.IgnoreCycles
		};

		var jsonString = JsonSerializer.Serialize(data, jsOptions);
		await sw.WriteLineAsync(jsonString);
	}


	public static async Task<List<T>> ReadData<T>(string fileName)
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

		try
		{
			using StreamReader sr = new StreamReader(path);
			var jsonString = await sr.ReadToEndAsync();
			var data = JsonSerializer.Deserialize<List<T>>(jsonString);
			return data;
		}
		catch
		{
			return null;
		}
	}
}

