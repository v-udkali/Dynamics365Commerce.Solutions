/**
 * SAMPLE CODE NOTICE
 * 
 * THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
 * OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
 * THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
 * NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
 */

namespace Contoso
{
    namespace CommerceRuntime.Handlers
    {
        using CommerceRuntime.Messages;
        using Microsoft.Dynamics.Commerce.Runtime;
        using Microsoft.Dynamics.Commerce.Runtime.DataModel;
        using Microsoft.Dynamics.Commerce.Runtime.Messages;
        using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
        using System;

        public class NotificationExtensionService : SingleAsyncRequestHandler<GetNotificationsExtensionServiceRequest>
        {
            /// <summary>
            /// The handler for the <c>GetNotificationsExtensionServiceRequest</c> request.
            /// </summary>
            /// <param name="request">The request with the operation.</param>
            /// <returns>The notification details for the operation.</returns>
            protected override async System.Threading.Tasks.Task<Response> Process(GetNotificationsExtensionServiceRequest request)
            {
                ThrowIf.Null(request, "request");

                NotificationDetailCollection notificationDetails = new NotificationDetailCollection();
                try
                {
                    switch ((int)request.SubscribedOperation)
                    {
                        case 9000: //Connected store alert operation
                            notificationDetails.Add(this.GetStoreAlerts(request.RequestContext, DateTimeOffset.Now.AddDays(-20.0).DateTime));
                            break;
                    }
                }
                catch {
                    // log the exception to appinsights
                    notificationDetails.Add(NotificationExtensionService.GetNotificationErrorDetail());
                }

                var serviceResponse = new GetNotificationsExtensionServiceResponse(notificationDetails);
                return await System.Threading.Tasks.Task.FromResult(serviceResponse);
            }
            /// <summary>
            /// Gets the alerts from CDS
            /// </summary>
            /// <param name="context"></param>
            /// <param name="fromDate"></param>
            /// <returns></returns>
            private NotificationDetail GetStoreAlerts(RequestContext context, DateTime fromDate)
            {
                GetAlertsServiceRequest getAlertsServiceRequest = new GetAlertsServiceRequest(context.GetChannel().InventoryLocationId);
                GetAlertsServiceResponse response = context.Runtime.Execute<GetAlertsServiceResponse>((getAlertsServiceRequest), context);

                NotificationDetail notificationDetail = new NotificationDetail
                {
                    DisplayText = "Checkout queue needs attention",
                    IsNew = true,
                    IsSuccess = true,
                    ItemCount = response.Alerts.TotalCount.Value,
                    LastUpdatedDateTime = DateTimeOffset.UtcNow.DateTime,
                };

                return notificationDetail;
            }

            /// <summary>
            /// Gets the error NotificationDetail.
            /// </summary>
            /// <param name="actionProperty">Action property of the notification detail.</param>
            /// <param name="displayText">The display text of the notification.</param>
            /// <returns>The notification detail.</returns>
            public static NotificationDetail GetNotificationErrorDetail(string actionProperty = "", string displayText = "")
            {
                NotificationDetail notificationDetailItem = new NotificationDetail();

                if (!string.IsNullOrWhiteSpace(actionProperty))
                {
                    notificationDetailItem.ActionProperty = actionProperty;
                }

                if (!string.IsNullOrWhiteSpace(displayText))
                {
                    notificationDetailItem.DisplayText = displayText;
                }

                notificationDetailItem.ItemCount = 0;
                notificationDetailItem.IsNew = false;
                notificationDetailItem.IsSuccess = false;
                notificationDetailItem.IsLiveContentOnly = false;

                return notificationDetailItem;
            }
        }
    }
}