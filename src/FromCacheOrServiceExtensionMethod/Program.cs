using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FromCacheOrServiceExtensionMethod
{
    enum DayOfWeek
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday
    }
    class Program
    {
        static void Main(string[] args)
        {
            var guidKey = Guid.NewGuid();
            var tupleKey = new Tuple<DayOfWeek, Guid>(DayOfWeek.Monday, guidKey);
            
            Dictionary<Guid, string> _guidStringCache = new Dictionary<Guid, string>()
            {
                { guidKey, "Hello World from Cache" }
            };

            Dictionary<Tuple<DayOfWeek, Guid>, string> _tupleStringCache = new Dictionary<Tuple<DayOfWeek,Guid>, string>()
            {
                { tupleKey, "Hello World from Tuple Cache" }
            };

            Console.WriteLine(_guidStringCache.FromCacheOrService(() => "Hello World", guidKey));

            Console.WriteLine(_guidStringCache.FromCacheOrService(() => "Hello World", Guid.NewGuid()));

            Console.WriteLine(_tupleStringCache.FromCacheOrService(() => GetSomethingByParameter("Something"), new Tuple<DayOfWeek, Guid>(DayOfWeek.Monday, guidKey)));

            Console.WriteLine(_tupleStringCache.FromCacheOrService(() => GetSomethingByParameter("Something"), new Tuple<DayOfWeek, Guid>(DayOfWeek.Monday, Guid.NewGuid())));

            Console.ReadLine();
        }

        public static string GetSomethingByParameter(string param)
            => $"Hello World By {param} parameter";
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
}
