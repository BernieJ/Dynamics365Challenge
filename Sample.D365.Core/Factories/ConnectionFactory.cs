namespace BES.D365.Core.Factories
{
  using System;
  using System.Configuration;
  using Microsoft.Xrm.Sdk;
  using Microsoft.Xrm.Tooling.Connector;

  /// <summary>
  /// The ConnectionFactory
  /// </summary>
  public sealed class ConnectionFactory
  {
    /// <summary>
    /// Prevents a default instance of the <see cref="ConnectionFactory"/> class from being created.
    /// </summary>
    private ConnectionFactory()
    {
    }

    /// <summary>
    /// Gets a new organization service
    /// </summary>
    /// <param name="maxConnectionTimeout">The maximum connection timeout for the connection. The default setting is 2 minutes.</param>
    /// <param name="bypassPluginExecution">A value indicating whether to bypass plugin execution. Important: This will only apply if the caller has the prvBypassPlugins permission to bypass plugins. If its attempted without the permission requests will fault.</param>
    /// <returns>The organization service</returns>
    public static IOrganizationService GetOrganizationService(TimeSpan? maxConnectionTimeout = null, bool bypassPluginExecution = false)
    {
      CrmServiceClient.MaxConnectionTimeout = maxConnectionTimeout ?? TimeSpan.FromSeconds(120);

      CrmServiceClient crmServiceClient = new CrmServiceClient(ConfigurationManager.ConnectionStrings["CRM"].ConnectionString)
      {
        BypassPluginExecution = bypassPluginExecution,
      };

      if (crmServiceClient.IsReady)
      {
        return crmServiceClient;
      }
      else
      {
        throw crmServiceClient.LastCrmException;
      }
    }
  }
}
