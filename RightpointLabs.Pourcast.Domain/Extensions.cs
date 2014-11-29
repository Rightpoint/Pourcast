using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightpointLabs.Pourcast.Domain
{
    public static class Extensions
    {
        public static V TryGetValue<K, V>(this IReadOnlyDictionary<K, V> dictionary, K key)
        {
            V value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return default(V);
        }

        public static T2 ChainIfNotNull<T1, T2>(this T1 value, Func<T1, T2> chainCall) where T1: class
        {
            if (value == null)
                return default(T2);
            return chainCall(value);
        }
    }
}
