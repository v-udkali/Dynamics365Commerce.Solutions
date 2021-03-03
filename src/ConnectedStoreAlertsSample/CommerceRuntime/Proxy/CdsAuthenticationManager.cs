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
        using Microsoft.Dynamics.Commerce.Runtime;
        using Microsoft.Dynamics.Commerce.Runtime.Extensions;

        /// <summary>
        /// CDS authentication manager
        /// </summary>
        internal static class CdsAuthenticationManager
        {
            private const string ConnectionStringKeyName = "ext.CDS.ConnectionString";

            /// <summary>
            /// Gets the CDS connection string from the commerce runtime config file
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public static string GetConnectionString(RequestContext context)
            {
                string connectionString = context.Runtime.Configuration.GetSettingValue(ConnectionStringKeyName);
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new ConfigurationException(
                        ConfigurationErrors.Microsoft_Dynamics_Commerce_Runtime_ConnectionStringNotProvided,
                        $"CDS connection string is missing or empty for {ConnectionStringKeyName} key.");
                }

                return connectionString;
            }
        }
    }
}