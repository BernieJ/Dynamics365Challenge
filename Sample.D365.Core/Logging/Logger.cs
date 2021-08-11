namespace Sample.D365.Core.Logging
{
  using System;
  using Microsoft.Xrm.Sdk;

  /// <summary>
  /// The Logger class
  /// </summary>
  public static class Logger
  {
    private static IDynamicsLogger logger;

    /// <summary>
    /// The logger initialisation delegate
    /// </summary>
    /// <returns>The logger</returns>
    public delegate IDynamicsLogger InitialiseLogger();

    /// <summary>
    /// Initialise the logger
    /// </summary>
    /// <param name="initialiseLogger">The logger initialisation delegate</param>
    public static void Initialise(InitialiseLogger initialiseLogger)
    {
      if (logger == null)
      {
        logger = initialiseLogger();
      }
    }

    /// <summary>
    /// Logs an Exception to App Insights
    /// </summary>
    /// <param name="ex">The exception to log</param>
    /// <param name="tracingService">The tracing service containing additional information</param>
    public static void Log(Exception ex, ITracingService tracingService)
    {
      if (logger != null)
      {
        logger.Log(ex, tracingService);
      }
    }
  }
}
