// <copyright file="PluginBase.cs" company="Singular Systems">
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>HP Inc.</author>
// <date>6/14/2019 2:34:46 PM</date>
// <summary>Implements the PluginBase.</summary>

using Microsoft.Xrm.Sdk;
using Sample.D365.Core.Logging;
using System;
using System.ServiceModel;

namespace Sample.D365.Plugins
{
    /// <summary>
    /// Plugin base class
    /// </summary>    
    public abstract class PluginBase : IPlugin
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginBase"/> class.
        /// </summary>
        /// <param name="childClassName">The <see cref=" cred="Type"/> of the derived class.</param>
        protected PluginBase(Type childClassName)
        {
            this.ChildClassName = childClassName.ToString();
        }

        protected PluginBase(string childClassName)
        {
            this.ChildClassName = childClassName.ToString();
        }

        /// <summary>
        /// Gets the name of the child class.
        /// </summary>
        protected string ChildClassName { get; private set; }

        /// <summary>
        /// Main entry point for the business logic that the plug-in is to execute.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new InvalidPluginExecutionException("serviceProvider");
            }

            LocalPluginContext localcontext = new LocalPluginContext(serviceProvider);

            localcontext.Trace($"Entered {this.ChildClassName}.Execute()");

            try
            {
                this.ExecuteCrmPlugin(localcontext);
            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                localcontext.Trace($"Exception: {e.ToString()}");
                Logger.Log(e, localcontext.TracingService);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Log(ex, localcontext.TracingService);
                throw;
            }
            finally
            {
                localcontext.Trace($"Exiting {this.ChildClassName}.Execute()");
            }
        }

        /// <summary>
        /// Placeholder for a custom plug-in implementation. 
        /// </summary>
        /// <param name="localcontext">The local context for the current plug-in.</param>
        protected abstract void ExecuteCrmPlugin(LocalPluginContext localcontext);
    }
}