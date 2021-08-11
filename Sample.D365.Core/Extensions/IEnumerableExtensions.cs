namespace System.Collections.Generic
{
  using System;
  using System.Collections.Concurrent;
  using System.Linq;

  /// <summary>
  /// The IEnumerableExtensions class
  /// </summary>
  public static class IEnumerableExtensions
  {
    /// <summary>
    /// Determines whether a sequence is empty
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <param name="source">The source</param>
    /// <returns>true if the source sequence is empty; otherwise, false.</returns>
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source)
    {
      return !source.Any();
    }

    /// <summary>
    /// Determines whether a sequence is empty after applying a condition
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <param name="source">The source</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>true if no elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
    public static bool IsEmpty<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return !source.Any(predicate);
    }

    /// <summary>
    /// Determines whether a sequence contains no elements
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <param name="source">The source</param>
    /// <returns>true if the source sequence contains no elements; otherwise, false.</returns>
    public static bool None<TSource>(this IEnumerable<TSource> source)
    {
      return !source.Any();
    }

    /// <summary>
    /// Determines whether none of the elements of a sequence satisfy a condition
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <param name="source">The source</param>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <returns>true if no elements in the source sequence pass the test in the specified predicate; otherwise, false.</returns>
    public static bool None<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
    {
      return !source.Any(predicate);
    }

    /// <summary>
    /// Creates a System.Collections.Concurrent&lt;TKey, TValue&gt; from an System.Collections.Generic.IEnumerable&lt;TValue&gt;
    /// </summary>
    /// <typeparam name="TKey">The type of the key</typeparam>
    /// <typeparam name="TValue">The type of elements in the collection</typeparam>
    /// <param name="source">A System.Collections.Generic.IEnumerable&lt;T&gt; instance</param>
    /// <param name="keySelector">A System.Func&lt;in TValue, out TKey&gt; key selector function</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <returns>A System.Collections.Concurrent&lt;TKey, TValue&gt; that contains elements from the input sequence</returns>
    public static ConcurrentDictionary<TKey, TValue> ToConcurrentDictionary<TKey, TValue>(this IEnumerable<TValue> source, Func<TValue, TKey> keySelector)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      ConcurrentDictionary<TKey, TValue> result = new ConcurrentDictionary<TKey, TValue>();
      foreach (TValue value in source)
      {
        result.TryAdd(keySelector(value), value);
      }

      return result;
    }

    /// <summary>
    /// Creates a System.Collections.Generic.HashSet&lt;T&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt;
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the collection</typeparam>
    /// <param name="source">A System.Collections.Generic.IEnumerable&lt;T&gt; instance</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <returns>A System.Collections.Generic.HashSet&lt;T&gt; that contains elements from the input sequence</returns>
    public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      return new HashSet<TSource>(source);
    }

    /// <summary>
    /// Creates a System.Collections.Generic.HashSet&lt;T&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt;
    /// </summary>
    /// <typeparam name="TSource">The type of elements in the collection</typeparam>
    /// <param name="source">A System.Collections.Generic.IEnumerable&lt;T&gt; instance</param>
    /// <param name="comparer">An System.Collections.Generic.IEqualityComparer&lt;T&gt; to compare keys</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <returns>A System.Collections.Generic.HashSet&lt;T&gt; that contains elements from the input sequence</returns>
    public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
    {
      if (source == null)
      {
        throw new ArgumentNullException(nameof(source));
      }

      return new HashSet<TSource>(source, comparer);
    }

    /// <summary>
    /// Creates a System.Collections.Generic.HashSet&lt;TResult&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt; according to a specific item selector function
    /// </summary>
    /// <typeparam name="TSource">The source type of elements in the collection</typeparam>
    /// <typeparam name="TResult">The destination type of elements in the collection</typeparam>
    /// <param name="source">A System.Collections.Generic.IEnumerable&lt;T&gt; instance</param>
    /// <param name="newItemSelector">A System.Func&lt;in T, out TResult&gt; item selector function</param>
    /// <exception cref="System.ArgumentNullException">source is null</exception>
    /// <returns>A System.Collections.Generic.HashSet&lt;TResult&gt; from an System.Collections.Generic.IEnumerable&lt;T&gt; according to a specific item selector function</returns>
    public static HashSet<TResult> ToHashSet<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> newItemSelector)
    {
      return source.Select(newItemSelector).ToHashSet();
    }
  }
}
