using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews
{
	public static class ExtensionsTask
	{
		public static double Median(this IEnumerable<double> items)
		{
			var i = items.ToList();
			i.Sort();
			var iCount = i.Count;
			if (iCount == 0) throw new InvalidOperationException();
			if (iCount % 2 != 0)
				return i[iCount / 2];
			return i.Skip(iCount / 2 - 1).Take(2).Average();
		}

		public static IEnumerable<Tuple<T, T>> Bigrams<T>(this IEnumerable<T> items)
		{
			var itr = items.GetEnumerator();
			itr.MoveNext();
			var currentStep = itr.Current;

			while(itr.MoveNext())
			{
				yield return Tuple.Create(currentStep, itr.Current);
				currentStep = itr.Current;
			}
		}
	}
}