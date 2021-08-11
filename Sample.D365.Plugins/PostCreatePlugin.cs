using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.D365.Plugins
{
    public class PostCreatePlugin : PluginBase
    {
        public PostCreatePlugin() : base("Contact")
        {

        }

        protected override void ExecuteCrmPlugin(LocalPluginContext localcontext)
        {
            Entity followup = new Entity("task");

            followup["subject"] = "First follow up meeting";
            followup["description"] =
                "Create first follow up meeting with" + localcontext.PluginExecutionContext.PrimaryEntityName;
            followup["scheduledstart"] = DateTime.Now.AddDays(7);
            followup["scheduledend"] = DateTime.Now.AddDays(7);
            followup["category"] = localcontext.PluginExecutionContext.PrimaryEntityName;

            // Refer to the account in the task activity.
            if (localcontext.PluginExecutionContext.OutputParameters.Contains("id"))
            {
                Guid regardingobjectid = new Guid(localcontext.PluginExecutionContext.OutputParameters["id"].ToString());
                string regardingobjectidType = "contact";

                followup["regardingobjectid"] =
                new EntityReference(regardingobjectidType, regardingobjectid);
            }


            localcontext.TracingService.Trace("FollowupPlugin: Creating the task activity.");
            localcontext.OrganizationService.Create(followup);
        }
    }
}
