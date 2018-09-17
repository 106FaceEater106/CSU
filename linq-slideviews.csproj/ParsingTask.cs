using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public class ParsingTask
	{
		public static IDictionary<int, SlideRecord> ParseSlideRecords(IEnumerable<string> lines)
		{
			return lines.Skip(1)
				.Select(i => i.WriteToSlide())
				.Where(j => j != null)
				.ToDictionary(j => j.SlideId, j => j);
		}

		public static IEnumerable<VisitRecord> ParseVisitRecords(
			IEnumerable<string> lines, IDictionary<int, SlideRecord> slides)
		{
			return lines.Skip(1)
				.Select(s => s.UsingVisit(slides));
		}
	}
	
	static class Formatter
	{
		public static VisitRecord UsingVisit(this string str,
			IDictionary<int, SlideRecord> slides)
		{
			DateTime currentStatus;
			int counterFirst;
			int counterSecond;
			var spl = str.IsDivsed();
			if (int.TryParse(spl[0], out counterFirst) &&
			    int.TryParse(spl[1], out counterSecond) &&
			    spl.Count() == 4 &&
			    slides.ContainsKey(counterSecond) &&
			    DateTime.TryParse(spl[2] + 'T' + spl[3], out currentStatus))
				return new VisitRecord(counterFirst, counterSecond, currentStatus, slides[counterSecond].SlideType);
			throw new FormatException("Wrong line [" + str + "]");
		}
		

		public static SlideRecord WriteToSlide(this string str)
		{
			var i = str.IsDivsed();
			int writeCount;
			SlideType type;
			if (3 == i.Count() &&
			    Enum.TryParse(i[1], true, out type) &&
			    int.TryParse(i[0], out writeCount))
				return new SlideRecord(writeCount, type, i[2]);
			return null;
		}
		
		public static string[] IsDivsed(this string str) => str.Split(';');
	}
}