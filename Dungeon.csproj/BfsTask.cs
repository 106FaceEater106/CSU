using System.Collections.Generic;
using System.Drawing;
 
namespace Dungeon
{
	public class BfsTask
	{
		public static IEnumerable<SinglyLinkedList<Point>> FindPaths(Map map, Point start, Point[] chests)
		{
			var tourist = new HashSet<Point>();
			var direction = new Dictionary<Point, SinglyLinkedList<Point>>();
			var turn = new Queue<Point>();
			tourist.Add(start);
			direction.Add(start, new SinglyLinkedList<Point>(start));
			turn.Enqueue(start);
            
			while (turn.Count > 0)
			{
				var checker = turn.Dequeue();
				if (checker.X >= map.Dungeon.GetLength(0) || checker.X < 0 || 
				    checker.Y >= map.Dungeon.GetLength(1) || checker.Y < 0 ||
				    map.Dungeon[checker.X, checker.Y] != MapCell.Empty) continue;
 
				for (var ac = -1; ac <= 1; ac++)
				for (var ab = -1; ab <= 1; ab++)
				{
					if (ab != 0 && ac != 0) continue;
					var checkForward = new Point {X = checker.X + ab, Y = checker.Y + ac}; 
					if (tourist.Contains(checkForward)) continue;
					turn.Enqueue(checkForward);
					tourist.Add(checkForward);
					direction.Add(checkForward, new SinglyLinkedList<Point>(checkForward, direction[checker]));
				}
			}
 
			foreach (var box in chests)
			{
				if (direction.ContainsKey(box)) yield return direction[box];
			}
		}
	}
}