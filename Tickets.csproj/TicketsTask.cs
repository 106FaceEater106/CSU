
using System.Numerics;

namespace Tickets
{
    public static class TicketsTask
    {
        public static BigInteger Solve(int size, int entire)
        {
            if (entire % 2 != 0) return 0;
            var subEntire = entire * 1/2;
            var output = new BigInteger[size + 1, subEntire + 1];

            for (var a = 0; a < size + 1; a++)
                output[a, 0] = 1;

            for (var a = 1; a < size + 1; a++)
            for (var b = 1; b < subEntire + 1; b++)
            for (var c = 0; c <= b && c < 10; c++)
                output[a, b] += output[a - 1, b - c];
            return output[size, subEntire] * output[size, subEntire];
        }
    }
}