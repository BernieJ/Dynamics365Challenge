namespace Sample.D365.Core.Logging
{
  using System;
  using System.Collections.Generic;
  using System.Net;
  using System.Net.Http;
  using System.Text;
  using Sample.D365.Core.Helpers;
  using Microsoft.Xrm.Sdk;

  /// <summary>
  /// The DynamicsLogger class
  /// </summary>
  public class DynamicsLogger : IDynamicsLogger
  {
    private static HttpClient httpClient;
    private readonly SourceLocation sourceLocation;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicsLogger"/> class.
    /// </summary>
    /// <param name="service">The organisation service</param>
    /// <param name="sourceLocation">The logical location of the logger</param>
    /// <param name="azureFunctionUrl">This url to the logging azure function</param>
    public DynamicsLogger(IOrganizationService service, SourceLocation sourceLocation)
    {
      this.sourceLocation = sourceLocation;
      httpClient = new HttpClient();
      // Recommendation by Microsoft to turn off keep-alive,
      // keep-alive might cause a plugin to wait for the connection to close even if it was completed
      httpClient.DefaultRequestHeaders.Connection.Add("close");
    }

    /// <summary>
    /// Logs an Exception to App Insights
    /// </summary>
    /// <param name="ex">The exception to log</param>
    /// <param name="tracingService">The tracing service containing additional information</param>
    public void Log(Exception ex, ITracingService tracingService)
    {
        var exception = new DynamicsException()
        {
          SourceLocation = this.sourceLocation,
          Source = "CRM",
        };

      // this is where you add custom logging
    }

    private List<string> ReadExceptions(Exception ex, List<string> list)
    {
      list.Add(ex.Serialise());

      if (ex.InnerException != null)
      {
        return this.ReadExceptions(ex.InnerException, list);
      }
      else
      {
        return list;
      }
    }
  }
}
