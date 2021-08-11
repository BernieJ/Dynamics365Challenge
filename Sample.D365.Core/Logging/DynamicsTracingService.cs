namespace Sample.D365.Core.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Microsoft.Xrm.Sdk;

  /// <summary>
  /// The AzureTracingService class
  /// </summary>
  public class DynamicsTracingService : ITracingService
  {
    private readonly List<string> traces;
    private readonly IExecutionContext context;
    private readonly ITracingService tracingService;
    private readonly TextWriter writer;
    private readonly StringBuilder internalStringBuilder;

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicsTracingService"/> class.
    /// </summary>
    /// <param name="tracingService">The ITracingService instance</param>
    /// <param name="executionContext">The ExecutionContext</param>
    public DynamicsTracingService(ITracingService tracingService, IExecutionContext executionContext)
    {
      this.traces = new List<string>();
      this.internalStringBuilder = new StringBuilder();
      this.context = executionContext;
      this.tracingService = tracingService;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicsTracingService"/> class.
    /// </summary>
    /// <param name="writer">The TextWriter instance</param>
    /// <param name="executionContext">The ExecutionContext</param>
    public DynamicsTracingService(TextWriter writer, IExecutionContext executionContext)
    {
      this.traces = new List<string>();
      this.internalStringBuilder = new StringBuilder();
      this.context = executionContext;
      this.writer = writer;
    }

    /// <summary>
    /// Gets the list of traces logged
    /// </summary>
    public ReadOnlyCollection<string> Traces
    {
      get { return this.traces.AsReadOnly(); }
    }

    /// <summary>
    /// Gets the current execution context
    /// </summary>
    public IExecutionContext ExecutionContext
    {
      get { return this.context; }
    }

    /// <summary>
    /// Logs trace information to be included in the logs
    /// </summary>
    /// <param name="format">The string format</param>
    /// <param name="args">The args to replace in the string</param>
    public void Trace(string format, params object[] args)
    {
      this.internalStringBuilder.Clear();
      this.internalStringBuilder.AppendFormat(CultureInfo.InvariantCulture, format, args).AppendLine();
      this.traces.Add(this.internalStringBuilder.ToString());

      if (this.tracingService != null)
      {
        this.tracingService.Trace(format, args);
      }

      if (this.writer != null)
      {
        this.writer.WriteLine(format, args);
      }
    }

    /// <summary>
    /// Clears the traces list
    /// </summary>
    public void ClearTraces()
    {
      this.traces.Clear();
    }

    /// <summary>
    /// Return a string representation of all the traces
    /// </summary>
    /// <returns>The string</returns>
    public override string ToString()
    {
      return string.Join(Environment.NewLine, this.traces);
    }
  }
}
