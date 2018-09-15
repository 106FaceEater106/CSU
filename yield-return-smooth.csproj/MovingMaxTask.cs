using System.Collections.Generic;
 
namespace yield
{
	public static class MovingMaxTask
	{
		public static IEnumerable<DataPoint> MovingMax(this IEnumerable<DataPoint> data, int windowWidth)
		{
			var stanceFwd = new Queue<double>();
			var stance = new LinkedList<double>();
			foreach (var position in data)
			{
				stanceFwd.Enqueue(position.X);
				if (stanceFwd.Count > windowWidth && stance.First.Value <= stanceFwd.Dequeue())
				{
					stance.RemoveFirst();
					stance.RemoveFirst();
				}
				while (stance.Count != 0 && stance.Last.Value < position.OriginalY)
				{
					stance.RemoveLast();
					stance.RemoveLast();
				}
				stance.AddLast(position.X);
				stance.AddLast(position.OriginalY);
				position.MaxY = stance.First.Next.Value;
				yield return position;
			}
		}
	}
}