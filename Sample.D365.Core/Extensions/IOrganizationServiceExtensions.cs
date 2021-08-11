namespace Microsoft.Xrm.Sdk
{
  using Microsoft.Xrm.Sdk.Query;

  /// <summary>
  /// The IOrganizationServiceExtensions class
  /// </summary>
  public static class IOrganizationServiceExtensions
  {
    /// <summary>
    /// Retrieves a collection of records using paging
    /// </summary>
    /// <param name="service">An IOrganisationService instance</param>
    /// <param name="query">A query that determines the set of records to retrieve.</param>
    /// <returns>The collection of entities returned from the query.</returns>
    public static EntityCollection RetrieveMultiplePaged(this IOrganizationService service, QueryExpression query)
    {
      if (query.PageInfo == null)
      {
        query.PageInfo = new PagingInfo
        {
          Count = 4000,
          PageNumber = 1,
          PagingCookie = null,
        };
      }

      EntityCollection results;
      EntityCollection entities = new EntityCollection();
      do
      {
        results = service.RetrieveMultiple(query);
        entities.Entities.AddRange(results.Entities);

        query.PageInfo.PageNumber++;
        query.PageInfo.PagingCookie = results.PagingCookie;
      }
      while (results.MoreRecords);

      return entities;
    }
  }
}