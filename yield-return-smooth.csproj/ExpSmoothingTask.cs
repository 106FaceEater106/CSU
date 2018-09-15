using System.Collections.Generic;
 
namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			var isTop = true;
			double beforeIt = 0;
			foreach (var e in data)
			{
				var value = new DataPoint();
				value.X = e.X;
				value.OriginalY = e.OriginalY;
				value.MaxY = e.MaxY;
				value.AvgSmoothedY = e.AvgSmoothedY;
				if(!isTop)
				{
					beforeIt = alpha * e.OriginalY + (1 - alpha) * beforeIt;
				}
				else
				{
					isTop = false;
					beforeIt = e.OriginalY;
				}
				value.ExpSmoothedY = beforeIt;
				yield return value;
			}
		}
	}
}