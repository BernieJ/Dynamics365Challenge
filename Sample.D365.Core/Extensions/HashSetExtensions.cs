namespace System.Collections.Generic
{
  /// <summary>
  /// The HashSetExtensions class
  /// </summary>
  public static class HashSetExtensions
  {
    /// <summary>
    /// Adds an object to the end of the System.Collections.Generic.HashSet&lt;T&gt; if the value is not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="hashSet">A System.Collections.Generic.HashSet&lt;T&gt; instance</param>
    /// <param name="item">The object to be added to the end of the System.Collections.Generic.HashSet&lt;T&gt;</param>
    /// <returns>true if the element is added to the System.Collections.Generic.HashSet&lt;T&gt; object; false if the element is null or already present.</returns>
    public static bool AddIfNotNull<T>(this HashSet<T> hashSet, T item)
    {
      if (item != null)
      {
        return hashSet.Add(item);
      }

      return false;
    }
  }
}
