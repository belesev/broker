using System;
using QuikSharp.DataStructures;

namespace Tests
{
    public static class CurrentTime
    {
        private static Func<QuikDateTime> func;

        public static void SetFunction(Func<QuikDateTime> funcParam)
        {
            func = funcParam;
        }

        public static DateTime GetTime()
        {
            if (func != null)
                return (DateTime) func();
            return DateTime.Now;
        }
    }
}