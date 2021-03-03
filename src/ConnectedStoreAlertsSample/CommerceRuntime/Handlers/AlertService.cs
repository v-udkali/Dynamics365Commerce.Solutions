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
    namespace CommerceRuntime.Service
    {
        using CommerceRuntime.Proxy;
        using CommerceRuntime.DataModel;
        using CommerceRuntime.Messages;
        using Microsoft.Dynamics.Commerce.Runtime;
        using Microsoft.Dynamics.Commerce.Runtime.Messages;
        using Microsoft.Xrm.Sdk;
        using Microsoft.Xrm.Tooling.Connector;
        using System;
        using System.Collections.Generic;
        using System.Globalization;
        using System.Threading.Tasks;

        /// <summary>
        /// Sample service to demonstrate returning an array of a new entity.
        /// </summary>
        public class AlertService : IRequestHandlerAsync
        {
            private const string EntityName = "msdyn_alert";
            private const string Id = "msdyn_alertid";
            private const string Name = "msdyn_name";
            private const string Description = "msdyn_description";
            private const string Time = "msdyn_timestamp";

            /// <summary>
            /// Gets the collection of supported request types by this handler.
            /// </summary>
            public IEnumerable<Type> SupportedRequestTypes
            {
                get
                {
                    return new[]
                    {
                        typeof(GetAlertsServiceRequest),
                    };
                }
            }

            /// <summary>
            /// Entry point to AlertsService service.
            /// </summary>
            /// <param name="request">The request to execute.</param>
            /// <returns>Result of executing request, or null object for void operations.</returns>
            public async Task<Response> Execute(Request request)
            {
                if (request == null)
                {
                    throw new ArgumentNullException("request");
                }

                Type reqType = request.GetType();
                if (reqType == typeof(GetAlertsServiceRequest))
                {
                    return await this.GetAlerts((GetAlertsServiceRequest)request);
                }
                else
                {
                    string message = string.Format(CultureInfo.InvariantCulture, "Request '{0}' is not supported.", reqType);
                    Console.WriteLine(message);
                    throw new NotSupportedException(message);
                }
            }

            /// <summary>
            /// Reads the alerts from CDS and returns the alert entity collection.
            /// </summary>
            /// <param name="request"></param>
            /// <returns></returns>
            private async Task<Response> GetAlerts(GetAlertsServiceRequest request)
            {
                ThrowIf.Null(request, "request");

                List<Alert> alerts = new List<Alert>();
                DataCollection<Entity> alertEntityCollection = await this.ReadCDSEntity(request.RequestContext, EntityName, DateTimeOffset.Now.AddDays(-20.0).DateTime, request.StoreNumber);
                if (alertEntityCollection.Count > 0)
                {
                    foreach (var entity in alertEntityCollection)
                    {
                        alerts.Add(new Alert() { Id = (Guid)entity.Attributes[Id], Name = entity.Attributes[Name].ToString(), Description = entity.Attributes[Description].ToString(), CreatedDateTime = entity.Attributes[Time].ToString() });
                    }
                }

                return new GetAlertsServiceResponse(alerts.AsPagedResult());
            }

            /// <summary>
            /// Reads the alerts from CDS.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="fromDate"></param>
            /// <returns></returns>
            private async Task<DataCollection<Entity>> ReadCDSEntity(RequestContext context, string entityName, DateTime fromDate, string venue)
            {
                DataCollection<Entity> entityCollection = new EntityCollection().Entities;
                string connectionString = CdsAuthenticationManager.GetConnectionString(context);

                if (string.IsNullOrEmpty(entityName))
                    return entityCollection;

                using (CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString))
                {
                    var runtime = new CdsRuntime(crmServiceClient);

                    var entities = await runtime.ReadEntities(entityName, fromDate, venue);

                    if (entities.Entities != null)
                    {
                        entityCollection = entities.Entities;
                        return entityCollection;
                    }

                }
                return entityCollection;
            }

        }
    }
}
