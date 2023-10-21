using System.ComponentModel.DataAnnotations;

namespace ChapterApp.Models;

public class Chapter
{
	public int ChapterId { get; set; }
	public virtual ICollection<ChapterLink>? Links { get; set; } = new HashSet<ChapterLink>();

	public override string ToString()
	{
		var toReturn = string.Format("Chapters to go to: " + (Links.Count > 0
			? Links
				.Select(c => c.LinkId.ToString())
				.Aggregate((link, link2) => $"{link}, {link2}") : "."));
		return toReturn;
	}
}

public class ChapterLink
{
	public int LinkId { get; set; } //Detta id är kapitel.
	public virtual ICollection<Chapter> ParentsChapters { get; set; } = new HashSet<Chapter>();
}