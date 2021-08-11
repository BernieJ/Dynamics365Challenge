namespace System
{

  /// <summary>
  /// The DateTimeExtensions class
  /// </summary>
  public static class DateTimeExtensions
  {
    /// <summary>
    /// Round a Time-zone date to a date only UTC date
    /// </summary>
    /// <param name="dateTime">The date and time to round</param>
    /// <returns>A Date object with a Date only value rounded to the nearest day</returns>
    public static DateTime RoundToDateOnly(this DateTime dateTime)
    {
      return dateTime.AddDays(dateTime.Hour >= 12 ? 1 : 0).Date;
    }
  }
}
