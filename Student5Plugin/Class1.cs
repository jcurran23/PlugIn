using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Net;

// Microsoft Dynamics CRM namespace(s)
using Microsoft.Xrm.Sdk;
using System.ServiceModel;

namespace Student5Plugin
{
    public class FollowupPlugin : IPlugin
    {
        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an account.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "account")
                    return;

                try
                {

                    //    var response = entity.GetAttributeValue<string>(");
                    //  UpdateAPI(entity.Id.ToString(), "FREDE");

                    Entity followup = new Entity("task");

                    followup["subject"] = "Send e-mail to the new customer.";
                    followup["description"] =
                        "Follow up with the customer. Check if there are any new issues that need resolution.";
                    followup["scheduledstart"] = DateTime.Now.AddDays(7);
                    followup["scheduledend"] = DateTime.Now.AddDays(7);
                    //followup["category"] = context.PrimaryEntityName;

                    // Refer to the account in the task activity.
                    if (context.OutputParameters.Contains("id"))
                    {
                        Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                        string regardingobjectidType = "account";

                        followup["regardingobjectid"] =
                        new EntityReference(regardingobjectidType, regardingobjectid);
                    }

                    // Obtain the organization service reference.
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    // Create the task in Microsoft Dynamics CRM.
                    tracingService.Trace("FollowupPlugin: Creating the task activity.");
                    service.Create(followup);


                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                    throw;
                }


            }
        }
        public void UpdateAPI(string Id, string response)
        {
            var updateData = "{'InquiryId': " + Id + ", 'Response': " + response + "}";
            var client = new WebClient();
            FaultException ex = new FaultException();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            throw new InvalidPluginExecutionException("An error 11111 occurred in the FollowupPlugin plug-in.", ex);
            //client.UploadStringAsync(new Uri("http://s4.fredp613.com/api/v1/inquries/" + Id), "PUT", updateData);
        }
    }

    public class FollowupPluginInquiry : IPlugin
    {
        /// <summary>
        /// A plug-in that creates a follow-up task activity when a new account is created.
        /// </summary>
        /// <remarks>Register this plug-in on the Create message, account entity,
        /// and asynchronous mode.
        /// </remarks>
        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service for use in debugging sandboxed plug-ins.
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            // The InputParameters collection contains all the data passed in the message request.
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.
                Entity entity = (Entity)context.InputParameters["Target"];

                // Verify that the target entity represents an identity.
                // If not, this plug-in was not registered correctly.
                if (entity.LogicalName != "jmc_inquiry")
                    return;

                try
                {

                    //    var response = entity.GetAttributeValue<string>(");
                    //  UpdateAPI(entity.Id.ToString(), "FREDE");

                    Entity followup = new Entity("task");

                    followup["subject"] = "Inquiry created";
                    followup["description"] = "Respond to the client.";
                    followup["scheduledstart"] = DateTime.Now.AddDays(1);
                    followup["scheduledend"] = DateTime.Now.AddDays(1);
                    //followup["category"] = context.PrimaryEntityName;

                    // Refer to the account in the task activity.
                    if (context.OutputParameters.Contains("id"))
                    {
                        Guid regardingobjectid = new Guid(context.OutputParameters["id"].ToString());
                        string regardingobjectidType = "jmc_inquiry";

                        followup["regardingobjectid"] =
                        new EntityReference(regardingobjectidType, regardingobjectid);
                    }

                    // Obtain the organization service reference.
                    IOrganizationServiceFactory serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

                    // Create the task in Microsoft Dynamics CRM.
                    tracingService.Trace("FollowupPlugin: Creating the task activity.");
                    service.Create(followup);


                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in the FollowupPlugin plug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("FollowupPlugin: {0}", ex.ToString());
                    throw;
                }


            }
        }
        public void UpdateAPI(string Id, string response)
        {
            var updateData = "{'InquiryId': " + Id + ", 'Response': " + response + "}";
            var client = new WebClient();
            FaultException ex = new FaultException();
            client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            throw new InvalidPluginExecutionException("An error 11111 occurred in the FollowupPlugin plug-in.", ex);
            //client.UploadStringAsync(new Uri("http://s4.fredp613.com/api/v1/inquries/" + Id), "PUT", updateData);
        }
    }
}
