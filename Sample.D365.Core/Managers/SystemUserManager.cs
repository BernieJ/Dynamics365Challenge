namespace Sample.D365.Core.Managers
{
  using System;
  using System.Collections.Concurrent;
  using System.Collections.Generic;
  using System.Linq;
  using Sample.D365.Entities;
  using Microsoft.Xrm.Sdk;
  using Microsoft.Xrm.Sdk.Query;

  /// <summary>
  /// The SystemUserManager class
  /// </summary>
  public sealed class SystemUserManager
  {
    private const int CacheValidMinutes = 5;

    private static readonly object LockSystemUserRoleCache = new object();

    private static readonly object LockSystemUserTeamCache = new object();

    private SystemUserManager()
    {
    }

    /// <summary>
    /// Gets the system user role cache
    /// </summary>
    private static ConcurrentDictionary<Guid, HashSet<string>> SystemUserRoleCache { get; } = new ConcurrentDictionary<Guid, HashSet<string>>();

    /// <summary>
    /// Gets or sets the time when the system user role cache expires
    /// </summary>
    private static DateTime? SystemUserRoleCacheExpiry { get; set; } = DateTime.Now.AddMinutes(CacheValidMinutes);

    /// <summary>
    /// Gets the system user team cache
    /// </summary>
    private static ConcurrentDictionary<Guid, List<Team>> SystemUserTeamCache { get; } = new ConcurrentDictionary<Guid, List<Team>>();

    /// <summary>
    /// Gets or sets the time when the system user team cache expires
    /// </summary>
    private static DateTime? SystemUserTeamCacheExpiry { get; set; } = DateTime.Now.AddMinutes(CacheValidMinutes);

    #region Roles

    /// <summary>
    /// Gets the roles for the supplied user
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>The user's roles</returns>
    public static HashSet<string> GetUserRoles(IOrganizationService service, Guid systemUserId, bool ignoreCache = false)
    {
      lock (LockSystemUserRoleCache)
      {
        HashSet<string> userRoles;
        if (ignoreCache || DateTime.Now > SystemUserRoleCacheExpiry)
        {
          SystemUserRoleCache.TryRemove(systemUserId, out userRoles);
          SystemUserRoleCacheExpiry = DateTime.Now.AddMinutes(CacheValidMinutes);
        }

        userRoles = SystemUserRoleCache.GetOrAdd(systemUserId, x =>
          new HashSet<string>(service.RetrieveMultiple(new QueryExpression
          {
            EntityName = Role.EntityLogicalName,
            ColumnSet = new ColumnSet(Role.Fields.RoleId, Role.Fields.Name),
            LinkEntities =
            {
              new LinkEntity
              {
                LinkFromEntityName = Role.EntityLogicalName,
                LinkFromAttributeName = Role.Fields.RoleId,
                LinkToEntityName = SystemUserRoles.EntityLogicalName,
                LinkToAttributeName = SystemUserRoles.Fields.RoleId,
                LinkCriteria = new FilterExpression
                {
                  FilterOperator = LogicalOperator.And,
                  Conditions =
                  {
                    new ConditionExpression(SystemUserRoles.Fields.SystemUserId, ConditionOperator.Equal, systemUserId),
                  },
                },
              },
            },
          }).Entities.Select(entity => (string)entity[Role.Fields.Name])));

        return userRoles;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user has the supplied role
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="roleName">The name of the role</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>A value indicating whether the supplied user has the supplied role</returns>
    public static bool UserHasRole(IOrganizationService service, Guid systemUserId, string roleName, bool ignoreCache = false)
    {
      HashSet<string> userRoles = GetUserRoles(service, systemUserId, ignoreCache);
      return userRoles.Contains(roleName);
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user has any of the supplied roles
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="roleNames">The name of the roles</param>
    /// <returns>A value indicating whether the supplied user has any of the supplied roles</returns>
    public static bool UserHasRole(IOrganizationService service, Guid systemUserId, params string[] roleNames)
    {
      HashSet<string> userRoles = GetUserRoles(service, systemUserId);
      return roleNames.Any(roleName => userRoles.Contains(roleName));
    }

    #endregion

    #region Teams

    /// <summary>
    /// Gets the teams for the supplied user
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>The user's teams</returns>
    public static List<Team> GetUserTeams(IOrganizationService service, Guid systemUserId, bool ignoreCache = false)
    {
      lock (LockSystemUserTeamCache)
      {
        List<Team> userTeams;
        if (ignoreCache || DateTime.Now > SystemUserTeamCacheExpiry)
        {
          SystemUserTeamCache.TryRemove(systemUserId, out userTeams);
          SystemUserTeamCacheExpiry = DateTime.Now.AddMinutes(CacheValidMinutes);
        }

        userTeams = SystemUserTeamCache.GetOrAdd(systemUserId, x =>
          service.RetrieveMultiple(new QueryExpression
          {
            EntityName = Team.EntityLogicalName,
            ColumnSet = new ColumnSet(Team.Fields.TeamId, Team.Fields.Name),
            LinkEntities =
            {
              new LinkEntity
              {
                LinkFromEntityName = Team.EntityLogicalName,
                LinkFromAttributeName = Team.Fields.TeamId,
                LinkToEntityName = TeamMembership.EntityLogicalName,
                LinkToAttributeName = TeamMembership.Fields.TeamId,
                LinkCriteria = new FilterExpression
                {
                  FilterOperator = LogicalOperator.And,
                  Conditions =
                  {
                    new ConditionExpression(TeamMembership.Fields.SystemUserId, ConditionOperator.Equal, systemUserId),
                  },
                },
              },
            },
          }).Entities.Select(entity => entity.ToEntity<Team>()).ToList());

        return userTeams;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user is a member of the supplied team
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="teamId">The Id of the team</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>A value indicating whether the supplied user is a member of the supplied team</returns>
    public static bool UserIsMemberOfTeam(IOrganizationService service, Guid systemUserId, Guid teamId, bool ignoreCache = false)
    {
      List<Team> userTeams = GetUserTeams(service, systemUserId, ignoreCache);
      return userTeams.Any(t => t.TeamId == teamId);
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user is a member of any of the supplied teams
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="teamIds">The Id of the team</param>
    /// <returns>A value indicating whether the supplied user is a member of any of the supplied teams</returns>
    public static bool UserIsMemberOfTeam(IOrganizationService service, Guid systemUserId, params Guid[] teamIds)
    {
      List<Team> userTeams = GetUserTeams(service, systemUserId);
      return teamIds.Any(teamId => userTeams.Any(t => t.TeamId == teamId));
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user is a member of the supplied team
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="teamName">The name of the team</param>
    /// <param name="ignoreCache">A value indicating whether to ignore the cache</param>
    /// <returns>A value indicating whether the supplied user is a member of the supplied team</returns>
    public static bool UserIsMemberOfTeam(IOrganizationService service, Guid systemUserId, string teamName, bool ignoreCache = false)
    {
      List<Team> userTeams = GetUserTeams(service, systemUserId, ignoreCache);
      return userTeams.Any(t => t.Name == teamName);
    }

    /// <summary>
    /// Gets a value indicating whether the supplied user is a member of any of the supplied teams
    /// </summary>
    /// <param name="service">The organization service</param>
    /// <param name="systemUserId">The Id of the system user</param>
    /// <param name="teamNames">The names of the teams</param>
    /// <returns>A value indicating whether the supplied user is a member of any of the supplied teams</returns>
    public static bool UserIsMemberOfTeam(IOrganizationService service, Guid systemUserId, params string[] teamNames)
    {
      List<Team> userTeams = GetUserTeams(service, systemUserId);
      return teamNames.Any(teamName => userTeams.Any(t => t.Name == teamName));
    }

    #endregion

    /// <summary>
    /// The Roles class
    /// </summary>
    public static class Roles
    {
#pragma warning disable SA1600 // Elements should be documented
      public const string SystemAdministrator = "System Administrator";
#pragma warning restore SA1600 // Elements should be documented
    }

    /// <summary>
    /// The Teams class
    /// </summary>
    public static class Teams
    {
#pragma warning disable SA1600 // Elements should be documented

#pragma warning restore SA1600 // Elements should be documented

    }
  }
}
