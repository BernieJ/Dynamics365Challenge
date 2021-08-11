using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Sample.D365.Core.Logging;
using Sample.D365.Core.Services;
using System;
using System.Activities;

namespace Sample.D365.Workflows
{
    /// <summary>
    /// The LocalWorkflowContext class
    /// </summary>
    public class LocalWorkflowContext : ILocalWorkflowContext
    {
        private IOrganizationService organizationService;
        private ITracingService tracingService;
        private IWorkflowContext workflowExecutionContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalWorkflowContext"/> class.
        /// </summary>
        private LocalWorkflowContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalWorkflowContext"/> class.
        /// </summary>
        /// <param name="codeActivityContext">The code activity context</param>
        public LocalWorkflowContext(CodeActivityContext codeActivityContext)
        {
            this.CodeActivityContext = codeActivityContext ?? throw new InvalidPluginExecutionException("codeActivityContext");

            Logger.Initialise(this.InitialiseLogger);
        }

        /// <summary>
        /// Gets the organization service
        /// </summary>
        public IOrganizationService OrganizationService
        {
            get
            {
                if (this.organizationService == null)
                {
                    this.organizationService = new LazyOrganizationService(this.CodeActivityContext, this.WorkflowExecutionContext.UserId);
                }

                return this.organizationService;
            }
        }

        /// <summary>
        /// Gets the workflow execution context
        /// </summary>
        public IWorkflowContext WorkflowExecutionContext
        {
            get
            {
                if (this.workflowExecutionContext == null)
                {
                    this.workflowExecutionContext = this.CodeActivityContext.GetExtension<IWorkflowContext>();
                }

                return this.workflowExecutionContext;
            }
        }

        /// <summary>
        /// Gets the tracing service
        /// </summary>
        public ITracingService TracingService
        {
            get
            {
                if (this.tracingService == null)
                {
                    ITracingService tracingService = this.CodeActivityContext.GetExtension<ITracingService>();
                    this.tracingService = new DynamicsTracingService(tracingService, this.WorkflowExecutionContext);
                }

                return this.tracingService;
            }
        }

        /// <summary>
        /// Gets the execution context
        /// </summary>
        public CodeActivityContext CodeActivityContext { get; }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (this.WorkflowExecutionContext == null)
                {
                    this.TracingService.Trace(message);
                }
                else
                {
                    this.TracingService.Trace($"{message}, Correlation Id: {this.WorkflowExecutionContext.CorrelationId}, Initiating User: {this.WorkflowExecutionContext.InitiatingUserId}");
                }
            }
        }

        /// <summary>
        /// Initialise the Logger
        /// </summary>
        /// <returns>The Logger</returns>
        private IDynamicsLogger InitialiseLogger()
        {
            try
            {
                return new DynamicsLogger(this.OrganizationService, SourceLocation.CustomWorkflowActivity);
            }
            catch (Exception ex)
            {
                this.Trace($"Could not Initialise Logging: {ex}");
            }

            return null;
        }
    }
}
