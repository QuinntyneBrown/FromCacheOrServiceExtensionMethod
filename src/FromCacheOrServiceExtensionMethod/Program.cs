using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromCacheOrServiceExtensionMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            var guidKey = Guid.NewGuid();
            var tupleKey = new Tuple<DayOfWeek, Guid>(DayOfWeek.Monday, guidKey);
            Dictionary<Guid, string> _guidStringCache = new();
            Dictionary<Tuple<DayOfWeek, Guid>, string> _tupleStringCache = new();
            
            Console.WriteLine(_guidStringCache.FromCacheOrService(() => $"Hello World at {DateTime.UtcNow.Millisecond}", guidKey));

            Console.WriteLine(_guidStringCache.FromCacheOrService(() => $"Hello World at {DateTime.UtcNow.Millisecond}", guidKey));

            Console.WriteLine(_tupleStringCache.FromCacheOrService(() => GetSomethingByParameter("Something"), tupleKey));

            Console.WriteLine(_tupleStringCache.FromCacheOrService(() => GetSomethingByParameter("Something"), tupleKey));

            Console.ReadLine();
        }

        public static string GetSomethingByParameter(string param)
            => $"Hello World By {param} parameter at {DateTime.UtcNow.Millisecond}";
    }

    public static class DictionaryExtensions
    {
        public static TValue FromCacheOrService<TKey, TValue>(this Dictionary<TKey,TValue> dictionary, Func<TValue> action, TKey key)
        {
            dictionary.TryGetValue(key, out TValue cached);
            if (cached == null)
            {
                cached = action();
                dictionary.Add(key, cached);
            }
            return cached;
        }

        public static async Task<TValue> FromCacheOrServiceAsync<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Func<Task<TValue>> action, TKey key)
        {
            dictionary.TryGetValue(key, out TValue cached);
            if (cached == null)
            {
                cached = await action();
                dictionary.Add(key, cached);
            }
            return cached;
        }
    }

    enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }
}
