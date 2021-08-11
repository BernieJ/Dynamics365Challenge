namespace Sample.D365.Core.Managers
{
  using System;
  using System.Linq;
  using Microsoft.Xrm.Sdk;

  /// <summary>
  /// The EntityManager class
  /// </summary>
  public sealed class EntityManager
  {
    /// <summary>
    /// Prevents a default instance of the <see cref="EntityManager"/> class from being created.
    /// </summary>
    private EntityManager()
    {
    }

    /// <summary>
    /// Gets the supplied attribute from the first entity instance that contains the attribute
    /// </summary>
    /// <typeparam name="T">The type</typeparam>
    /// <param name="attributeName">The attribute name</param>
    /// <param name="entities">The entities</param>
    /// <returns>The supplied attribute from the first entity instance that contains the attribute</returns>
    public static T GetValue<T>(string attributeName, params Entity[] entities)
    {
      foreach (Entity entity in entities.Where(e => e != null))
      {
        if (entity.Contains(attributeName)) { return (T)entity[attributeName]; }
      }

      return default(T);
    }

    /// <summary>
    /// Checks whether a property of an entity has a value
    /// </summary>
    /// <param name="entity">The entity</param>
    /// <param name="attribute">The attribute</param>
    /// <returns>Whether a property of an entity has a value</returns>
    public static bool HasValue(Entity entity, string attribute)
    {
      return entity != null && entity.Attributes.Contains(attribute) && entity.Attributes[attribute] != null;
    }

    /// <summary>
    /// Returns a value indicating whether value of any of the supplied attributes have changed between two entity instances
    /// </summary>
    /// <param name="preEntity">The pre entity instance</param>
    /// <param name="postEntity">The post entity instance</param>
    /// <param name="attributes">The attributes to check</param>
    /// <returns>A value indicating whether value of any of the supplied attributes have changed between two entity instances</returns>
    public static bool HaveValuesChanged(Entity preEntity, Entity postEntity, params string[] attributes)
    {
      return attributes.Any(attribute => EntityManager.HasValueChanged(preEntity, postEntity, attribute));
    }

    /// <summary>
    /// Returns a value indicating whether the value of an attribute has changed between two entity instances
    /// </summary>
    /// <param name="preEntity">The pre entity instance</param>
    /// <param name="postEntity">The post entity instance</param>
    /// <param name="attribute">The attribute to check</param>
    /// <returns>A value indicating whether the value of an attribute has changed between two entity instances</returns>
    private static bool HasValueChanged(Entity preEntity, Entity postEntity, string attribute)
    {
      if (!postEntity.Contains(attribute)) { return false; }

      object valuePre = preEntity.Contains(attribute) ? preEntity[attribute] : null;
      object valuePost = postEntity[attribute];

      if (valuePre == null && valuePost == null) { return false; }
      if (valuePre == null && valuePost != null) { return true; }
      if (valuePre != null && valuePost == null) { return true; }

      if (valuePost is bool) { return (bool)valuePre != (bool)valuePost; }
      if (valuePost is DateTime) { return (DateTime)valuePre != (DateTime)valuePost; }
      if (valuePost is decimal) { return (decimal)valuePre != (decimal)valuePost; }
      if (valuePost is double) { return (double)valuePre != (double)valuePost; }
      if (valuePost is EntityReference) { return ((EntityReference)valuePre).Id != ((EntityReference)valuePost).Id; }
      if (valuePost is Guid) { return (Guid)valuePre != (Guid)valuePost; }
      if (valuePost is int) { return (int)valuePre != (int)valuePost; }
      if (valuePost is Money) { return ((Money)valuePre).Value != ((Money)valuePost).Value; }
      if (valuePost is OptionSetValue) { return ((OptionSetValue)valuePre).Value != ((OptionSetValue)valuePost).Value; }
      if (valuePost is OptionSetValueCollection) { return ((OptionSetValueCollection)valuePre).Select(optionSetValue => optionSetValue.Value).SequenceEqual(((OptionSetValueCollection)valuePost).Select(optionSetValue => optionSetValue.Value)); }
      if (valuePost is string) { return (string)valuePre != (string)valuePost; }

      throw new InvalidOperationException($"Unhandled type: {valuePost.GetType()}");
    }
  }
}
