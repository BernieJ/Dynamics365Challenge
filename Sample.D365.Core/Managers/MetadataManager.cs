namespace Sample.D365.Core.Managers
{
  using System;
  using System.Collections.Concurrent;
  using System.Linq;
  using Sample.D365.Entities;
  using Microsoft.Xrm.Sdk;
  using Microsoft.Xrm.Sdk.Messages;
  using Microsoft.Xrm.Sdk.Metadata;

  /// <summary>
  /// The MetadataManager class
  /// </summary>
  public sealed class MetadataManager
  {
    private const int CacheValidMinutes = 30;

    private static readonly object LockEntityMetadataCache = new object();

    private static readonly object LockAttributeMetadataCache = new object();

    private MetadataManager()
    {
    }

    /// <summary>
    /// Gets the entity metadata cache
    /// </summary>
    private static ConcurrentDictionary<string, EntityMetadata> EntityMetadataCache { get; } = new ConcurrentDictionary<string, EntityMetadata>();

    /// <summary>
    /// Gets or sets the time when the entity metadata cache expires
    /// </summary>
    private static DateTime? EntityMetadataCacheExpiry { get; set; } = DateTime.Now.AddMinutes(CacheValidMinutes);

    /// <summary>
    /// Gets the entity metadata cache
    /// </summary>
    private static ConcurrentDictionary<string, AttributeMetadata> AttributeMetadataCache { get; } = new ConcurrentDictionary<string, AttributeMetadata>();

    /// <summary>
    /// Gets or sets the time when the attribute metadata cache expires
    /// </summary>
    private static DateTime? AttributeMetadataCacheExpiry { get; set; } = DateTime.Now.AddMinutes(CacheValidMinutes);

    /// <summary>
    /// Get the metadata for the supplied entity
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="logicalName">The logical name of the entity</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>The metadata for the supplied entity</returns>
    public static EntityMetadata GetEntityMetadata(IOrganizationService service, string logicalName, bool ignoreCache = false)
    {
      lock (LockEntityMetadataCache)
      {
        string key = logicalName;
        EntityMetadata entityMetadata;

        if (ignoreCache || DateTime.Now > EntityMetadataCacheExpiry)
        {
          EntityMetadataCache.TryRemove(key, out entityMetadata);
          EntityMetadataCacheExpiry = DateTime.Now.AddMinutes(CacheValidMinutes);
        }

        entityMetadata = EntityMetadataCache.GetOrAdd(key, x => ((RetrieveEntityResponse)service.Execute(new RetrieveEntityRequest { LogicalName = logicalName, EntityFilters = EntityFilters.All })).EntityMetadata);

        return entityMetadata;
      }
    }

    /// <summary>
    /// Get the metadata for the supplied attribute
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="entityLogicalName">The logical name of the entity</param>
    /// <param name="logicalName">The logical name of the attribute</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>The metadata for the supplied attribute</returns>
    public static AttributeMetadata GetAttributeMetadata(IOrganizationService service, string entityLogicalName, string logicalName, bool ignoreCache = false)
    {
      lock (LockAttributeMetadataCache)
      {
        string key = $"{entityLogicalName}.{logicalName}";
        AttributeMetadata attributeMetadata;

        if (ignoreCache || DateTime.Now > AttributeMetadataCacheExpiry)
        {
          AttributeMetadataCache.TryRemove(key, out attributeMetadata);
          AttributeMetadataCacheExpiry = DateTime.Now.AddMinutes(CacheValidMinutes);
        }

        attributeMetadata = AttributeMetadataCache.GetOrAdd(key, x => ((RetrieveAttributeResponse)service.Execute(new RetrieveAttributeRequest { EntityLogicalName = entityLogicalName, LogicalName = logicalName, RetrieveAsIfPublished = true })).AttributeMetadata);

        return attributeMetadata;
      }
    }

    /// <summary>
    /// Gets the option set text
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="entityLogicalName">The logical name of the entity</param>
    /// <param name="logicalName">The logical name of the attribute</param>
    /// <param name="optionSetValue">The option set value</param>
    /// <returns>The option set text</returns>
    public static string GetOptionSetText(IOrganizationService service, string entityLogicalName, string logicalName, OptionSetValue optionSetValue)
    {
      if (optionSetValue != null)
      {
        return GetOptionSetText(service, entityLogicalName, logicalName, optionSetValue.Value);
      }

      return null;
    }

    /// <summary>
    /// Gets the option set text
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="entityLogicalName">The logical name of the entity</param>
    /// <param name="logicalName">The logical name of the attribute</param>
    /// <param name="optionSetValue">The option set value</param>
    /// <returns>The option set text</returns>
    public static string GetOptionSetText(IOrganizationService service, string entityLogicalName, string logicalName, int optionSetValue)
    {
      var picklistAttributeMetadata = (PicklistAttributeMetadata)MetadataManager.GetAttributeMetadata(service, entityLogicalName, logicalName);
      OptionMetadata optionMetadata = picklistAttributeMetadata.OptionSet.Options.FirstOrDefault(om => om.Value == optionSetValue);
      if (optionMetadata != null)
      {
        return optionMetadata.Label.UserLocalizedLabel.Label;
      }

      return null;
    }
  }
}