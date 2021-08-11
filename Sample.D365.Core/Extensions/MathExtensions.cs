namespace BES.D365.Core.Extensions
{
  using System;

  /// <summary>
  /// The MathExtensions class
  /// </summary>
  public static class MathExtensions
  {
    /// <summary>
    /// Returns the nth root of the specified number
    /// </summary>
    /// <param name="x">The x</param>
    /// <param name="n">The n</param>
    /// <returns>The nth root of the specified number</returns>
    public static double NthRoot(double x, double n)
    {
      return Math.Pow(x, 1.0 / n);
    }
  }
}
