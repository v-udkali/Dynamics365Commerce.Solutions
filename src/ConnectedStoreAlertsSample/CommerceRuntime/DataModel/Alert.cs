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
    namespace CommerceRuntime.DataModel
    {
        using Microsoft.Dynamics.Commerce.Runtime.ComponentModel.DataAnnotations;
        using Microsoft.Dynamics.Commerce.Runtime.DataModel;
        using System;
        using System.Runtime.Serialization;

        /// <summary>
        /// Defines a simple class that holds information about the alert.
        /// </summary>
        public class Alert : CommerceEntity
        {
            private const string NameColumn = "NAME";
            private const string DescriptionColumn = "DESCRIPTION";
            private const string CreatedDateTimeColumn = "CREATEDDATETIME";
            private const string IdColumn = "ID";

            /// <summary>
            /// Initializes a new instance of the <see cref="StoreDayHours"/> class.
            /// </summary>
            public Alert()
                : base("Alert")
            {
            }

            /// <summary>
            /// Gets or sets the day of the week.
            /// </summary>
            [DataMember]
            [Column(NameColumn)]
            public string Name
            {
                get { return (string)this[NameColumn]; }
                set { this[NameColumn] = value; }
            }

            /// <summary>
            /// Gets or sets the open time.
            /// </summary>
            [DataMember]
            [Column(DescriptionColumn)]
            public string Description
            {
                get { return (string)this[DescriptionColumn]; }
                set { this[DescriptionColumn] = value; }
            }

            /// <summary>
            /// Gets or sets the closing time.
            /// </summary>
            [DataMember]
            [Column(CreatedDateTimeColumn)]
            public string CreatedDateTime
            {
                get { return (string)this[CreatedDateTimeColumn]; }
                set { this[CreatedDateTimeColumn] = value; }
            }

            /// <summary>
            /// Gets or sets the id.
            /// </summary>
            [Key]
            [DataMember]
            [Column(IdColumn)]
            public Guid Id
            {
                get { return (Guid)this[IdColumn]; }
                set { this[IdColumn] = value; }
            }
        }
    }
}
