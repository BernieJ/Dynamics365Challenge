using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Sample.D365.Workflows
{
    /// <summary>
    /// The ILocalWorkflowContext interface
    /// </summary>
    public interface ILocalWorkflowContext
    {
        /// <summary>
        /// Gets the organization service
        /// </summary>
        IOrganizationService OrganizationService { get; }

        /// <summary>
        /// Gets the workflow execution context
        /// </summary>
        IWorkflowContext WorkflowExecutionContext { get; }

        /// <summary>
        /// Gets the tracing service
        /// </summary>
        ITracingService TracingService { get; }

        /// <summary>
        /// Writes a trace message to the CRM trace log
        /// </summary>
        /// <param name="message">The message</param>
        void Trace(string message);
    }
}
