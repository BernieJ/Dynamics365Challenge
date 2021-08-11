// <copyright file="WorkflowBase.cs" company="Singular Systems">
// Copyright (c) 2019 All Rights Reserved
// </copyright>
// <author>HP Inc.</author>
// <date>6/14/2019 2:34:46 PM</date>
// <summary>Implements the WorkFlowActivityBase.</summary>

using Microsoft.Xrm.Sdk;
using Sample.D365.Core.Logging;
using System;
using System.Activities;
using System.ServiceModel;

namespace Sample.D365.Workflows
{
    /// <summary>
    /// Plugin base class
    /// </summary>    
    public abstract class WorkFlowActivityBase : CodeActivity
    {
        /// <summary>
        /// Main entry point for the business logic that the workflow activity is to execute.
        /// </summary>
        /// <param name="codeActivityContext">The code activity context.</param>
        protected override void Execute(CodeActivityContext codeActivityContext)
        {
            if (codeActivityContext == null)
            {
                throw new InvalidPluginExecutionException("codeActivityContext");
            }

            LocalWorkflowContext localcontext = new LocalWorkflowContext(codeActivityContext);

            try
            {
                this.ExecuteCRMWorkFlowActivity(localcontext);
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
            }
        }

        /// <summary>
        /// Placeholder for a custom worflow activity implementation. 
        /// </summary>
        /// <param name="localcontext">The local context for the current workflow activity.</param>
        protected abstract void ExecuteCRMWorkFlowActivity(LocalWorkflowContext localcontext);
    }
}