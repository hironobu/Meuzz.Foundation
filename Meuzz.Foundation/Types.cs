using System;
using System.Linq;
using System.Reflection;

namespace Meuzz.Foundation
{
    public static class TypeExtensions
    {
        public static bool IsTuple(this Type tuple)
        {
            if (!tuple.IsGenericType)
            {
                return false;
            }
            var openType = tuple.GetGenericTypeDefinition();
            return openType == typeof(ValueTuple<>)
                || openType == typeof(ValueTuple<,>)
                || openType == typeof(ValueTuple<,,>)
                || openType == typeof(ValueTuple<,,,>)
                || openType == typeof(ValueTuple<,,,,>)
                || openType == typeof(ValueTuple<,,,,,>)
                || openType == typeof(ValueTuple<,,,,,,>)
                || (openType == typeof(ValueTuple<,,,,,,,>) && IsTuple(tuple.GetGenericArguments()[7]));
        }
    }

    public static class TupleTypeExtensions
    {
        public static bool IsGenericTuple(this Type? type, bool checkBaseTypes = false)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (type == typeof(Tuple))
                return true;

            while (type != null)
            {
                if (type.IsGenericType)
                {
                    var genType = type.GetGenericTypeDefinition();
                    if (genType == typeof(Tuple<>)
                        || genType == typeof(Tuple<,>)
                        || genType == typeof(Tuple<,,>)
                        || genType == typeof(Tuple<,,,>)
                        || genType == typeof(Tuple<,,,,>)
                        || genType == typeof(Tuple<,,,,,>)
                        || genType == typeof(Tuple<,,,,,,>)
                        || genType == typeof(Tuple<,,,,,,,>)
                        || genType == typeof(Tuple<,,,,,,,>))
                        return true;
                }

                if (!checkBaseTypes)
                    break;

                type = type.BaseType;
            }

            return false;
        }

    }

    public static class TypedTuple
    { 
        public static object Make(object source)
        {
            var t = source.GetType();
            if (!t.IsTuple())
            {
                throw new ArgumentException("argument should be tuple", "source");
            }

            var fields = t.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var fieldValues = fields.Select(x => x.GetValue(source)).ToArray();
            var fts = fieldValues.Select(v => v!.GetType()).ToArray();

            Type tupleType;

            switch (fts.Length)
            {
                case 1:
                    tupleType = typeof(ValueTuple<>);
                    break;
                case 2:
                    tupleType = typeof(ValueTuple<,>);
                    break;
                case 3:
                    tupleType = typeof(ValueTuple<,,>);
                    break;
                case 4:
                    tupleType = typeof(ValueTuple<,,,>);
                    break;
                case 5:
                    tupleType = typeof(ValueTuple<,,,,>);
                    break;
                case 6:
                    tupleType = typeof(ValueTuple<,,,,,>);
                    break;
                case 7:
                    tupleType = typeof(ValueTuple<,,,,,,>);
                    break;
                case 8:
                    tupleType = typeof(ValueTuple<,,,,,,,>);
                    break;
                default:
                    throw new NotImplementedException();
            }

            tupleType = tupleType.MakeGenericType(fts);

            // var tupleType = typeof(ValueTuple<,>).MakeGenericType(left!.GetType(), right!.GetType());
            return Activator.CreateInstance(tupleType, fieldValues)!;
        }
    }
}
