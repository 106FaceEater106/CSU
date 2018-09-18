using System.Collections.Generic;
using System.Drawing;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class GreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var closedBox = new HashSet<Point>(state.Chests);
            if (state.Chests.Count < state.Goal)
            {
                return new List<Point>();
            }
            
            var openedBox = 0;
            var ongoingLevel = state.InitialEnergy;
            var ongoingStep = state.Position;
            var way = new List<Point>();

            while (openedBox < state.Goal)
            {
                int decreasedLevel;

                var toNextBox = WayToBox(closedBox, ongoingStep, state, out decreasedLevel);

                ongoingLevel -= decreasedLevel;
                if (toNextBox == null || ongoingLevel < 0)
                {
                    return new List<Point>();
                }

                way.AddRange(toNextBox);

                if (toNextBox.Count > 0)
                {
                    ongoingStep = toNextBox[toNextBox.Count - 1];
                }

                closedBox.Remove(ongoingStep);
                openedBox++;
            }
            return way;
        }
        
        private List<Point> WayToBox(HashSet<Point> boxes,
            Point begin, State state, out int decreasedLevel)
        {
            Point lastPoint;
            var check = new Dictionary<Point, MovingData>();
            var mileage = new Dictionary<Point, MovingData>();
            check.Add(begin, new MovingData(begin, 0));
            while (true)
            {
                if (check.Count == 0)
                {
                    decreasedLevel = 0;
                    return null;
                }

                var start = StartStep(check);

                mileage.Add(start, check[start]);
                check.Remove(start);

                if (boxes.Contains(start))
                {
                    lastPoint = start;
                    break;
                }

                foreach (var nextPoint in Environs(start))
                {
                    if (mileage.ContainsKey(nextPoint) || !state.InsideMap(nextPoint) || state.IsWallAt(nextPoint))
                    {
                        continue;
                    }

                    var energyForNextPoint = state.CellCost[nextPoint.X, nextPoint.Y]
                                             + mileage[start].LevelDown;

                    CheckSteps(check, nextPoint, energyForNextPoint, start);
                }
            }

            decreasedLevel = mileage[lastPoint].LevelDown;

            return GetResult(mileage, begin, lastPoint);
        }
        
        class MovingData
        {
            public Point Previous { get;}
            public int LevelDown { get;}

            public MovingData(Point previous, int levelDown)
            {
                Previous = previous;
                LevelDown = levelDown;
            }
        }
        
        private IEnumerable<Point> Environs(Point current)
        {
            for (var x = current.X - 1; x < current.X + 2; x++)
            {
                for (var y = current.Y - 1; y < current.Y + 2; y++)
                {
                    if (x == current.X || y == current.Y)
                    {
                        yield return new Point(x, y);
                    }
                }
            }
        }
        
        private Point StartStep(Dictionary<Point, MovingData> checkedStep)
        {
            var startStep = default(Point);
            var levelStep = int.MaxValue;

            foreach (var point in checkedStep.Keys)
            {
                if (levelStep > checkedStep[point].LevelDown)
                {
                    levelStep = checkedStep[point].LevelDown;
                    startStep = point;
                }
            }

            return startStep;
        }

        private void CheckSteps(Dictionary<Point, MovingData> checkedSteps,
                        Point nextStep, int levelNextStep, Point undoStep)
        {
            if (checkedSteps.TryGetValue(nextStep, out var alsoCheckedStep))
            {
                if (alsoCheckedStep.LevelDown <= levelNextStep)
                {
                    return;
                }
            }

            checkedSteps[nextStep] = new MovingData(undoStep, levelNextStep);
        }

        private List<Point> GetResult(Dictionary<Point, MovingData> passedPoints, Point startStep, Point lastStep)
        {
            var total = new List<Point>();

            while (lastStep != startStep)
            {
                total.Add(lastStep);
                lastStep = passedPoints[lastStep].Previous;
            }

            total.Reverse();

            return total;
        }
    }
}