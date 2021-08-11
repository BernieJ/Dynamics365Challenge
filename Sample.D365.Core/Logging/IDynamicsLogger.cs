namespace Sample.D365.Core.Logging
{
  using System;
  using Microsoft.Xrm.Sdk;

  /// <summary>
  /// The IDynamicsLogger interface
  /// </summary>
  public interface IDynamicsLogger
  {
    /// <summary>
    /// Logs an exception to App Insights
    /// </summary>
    /// <param name="ex">The exception to log</param>
    /// <param name="tracingService">The tracing service containing additional information</param>
    void Log(Exception ex, ITracingService tracingService = null);
  }
}
