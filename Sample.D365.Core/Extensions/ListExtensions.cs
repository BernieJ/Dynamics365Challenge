namespace System.Collections.Generic
{
  /// <summary>
  /// The ListExtensions class
  /// </summary>
  public static class ListExtensions
  {
    /// <summary>
    /// Adds an object to the end of the System.Collections.Generic.List&lt;T&gt; if the value is not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="list">A System.Collections.Generic.List&lt;T&gt; instance</param>
    /// <param name="item">The object to be added to the end of the System.Collections.Generic.List&lt;T&gt;</param>
    public static void AddIfNotNull<T>(this List<T> list, T item)
    {
      if (item != null)
      {
        list.Add(item);
      }
    }
  }
}
