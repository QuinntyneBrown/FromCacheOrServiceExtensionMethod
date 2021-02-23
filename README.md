# FromCacheOrService Extension Method

An example of how you can save yourself from writing extra code when implementing simple caching in your applications

## Give a Star! :star:

If you like or are using this project to learn or start your solution, please give it a star. Thanks!

## Code

```csharp

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
```    

## Use

```csharp

Dictionary<Guid, string> _guidStringCache = new Dictionary<Guid, string>()
{
    { guidKey, "Hello World from Cache" }
};
            
Console.WriteLine(_guidStringCache.FromCacheOrService(() => "Hello World", Guid.NewGuid()));

```
