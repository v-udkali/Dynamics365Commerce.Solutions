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
    namespace CommerceRuntime.Proxy
    {
        using Microsoft.Xrm.Sdk;
        using Microsoft.Xrm.Sdk.Messages;
        using Microsoft.Xrm.Sdk.Query;
        using Microsoft.Xrm.Tooling.Connector;
        using System;
        using System.Threading.Tasks;

        /// <summary>
        /// 
        /// </summary>
        internal class CdsRuntime
        {
            public CdsRuntime(CrmServiceClient crmServiceClient)
            {
                this.CrmServiceClient = crmServiceClient ?? throw new ArgumentNullException(nameof(crmServiceClient));
            }

            protected CrmServiceClient CrmServiceClient { get; }

            /// <summary>
            /// Read the entities from CDS
            /// </summary>
            /// <param name="entityName"></param>
            /// <param name="fromDate"></param>
            /// <returns></returns>
            public async Task<EntityCollection> ReadEntities(string entityName, DateTime fromDate, string venue)
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    throw new ArgumentNullException(nameof(entityName));
                }

                var query = new QueryExpression(entityName)
                {
                    ColumnSet = new ColumnSet(allColumns: true),
                };

                query.Criteria.AddCondition("createdon", ConditionOperator.OnOrAfter, fromDate.ToString("s"));

                RetrieveMultipleRequest request = new RetrieveMultipleRequest
                {
                    Query = query,
                };

                var response = (RetrieveMultipleResponse)this.CrmServiceClient.Execute(request);
                var collection = response.EntityCollection;
                return await Task.FromResult<EntityCollection>(collection);

            }
        }
    }
}