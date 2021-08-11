namespace System
{
  using System.Linq;

  /// <summary>
  /// The ObjectExtensions class
  /// </summary>
  public static class ObjectExtensions
  {
    /// <summary>
    /// Gets a value indicating whether the supplied value is in the list of supplied values
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="value">The value to search for</param>
    /// <param name="values">The values to search in</param>
    /// <returns>A value indicating whether the supplied value is in the list of supplied values</returns>
    public static bool In<T>(this T value, params T[] values)
      where T : IComparable
    {
      return values.Any(v => (v == null && value == null) || (v != null && v.Equals(value)));
    }
  }
}
