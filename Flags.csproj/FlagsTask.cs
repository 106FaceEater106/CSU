namespace Flags
{
    public static class FlagsTask
    {
        public static long Solve(int a)
        {
            var output = new long[a];
            if (a <= 2) return 2;
            output[0] = 2;
            output[1] = 2;
            for (var i = 2; i < a; i++)
                output[i] = output[i - 1] + output[i - 2];
            return output[a - 1];
        }
    }
}