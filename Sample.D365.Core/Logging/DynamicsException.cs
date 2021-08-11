namespace Sample.D365.Core.Logging
{
  using System;
  using System.Collections.Generic;

  /// <summary>
  /// The DynamicsException class
  /// </summary>
  public class DynamicsException
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="DynamicsException"/> class.
    /// </summary>
    public DynamicsException()
      : base()
    {
    }

    /// <summary>
    /// Gets or sets the entity id
    /// </summary>
    public Guid EntityId { get; set; }

    /// <summary>
    /// Gets or sets the user id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the initiating user id
    /// </summary>
    public Guid IntiatingUserId { get; set; }

    /// <summary>
    /// Gets or sets the source class
    /// </summary>
    public string SourceClass { get; set; }

    /// <summary>
    /// Gets or sets the entity type
    /// </summary>
    public string EntityType { get; set; }

    /// <summary>
    /// Gets or sets the entity message
    /// </summary>
    public string EntityMessageName { get; set; }

    /// <summary>
    /// Gets or sets the source location
    /// </summary>
    public SourceLocation SourceLocation { get; set; }

    /// <summary>
    /// Gets the traces
    /// </summary>
    public List<string> Traces { get; } = new List<string>();

    /// <summary>
    /// Gets or sets the associated exceptions
    /// </summary>
    public List<string> SerializedExceptions { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the source
    /// </summary>
    public string Source { get; set; }
  }
}
