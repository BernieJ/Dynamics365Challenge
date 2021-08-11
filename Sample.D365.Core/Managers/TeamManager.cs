namespace Sample.D365.Core.Managers
{
  using System;
  using Sample.D365.Entities;
  using Microsoft.Xrm.Sdk;
  using Microsoft.Xrm.Sdk.Query;

  /// <summary>
  /// The TeamManager Class
  /// </summary>
  public sealed class TeamManager
  {
    /// <summary>
    /// Prevents a default instance of the <see cref="TeamManager"/> class from being created.
    /// </summary>
    private TeamManager()
    {
    }

    /// <summary>
    /// Get the Default Queue from a Team
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="tracingService">The tracing service</param>
    /// <param name="teamId">The Team Guid</param>
    /// <returns>The Default Queue</returns>
    public static EntityReference GetTeamDefaultQueue(IOrganizationService service, ITracingService tracingService, Guid teamId)
    {
      Team team = service.Retrieve(Team.EntityLogicalName, teamId, new ColumnSet(Team.Fields.QueueId)).ToEntity<Team>();
      return team.QueueId;
    }
  }
}
