using System.Collections.Generic;
using System.Drawing;

namespace Rivals
{
	public class RivalsTask
	{
		static void AroundMarks(Queue<OwnedLocation> turn, OwnedLocation presentStep)
		{
			for (var a = -1; a <= 1; a++)
			for (var b = -1; b <= 1; b++)
				if ((a == 0 || b == 0) && a != b)
				{
					turn.Enqueue(new OwnedLocation(presentStep.Owner, new Point(presentStep.Location.X + b, 
							presentStep.Location.Y + a), presentStep.Distance + 1));
				}
		}
		
		public static IEnumerable<OwnedLocation> AssignOwners(Map map)
		{
			var turn = new Queue<OwnedLocation>();
			var owned = new Dictionary<Point,OwnedLocation>();

			for (int a = 0; a < map.Players.Length; a++)
				turn.Enqueue(new OwnedLocation(a, map.Players[a], 0));

			while (turn.Count > 0)
			{
				var currentLocation = turn.Dequeue();
				if (owned.ContainsKey(currentLocation.Location) ||
				    !map.InBounds(currentLocation.Location) ||
				    map.Maze[currentLocation.Location.X, currentLocation.Location.Y] != MapCell.Empty)
					continue;

				owned[currentLocation.Location] = currentLocation;
				yield return currentLocation;
				AroundMarks(turn, currentLocation);
			}
		}
	}
}