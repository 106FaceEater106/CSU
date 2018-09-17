using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace Dungeon
{
    public class DungeonTask
    {
        public static MoveDirection[] FindShortestPath(Map map)
        {
            var exitWay = BfsTask.FindPaths(map, map.InitialPosition, new[] { map.Exit }).FirstOrDefault();
            if (exitWay == null)
                return new MoveDirection[0];

            if (map.Chests.Any(box => exitWay.ToList().Contains(box)))
                return exitWay.ToList().SortingOut();

            var boxes = BfsTask.FindPaths(map, map.InitialPosition, map.Chests);
            var outcome = boxes.Select(box => Tuple.Create(
                box, BfsTask.FindPaths(map, box.Value, new[] { map.Exit }).FirstOrDefault()))
                .MinElement();
            if (outcome == null) return exitWay.ToList().SortingOut();

            return outcome.Item1.ToList().SortingOut().Concat(
                outcome.Item2.ToList().SortingOut())
                .ToArray();
        }
    }

    public static class Further
    {
        public static MoveDirection[] SortingOut(this List<Point> element)
        {
            var outcome = new List<MoveDirection>();
            if (element == null)
                return new MoveDirection[0];
            var elementValue = element.Count;

            for (var x = elementValue - 1; x > 0; x--)
            {
                outcome.Add(RequestWay(element[x], element[x - 1]));
            }
            return outcome.ToArray();
        }
        
        static MoveDirection RequestWay(Point firstPoint, Point secondPoint)
        {
            var newPoint = new Point(firstPoint.X - secondPoint.X, firstPoint.Y - secondPoint.Y);
            if (newPoint.Y == 1) return MoveDirection.Up;
            if (newPoint.Y == -1) return MoveDirection.Down;
            if (newPoint.X == -1) return MoveDirection.Right;
            if (newPoint.X == 1) return MoveDirection.Left;
            throw new ArgumentException();
        }
        
        public static Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>
            MinElement(this IEnumerable<Tuple<SinglyLinkedList<Point>, SinglyLinkedList<Point>>> items)
        {
            if (items.Count() == 0 || items.First().Item2 == null)
                return null;

            var less = int.MaxValue;
            var lesserItem = items.First();
            foreach (var element in items)
                if (element.Item1.Length + element.Item2.Length < less)
                {
                    less = element.Item1.Length + element.Item2.Length;
                    lesserItem = element;
                }
            return lesserItem;
        }
    }
}