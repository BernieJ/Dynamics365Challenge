namespace System.Collections.ObjectModel
{
  /// <summary>
  /// The CollectionExtensions class
  /// </summary>
  public static class CollectionExtensions
  {
    /// <summary>
    /// Adds an object to the end of the System.Collections.ObjectModel.Collection&lt;T&gt; if the value is not null.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    /// <param name="collection">A System.Collections.ObjectModel.Collection&lt;T&gt; instance</param>
    /// <param name="item">The object to be added to the end of the System.Collections.ObjectModel.Collection&lt;T&gt;</param>
    public static void AddIfNotNull<T>(this Collection<T> collection, T item)
    {
      if (item != null)
      {
        collection.Add(item);
      }
    }
  }
}
