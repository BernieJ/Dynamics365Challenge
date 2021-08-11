namespace Sample.D365.Core.Services
{
  using System;
  using System.Activities;
  using Microsoft.Xrm.Sdk;
  using Microsoft.Xrm.Sdk.Query;

  /// <summary>
  /// The LazyOrganizationService class
  /// </summary>
  public class LazyOrganizationService : IOrganizationService
  {
    private IOrganizationService organizationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyOrganizationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider</param>
    /// <param name="userId">The Id of the executing user</param>
    public LazyOrganizationService(IServiceProvider serviceProvider, Guid userId)
    {
      this.OrganizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
      this.UserId = userId;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LazyOrganizationService"/> class.
    /// </summary>
    /// <param name="codeActivityContext">The code activity context</param>
    /// <param name="userId">The Id of the executing user</param>
    public LazyOrganizationService(CodeActivityContext codeActivityContext, Guid userId)
    {
      this.OrganizationServiceFactory = codeActivityContext.GetExtension<IOrganizationServiceFactory>();
      this.UserId = userId;
    }

    /// <summary>
    /// Gets the organization service factory
    /// </summary>
    private IOrganizationServiceFactory OrganizationServiceFactory { get; }

    /// <summary>
    /// Gets the Id of the executing user
    /// </summary>
    private Guid? UserId { get; }

    /// <summary>
    /// Gets the organization service
    /// </summary>
    private IOrganizationService OrganizationService
    {
      get
      {
        if (this.organizationService == null)
        {
          this.organizationService = this.OrganizationServiceFactory.CreateOrganizationService(this.UserId);
        }

        return this.organizationService;
      }
    }

    /// <inheritdoc/>
    public void Associate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
    {
      this.OrganizationService.Associate(entityName, entityId, relationship, relatedEntities);
    }

    /// <inheritdoc/>
    public Guid Create(Entity entity)
    {
      return this.OrganizationService.Create(entity);
    }

    /// <inheritdoc/>
    public void Delete(string entityName, Guid id)
    {
      this.OrganizationService.Delete(entityName, id);
    }

    /// <inheritdoc/>
    public void Disassociate(string entityName, Guid entityId, Relationship relationship, EntityReferenceCollection relatedEntities)
    {
      this.OrganizationService.Disassociate(entityName, entityId, relationship, relatedEntities);
    }

    /// <inheritdoc/>
    public OrganizationResponse Execute(OrganizationRequest request)
    {
      return this.OrganizationService.Execute(request);
    }

    /// <inheritdoc/>
    public Entity Retrieve(string entityName, Guid id, ColumnSet columnSet)
    {
      return this.OrganizationService.Retrieve(entityName, id, columnSet);
    }

    /// <inheritdoc/>
    public EntityCollection RetrieveMultiple(QueryBase query)
    {
      return this.OrganizationService.RetrieveMultiple(query);
    }

    /// <inheritdoc/>
    public void Update(Entity entity)
    {
      this.OrganizationService.Update(entity);
    }
  }
}
