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
    namespace CommerceRuntime.Messages
    {
        using CommerceRuntime.DataModel;
        using Microsoft.Dynamics.Commerce.Runtime;
        using Microsoft.Dynamics.Commerce.Runtime.Messages;
        using System.Runtime.Serialization;

        /// <summary>
        /// Defines a simple response class that holds the alerts.
        /// </summary>
        [DataContract]
        public sealed class GetAlertsServiceResponse : Response
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetAlertsServiceResponse"/> class.
            /// </summary>
            /// <param name="dayHours">The collection of store hours.</param>
            public GetAlertsServiceResponse(PagedResult<Alert> alerts)
            {
                this.Alerts = alerts;
            }

            /// <summary>
            /// Gets the alerts triggered as a paged result set.
            /// </summary>
            [DataMember]
            public PagedResult<Alert> Alerts { get; private set; }
        }
    }
}