using System;
using System.Collections.Generic;
using System.Linq;

namespace Meuzz.Foundation
{
    public class DataSet<C, T>
    {
        public DataSet() : this(new C[] { }, new C[] { }, new T[] { })
        {
        }

        public DataSet(C[] keys, C[] cols, T[] rows)
        {
            KeyColumns = keys;
            Columns = cols;
            Rows = rows;
        }

        public C[] KeyColumns { get; set; }

        public C[] Columns { get; set; }

        public T[] Rows { get; set; }

        public static T1 CreateWithFilteringColumns<T1>(T1 source, C[] cols) where T1 : DataSet<C, T>, new()
        {
            return new T1() { KeyColumns = source.KeyColumns, Columns = source.Columns.Where(x => cols.Contains(x)).ToArray(), Rows = source.Rows };
        }

        public static T1 CreateWithExcludingColumns<T1>(T1 source, C[] cols) where T1 : DataSet<C, T>, new()
        {
            return new T1() { KeyColumns = source.KeyColumns, Columns = source.Columns.Where(x => !cols.Contains(x)).ToArray(), Rows = source.Rows };
        }

        public static T1 CreateWithFilteringRows<T1>(T1 source, Func<T, bool> predicate) where T1 : DataSet<C, T>, new()
        {
            return new T1() { KeyColumns = source.KeyColumns, Columns = source.Columns, Rows = source.Rows.Where(x => predicate(x)).ToArray() };
        }

        public static T1 CreateWithExcludingRows<T1>(T1 source, Func<T, bool> predicate) where T1 : DataSet<C, T>, new()
        {
            return new T1() { KeyColumns = source.KeyColumns, Columns = source.Columns, Rows = source.Rows.Where(x => !predicate(x)).ToArray() };
        }

        /// <summary>
        /// 2つ以上のDataSetを連結する
        /// </summary>
        /// <param name="dataSets"></param>
        public static T1 CreateFromDataSets<T1>(IEnumerable<T1> dataSets) where T1 : DataSet<C, T>, new()
        {
            C[] cols = { };
            T[] rows = { };

            if (dataSets.Any())
            {
                cols = dataSets.FirstOrDefault()?.Columns ?? cols;
                rows = Arrays.Concat<T>(dataSets.Select((x) => x.Rows).ToArray());
            }

            return new T1() { KeyColumns = dataSets.FirstOrDefault()?.KeyColumns ?? new C[] { }, Columns = cols, Rows = rows };
        }

    }

    public static class DataSetExtensions
    {
        [Obsolete]
        /// <summary>
        /// 2つ以上のDataSetを連結する
        /// </summary>
        /// <param name="dataSets"></param>
        public static T2 Concat<C, T, T2>(this IEnumerable<T2> dataSets) where T2 : DataSet<C, T>, new()
        {
            C[] cols = { };
            T[] rows = { };

            if (dataSets.Any())
            {
                cols = dataSets.FirstOrDefault()?.Columns ?? cols;
                rows = Arrays.Concat<T>(dataSets.Select((x) => x.Rows).ToArray());
            }

            return new T2() { KeyColumns = dataSets.FirstOrDefault()?.KeyColumns ?? new C[] { }, Columns = cols, Rows = rows };
        }
    }
}