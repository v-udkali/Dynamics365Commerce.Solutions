/**
 * SAMPLE CODE NOTICE
 * 
 * THIS SAMPLE CODE IS MADE AVAILABLE AS IS.  MICROSOFT MAKES NO WARRANTIES, WHETHER EXPRESS OR IMPLIED,
 * OF FITNESS FOR A PARTICULAR PURPOSE, OF ACCURACY OR COMPLETENESS OF RESPONSES, OF RESULTS, OR CONDITIONS OF MERCHANTABILITY.
 * THE ENTIRE RISK OF THE USE OR THE RESULTS FROM THE USE OF THIS SAMPLE CODE REMAINS WITH THE USER.
 * NO TECHNICAL SUPPORT IS PROVIDED.  YOU MAY NOT DISTRIBUTE THIS CODE UNLESS YOU HAVE A LICENSE AGREEMENT WITH MICROSOFT THAT ALLOWS YOU TO DO SO.
 */

namespace Contoso.Controllers
{
    using CommerceRuntime.Messages;
    using Microsoft.Dynamics.Commerce.Runtime;
    using Microsoft.Dynamics.Commerce.Runtime.DataModel;
    using Microsoft.Dynamics.Commerce.Runtime.Hosting.Contracts;
    using System.Threading.Tasks;
    using DataModel = CommerceRuntime.DataModel;

    /// <summary>
    /// The controller to retrieve a alert entity.
    /// </summary>
    [RoutePrefix("Alerts")]
    [BindEntity(typeof(DataModel.Alert))]
    public class AlertsController : IController
    {
        /// <summary>
        /// Gets the alerts for a given store.
        /// </summary>
        /// <param name="StoreNumber">Store number for which the alerts generated.</param>
        /// <param name="dateTime">Alerts generated during this date time.</param>
        /// <returns>The list of alerts.</returns>
        [HttpPost]
        [Authorization(CommerceRoles.Anonymous, CommerceRoles.Customer, CommerceRoles.Device, CommerceRoles.Employee)]
        public async Task<PagedResult<DataModel.Alert>> GetAlertsByStore(IEndpointContext context, string StoreNumber)
        {
            QueryResultSettings queryResultSettings = QueryResultSettings.AllRecords;
            queryResultSettings.Paging = new PagingInfo(25);

            var request = new GetAlertsServiceRequest(StoreNumber) { QueryResultSettings = queryResultSettings };
            var alertsResponse = await context.ExecuteAsync<GetAlertsServiceResponse>(request).ConfigureAwait(false);
            return alertsResponse.Alerts;
        }
    }
}