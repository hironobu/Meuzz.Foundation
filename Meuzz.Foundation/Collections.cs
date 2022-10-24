using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Meuzz.Foundation
{
    public static class IEnumerableTransposeClassExtensions
    {
        public static IEnumerable<IEnumerable<T?>> Transpose<T>(this IEnumerable<IEnumerable<T?>> matrix) where T : class
        {
            var resultMatrix = new List<IEnumerable<T?>>();

            var iters = matrix.Select(row => new { HasNext = true, _X = row.GetEnumerator() });
            iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            while (iters.Any(x => x.HasNext))
            {
                IEnumerable<T?> ee = iters.Select(x => x.HasNext ? x._X.Current : null).ToArray();
                resultMatrix.Add(ee);

                iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            }

            return resultMatrix;
        }
    }

    public static class IEnumerableTransposeStructExtensions
    { 
        public static IEnumerable<IEnumerable<T?>> Transpose<T>(this IEnumerable<IEnumerable<T?>> matrix) where T : struct
        {
            var resultMatrix = new List<IEnumerable<T?>>();

            var iters = matrix.Select(row => new { HasNext = true, _X = row.GetEnumerator() });
            iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            while (iters.Any(x => x.HasNext))
            {
                IEnumerable<T?> ee = iters.Select(x => x.HasNext ? x._X.Current : null).ToArray();
                resultMatrix.Add(ee);

                iters = iters.Select(x => new { HasNext = x.HasNext ? x._X.MoveNext() : false, _X = x._X }).ToArray();
            }

            return resultMatrix;
        }
    }

    public static class IEnumerableExtensions
    {
        public static IEnumerable<object?> EnumerableUncast<T>(this IEnumerable<T> args, Type t)
        {
            // TODO: array or list以外は避けるように(ex. IDictionary<>)
            var t1 = t.IsGenericType && t.GetGenericArguments().Length == 1 ? t.GetGenericArguments()[0] : t;

            switch (t1)
            {
                case Type inttype when inttype == typeof(int):
                    return args.Select(x => (object)Convert.ToInt32(x));

                case Type longtype when longtype == typeof(long):
                    return args.Select(x => (object)Convert.ToInt64(x));

                default:
                    var conv = _methodInfoCast.MakeGenericMethod(t1);
                    return (IEnumerable<object?>)conv.Invoke(null, new[] { args })!;
            }
        }

        private static MethodInfo _methodInfoCast = typeof(Enumerable).GetMethod("Cast") ?? throw new NotImplementedException();
    }

    public static class IDictionaryExtensions
    {
        public static TValue GetValueOrNew<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key)
            where TKey : notnull
            where TValue : new()
        {
            return GetValueOrFunc(self, key, () => new TValue());
        }

        public static TValue GetValueOrFunc<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TValue> newFunc)
            where TKey : notnull
        {
            if (!self.TryGetValue(key, out TValue value))
            {
                value = newFunc();
                self.Add(key, value);
            }
            return value;
        }
    }
}
