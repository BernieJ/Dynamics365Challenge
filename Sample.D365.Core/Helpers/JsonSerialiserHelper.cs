namespace Sample.D365.Core.Helpers
{
  using System.IO;
  using System.Runtime.Serialization.Json;
  using System.Text;

  /// <summary>
  /// The JsonSerializer class
  /// </summary>
  public class JsonSerialiserHelper
  {
    /// <summary>
    /// Serialise an object
    /// </summary>
    /// <typeparam name="T">The object type</typeparam>
    /// <param name="value">The object</param>
    /// <returns>The serialised object</returns>
    public static string Serialise<T>(T value)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
        serializer.WriteObject(stream, value);
        return Encoding.UTF8.GetString(stream.ToArray());
      }
    }

    /// <summary>
    /// Deserialise a json string
    /// </summary>
    /// <typeparam name="T">The object type</typeparam>
    /// <param name="json">The json string to deserialize</param>
    /// <returns>The deserialised object</returns>
    public static T Deserialise<T>(string json)
    {
      using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
      {
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true });
        return (T)serializer.ReadObject(stream);
      }
    }
  }
}
