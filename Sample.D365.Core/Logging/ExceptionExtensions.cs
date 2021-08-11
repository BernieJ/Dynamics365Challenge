namespace Sample.D365.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using Sample.D365.Core.Logging;

  /// <summary>
  /// The JsonSerializer class
  /// </summary>
  public static class ExceptionExtensions
  {
    /// <summary>
    /// Serialise an object
    /// </summary>
    /// <typeparam name="T">The object type</typeparam>
    /// <param name="value">The object</param>
    /// <returns>The serialised object</returns>
    public static string Serialise<T>(this T value)
      where T : Exception
    {
      using (MemoryStream stream = new MemoryStream())
      {
        var dictionary = new Dictionary<string, object>() {
          { "ClassName", value.GetType().FullName },
          { "Message", value.Message },
          { "StackTraceString", value.StackTrace },
          { "RemoteStackIndex", "0" },
          { "RemoteStackTraceString", null },
          { "ExceptionMethod", value.TargetSite?.Name },
          { "HResult", value.HResult.ToString(CultureInfo.InvariantCulture) },
          { "HelpURL", value.HelpLink },
          { "Source", value.Source },
          { "InnerException",  null },
        };

        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Dictionary<string, object>), new DataContractJsonSerializerSettings
        {
          UseSimpleDictionaryFormat = true,
          KnownTypes = new List<Type>() {
            typeof(Dictionary<string, object>),
          },
        });
        serializer.WriteObject(stream, dictionary);
        return Encoding.UTF8.GetString(stream.ToArray());
      }
    }

    /// <summary>
    /// Serialise the DynamicsException to json
    /// </summary>
    /// <param name="value">The DynamicsException to serialise</param>
    /// <returns>The Json string</returns>
    public static string Serialise(this DynamicsException value)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DynamicsException), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
        serializer.WriteObject(stream, value);
        return Encoding.UTF8.GetString(stream.ToArray());
      }
    }

    /// <summary>
    /// Deserialise a DynamicsException from a json string
    /// </summary>
    /// <param name="value">The DynamicsException to deserialise</param>
    /// <returns>An instance of DynamicsException</returns>
    public static DynamicsException Deserialise(this string value)
    {
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(DynamicsException), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
        return (DynamicsException)serializer.ReadObject(stream);
      }
    }
  }
}
