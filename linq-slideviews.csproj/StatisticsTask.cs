using System.Linq;
using System.Collections.Generic;
 
namespace linq_slideviews
 {
 	public class StatisticsTask
 	{
 		public static double GetMedianTimePerSlide(List<VisitRecord> visits, SlideType slideType)
 		{
 			var manner = visits.ToList();
 			if (!visits.Any()) return 0.0; 
 			manner.Sort((x, y) =>
 			{
 				var first = x.UserId.CompareTo(y.UserId);
 				var second = x.DateTime.CompareTo(y.DateTime);
 				if (first != 0)
 					return first;
 				return second;
 			});
 			
 			var selected = manner.Bigrams()
				 .Where(i => i.Item1.SlideType == slideType &&
				        i.Item1.UserId == i.Item2.UserId &&
				        i.Item1.SlideId != i.Item2.SlideId)
 				.Select(i => i.Item2.DateTime.Subtract(i.Item1.DateTime)
 					.TotalMinutes)
 				.Where(j => 1.0 <= j && j <= 120.0);
 			if (selected.Any()) return selected.Median();
			 return 0.0;
 		}
 	}
 }
 
 
 
 