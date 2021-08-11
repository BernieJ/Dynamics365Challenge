namespace System.Collections.Generic
{
  /// <summary>
  /// The DictionaryExtensions class
  /// </summary>
  public static class DictionaryExtensions
  {
    /// <summary>
    /// Adds the specified key and value to the dictionary if the dictionary does not already contain the key.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
    /// <param name="dictionary">A System.Collections.Generic.Dictionary&lt;TKey, TValue&gt; instance</param>
    /// <param name="key">The key of the element to add</param>
    /// <param name="value">The value of the element to add</param>
    public static void AddIfNotContainsKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
      if (!dictionary.ContainsKey(key))
      {
        dictionary.Add(key, value);
      }
    }

    /// <summary>
    /// Adds the specified key and value to the dictionary if the dictionary does not already contain the value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
    /// <param name="dictionary">A System.Collections.Generic.Dictionary&lt;TKey, TValue&gt; instance</param>
    /// <param name="key">The key of the element to add</param>
    /// <param name="value">The value of the element to add</param>
    public static void AddIfNotContainsValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
      if (!dictionary.ContainsValue(value))
      {
        dictionary.Add(key, value);
      }
    }
  }
}
