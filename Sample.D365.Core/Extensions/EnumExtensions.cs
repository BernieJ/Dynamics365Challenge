namespace System
{
  using System.ComponentModel;
  using System.Reflection;

  /// <summary>
  /// The EnumExtensions class
  /// </summary>
  public static class EnumExtensions
  {
    /// <summary>
    /// Converts the value of this instance to its equivalent description attribute
    /// </summary>
    /// <param name="value">An Enum value</param>
    /// <returns>The value of this instance to its equivalent description attribute</returns>
    public static string ToDescription(this Enum value)
    {
      FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
      DescriptionAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
      return attribute.Description;
    }
  }
}
