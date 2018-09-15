using System.Collections.Generic;

namespace yield
{
    public static class MovingAverageTask
    {
        public static IEnumerable<DataPoint> MovingAverage(this IEnumerable<DataPoint> data, int windowWidth)
        {
            var point = new Queue<double>();
            var rollbackMoving = 0d;
            var checkFilled = false;
            foreach (var item in data)
            {
                point.Enqueue(item.OriginalY);

                var rollCapacity = checkFilled ? point.Count : point.Count - 1;
                rollbackMoving = item.AvgSmoothedY =
                    CrutchMoving(rollbackMoving, item.OriginalY,
                        rollCapacity,point.Count,
                        checkFilled ? point.Dequeue() / windowWidth : 0, checkFilled);
                if (!checkFilled && point.Count == windowWidth)
                    checkFilled = true;
                yield return item;
            }
        }

        static double CrutchMoving(double rollbackCrutch, double mainImportance,
            int rollCapacity, int presensValue,
            double outside, bool checkFilled)
        {
            return rollbackCrutch
                   * ((double) rollCapacity / presensValue)
                   + mainImportance / (checkFilled ? presensValue - 1 : presensValue)
                   - outside;
        }
    }
}