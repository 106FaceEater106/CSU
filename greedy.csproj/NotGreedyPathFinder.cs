using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Greedy.Architecture;
using Greedy.Architecture.Drawing;

namespace Greedy
{
    public class NotGreedyPathFinder : IPathFinder
    {
        public List<Point> FindPathToCompleteGoal(State state)
        {
            var dgrm = new Diagramm(state.MapHeight * state.MapWidth);
            var mass = new Dictionary<Gap, double>();
            var boxes = state.Chests.Take(Protector);

            var amongBoxes = new Dictionary<Point, Dictionary<Point, List<Point>>>();

            InitializeGraphOnCells(state, mass, dgrm);

            amongBoxes[state.Position] = GetPathToAllChests(dgrm, mass, state.Position, state);

            foreach (var box in boxes)
                amongBoxes[box] = GetPathToAllChests(dgrm, mass, box, state);

            var shortcut = new List<Point>();

            for (var a = 1; a <= Protector; a++)
            {
                var optionsBox = GetBust(boxes, a)
                    .Where(perm => HowFarBoxWay(state, perm, amongBoxes) <= state.Energy);

                if (!optionsBox.Any())
                    break;

                shortcut = optionsBox
                    .First()
                    .ToList();
            }

            return AmountHowFar(state, shortcut, amongBoxes);
        }
        
        private static Dictionary<Point, List<Point>> GetPathToAllChests(Diagramm dgrm, Dictionary<Gap, double> mass,
            Point start, State state)
        {
            var total = new Dictionary<Point, List<Point>>();
            var boxes = state.Chests.Where(box => box != start);

            foreach (var box in boxes)
                total[box] = BoxWay(dgrm, mass, state, start, box);

            return total;
        }
        
        public const int Protector = 8;

        private static List<Point> BoxWay(Diagramm dgrm, Dictionary<Gap, double> mass,
            State state, Point start, Point box)
        {
            var stance = StepCheck(start, state.MapWidth);
            var checker = StepCheck(box, state.MapWidth);

            var way = stepOvule(dgrm, mass, dgrm[stance], dgrm[checker]);

            return way?.Select(z => StepMaker(z.UnitNumber, state.MapWidth)).Skip(1).ToList() ??
                   new List<Point>();
        }

        private static int IsHowFar(State state, IEnumerable<Point> way)
        {
            return way.Sum(step => state.CellCost[step.X, step.Y]);
        }

        private static int HowFarBoxWay(State state, IEnumerable<Point> boxes,
            Dictionary<Point, Dictionary<Point, List<Point>>> howFarWay)
        {
            var ongoing = state.Position;
            var total = 0;

            foreach (var box in boxes)
            {
                total += IsHowFar(state, howFarWay[ongoing][box]);
                ongoing = box;
            }

            return total;
        }

        private static List<Point> AmountHowFar(State state, ICollection<Point> pointsPermutation,
            Dictionary<Point, Dictionary<Point, List<Point>>> pathsBetweenChests)
        {
            var ongoing = state.Position;
            var total = new List<Point>();

            foreach (var point in pointsPermutation)
            {
                total.AddRange(pathsBetweenChests[ongoing][point]);
                ongoing = point;
            }

            return total;
        }

        private static IEnumerable<IEnumerable<T>> GetBust<T>(IEnumerable<T> roster, int size)
        {
            if (size == 1) return roster.Select(a => new[] { a });
            return GetBust(roster, size - 1)
                .SelectMany(a => roster.Where(o => !a.Contains(o)),
                    (a1, a2) => a1.Concat(new[] { a2 }));
        }

        private static void InitializeGraphOnCells(State state, IDictionary<Gap, double> mass, Diagramm dgrm)
        {
            for (var b = 0; b < state.MapHeight; b++)
                for (var a = 0; a < state.MapWidth; a++)
                {
                    var point = new Point(a, b);

                    for (var cb = -1; cb <= 1; cb++)
                        for (var ca = -1; ca <= 1; ca++)
                        {
                            if (ca != 0 && cb != 0) continue;
                            var neighbour = new Point(a + ca, b + cb);
                            if (!state.InsideMap(neighbour)) continue;
                            if (state.IsWallAt(neighbour)) continue;

                            var pointNumber = StepCheck(point, state.MapWidth);
                            var neighbourNumber = StepCheck(neighbour, state.MapWidth);
                            mass[dgrm.Connect(pointNumber, neighbourNumber)] = state.CellCost[neighbour.X, neighbour.Y];
                        }
                }
        }

        private static int StepCheck(Point point, int width)
        {
            return point.Y * width + point.X;
        }

        private static Point StepMaker(int pointNumber, int width)
        {
            return new Point(pointNumber % width, pointNumber / width);
        }

        private static List<Unit> stepOvule(Diagramm dgrm, Dictionary<Gap, double> mass, Unit start, Unit end)
        {
            var clear = dgrm.Units.ToList();
            var way = new Dictionary<Unit, StepCounter>();
            way[start] = new StepCounter { Price = 0, Previous = null };

            while (true)
            {
                Unit discover = null;
                var betterOffer = double.PositiveInfinity;
                foreach (var v in clear)
                    if (way.ContainsKey(v) && way[v].Price < betterOffer)
                    {
                        betterOffer = way[v].Price;
                        discover = v;
                    }

                if (discover == null) return null;
                if (discover == end) break;

                foreach (var e in discover.IncidentEdges.Where(z => z.From == discover))
                {
                    var currentPrice = way[discover].Price + mass[e];
                    var nextNode = e.GetUnit(discover);
                    if (!way.ContainsKey(nextNode) || way[nextNode].Price > currentPrice)
                        way[nextNode] = new StepCounter { Previous = discover, Price = currentPrice };
                }

                clear.Remove(discover);
            }

            var total = new List<Unit>();
            while (end != null)
            {
                total.Add(end);
                end = way[end].Previous;
            }
            total.Reverse();
            return total;
        }

        internal class StepCounter
        {
            public Unit Previous { get; set; }
            public double Price { get; set; }
        }

        internal class Gap
        {
            public readonly Unit From;
            public readonly Unit To;

            public Gap(Unit a, Unit b)
            {
                From = a;
                To = b;
            }

            public bool IsIncident(Unit unit)
            {
                return From == unit || To == unit;
            }

            public Unit GetUnit(Unit unit)
            {
                if (!IsIncident(unit)) throw new ArgumentException();
                if (From == unit) return To;
                return From;
            }
        }

        internal class Unit
        {
            private readonly List<Gap> edges = new List<Gap>();
            public readonly int UnitNumber;

            public Unit(int number)
            {
                UnitNumber = number;
            }

            public IEnumerable<Unit> IncidentUnits
            {
                get { return edges.Select(z => z.GetUnit(this)); }
            }

            public IEnumerable<Gap> IncidentEdges
            {
                get
                {
                    foreach (var e in edges) 
                        yield return e;
                }
            }

            public static Gap Connect(Unit first, Unit second, Diagramm dgrm)
            {
                if (!dgrm.Units.Contains(first) || !dgrm.Units.Contains(second)) throw new ArgumentException();
                var edge = new Gap(first, second);
                first.edges.Add(edge);
                return edge;
            }
        }

        internal class Diagramm
        {
            readonly Unit[] group;
            
            public static Diagramm MakeGraph(params int[] units)
            {
                var dgrm = new Diagramm(units.Max() + 1);
                for (var a = 0; a < units.Length - 1; a += 2)
                    dgrm.Connect(units[a], units[a + 1]);
                return dgrm;
            }
            
            public Diagramm(int nodesCount)
            {
                group = Enumerable.Range(0, nodesCount).Select(z => new Unit(z)).ToArray();
            }

            public int Size => group.Length;

            public Unit this[int index] => group[index];

            public IEnumerable<Unit> Units
            {
                get
                {
                    foreach (var unit in group) 
                        yield return unit;
                }
            }
            
            public Gap Connect(int index1, int index2)
            {
                return Unit.Connect(group[index1], group[index2], this);
            }
            
            public IEnumerable<Gap> Borders
            {
                get { return group.SelectMany(z => z.IncidentEdges).Distinct(); }
            }
        }
    }
}