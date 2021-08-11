using Microsoft.Xrm.Sdk;
using Sample.D365.Core.Logging;
using Sample.D365.Core.Services;
using System;

namespace Sample.D365.Plugins
{
    /// <summary>
    /// The LocalPluginContext class 
    /// </summary>
    public class LocalPluginContext : ILocalPluginContext
    {
        private IServiceEndpointNotificationService notificationService;
        private IOrganizationService organizationService;
        private IPluginExecutionContext pluginExecutionContext;
        private ITracingService tracingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPluginContext"/> class.
        /// </summary>
        private LocalPluginContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPluginContext"/> class.
        /// </summary>
        /// <param name="serviceProvider">An instance of an IServiceProvider implementation</param>
        public LocalPluginContext(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider ?? throw new InvalidPluginExecutionException("serviceProvider");
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
                    this.organizationService = new LazyOrganizationService(this.ServiceProvider, this.PluginExecutionContext.UserId);
                }

                return this.organizationService;
            }
        }

        /// <summary>
        /// Gets the plugin execution context
        /// </summary>
        public IPluginExecutionContext PluginExecutionContext
        {
            get
            {
                if (this.pluginExecutionContext == null)
                {
                    this.pluginExecutionContext = (IPluginExecutionContext)this.ServiceProvider.GetService(typeof(IPluginExecutionContext));
                }

                return this.pluginExecutionContext;
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
                    ITracingService tracingService = (ITracingService)this.ServiceProvider.GetService(typeof(ITracingService));
                    this.tracingService = new DynamicsTracingService(tracingService, this.PluginExecutionContext);
                }

                return this.tracingService;
            }
        }

        /// <summary>
        /// Gets the tracing service
        /// </summary>
        public IServiceEndpointNotificationService NotificationService
        {
            get
            {
                if (this.notificationService == null)
                {
                    this.notificationService = (IServiceEndpointNotificationService)this.ServiceProvider.GetService(typeof(IServiceEndpointNotificationService));
                }

                return this.notificationService;
            }
        }

        /// <summary>
        /// Gets the service provider
        /// </summary>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Writes a trace message to the CRM trace log.
        /// </summary>
        /// <param name="message">Message name to trace.</param>
        public void Trace(string message)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (this.PluginExecutionContext == null)
                {
                    this.TracingService.Trace(message);
                }
                else
                {
                    this.TracingService.Trace($"{message}, Correlation Id: {this.PluginExecutionContext.CorrelationId}, Initiating User: {this.PluginExecutionContext.InitiatingUserId}");
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
                return new DynamicsLogger(this.OrganizationService, SourceLocation.Plugin);
            }
            catch (Exception ex)
            {
                this.Trace($"Could not Initialise Logging: {ex}");
            }

            return null;
        }
    }
}
