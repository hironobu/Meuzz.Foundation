using System.Collections.Generic;
using System.Linq;

namespace Meuzz.Foundation
{
    public static class IEnumerableTransposeClassExtensions
    {
        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> matrix) where T : class
        {
            List<IEnumerable<T>> resultMatrix = new List<IEnumerable<T>>();

            var iters = matrix.Select(row => new { HasNext = true, _X = row.GetEnumerator() as IEnumerator<T> });
            iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            while (iters.Any(x => x.HasNext))
            {
                IEnumerable<T> ee = iters.Select(x => x.HasNext ? (T)x._X.Current : (T)null).ToArray();
                resultMatrix.Add(ee);

                iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            }

            return resultMatrix;
        }
    }

    public static class IEnumerableTransposeStructExtensions
    { 
        public static IEnumerable<IEnumerable<T?>> Transpose<T>(this IEnumerable<IEnumerable<T>> matrix) where T : struct
        {
            List<IEnumerable<T?>> resultMatrix = new List<IEnumerable<T?>>();

            var iters = matrix.Select(row => new { HasNext = true, _X = row.GetEnumerator() as IEnumerator<T> });
            iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            while (iters.Any(x => x.HasNext))
            {
                IEnumerable<T?> ee = iters.Select(x => x.HasNext ? (T?)x._X.Current : (T?)null).ToArray();
                resultMatrix.Add(ee);

                iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            }

            return resultMatrix;
        }

    }
}
