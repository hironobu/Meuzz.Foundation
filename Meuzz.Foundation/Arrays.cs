using System;
using System.Linq;

namespace Meuzz.Foundation
{
    public class Arrays
    {
        public static bool IsNullOrEmpty<T>(T[] arr)
        {
            return arr == null || !arr.Any();
        }

        public static bool Equals<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            return arr1.Zip(arr2, (a, b) => a.Equals(b)).All(x => x);
        }

        public static T[] Concat<T>(params T[][] arrs)
        {
            var result = new T[arrs.Select((x) => x.Length).Aggregate((x, y) => x + y)];
            int i = 0;
            foreach (var a in arrs)
            {
                Array.Copy(a, 0, result, i, a.Length);
                i += a.Length;
            }
            return result;
        }
    }
}
