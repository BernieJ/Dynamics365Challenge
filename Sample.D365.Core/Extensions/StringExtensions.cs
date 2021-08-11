namespace System
{
  /// <summary>
  /// The StringExtensions class
  /// </summary>
  public static class StringExtensions
  {
    /// <summary>
    /// Retrieves a substring of a specified length from the left hand side of this instance.
    /// </summary>
    /// <param name="value">A System.String reference</param>
    /// <param name="length">The number of characters to return</param>
    /// <exception cref="NullReferenceException">A NullReferenceException</exception>
    /// <returns>A substring of a specified length from the left hand side of this instance</returns>
    public static string Left(this string value, int length)
    {
      if (value == null)
      {
        throw new NullReferenceException();
      }
      if (value.Length > length)
      {
        return value.Substring(0, length);
      }
      else
      {
        return value;
      }
    }

    /// <summary>
    /// Retrieves a substring of a specified length from the right hand side of this instance.
    /// </summary>
    /// <param name="value">A System.String reference</param>
    /// <param name="length">The number of characters to return</param>
    /// <exception cref="NullReferenceException">A NullReferenceException</exception>
    /// <returns>A substring of a specified length from the left right side of this instance</returns>
    public static string Right(this string value, int length)
    {
      if (value == null)
      {
        throw new NullReferenceException();
      }
      if (value.Length > length)
      {
        return value.Substring(value.Length - length);
      }
      else
      {
        return value;
      }
    }
  }
}
