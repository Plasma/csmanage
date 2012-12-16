//---------------------------------------------------------------------------------
// Microsoft (R) Windows Azure SDK
// Software Development Kit
// 
// Copyright (c) Microsoft Corporation. All rights reserved.  
//
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE. 
//---------------------------------------------------------------------------------

namespace Microsoft.Samples.WindowsAzure.ServiceManagement.Tools
{
    using System;

    using Microsoft.Samples.WindowsAzure.ServiceManagement;

    class ListOperatingSystemsCommmand : CSManageCommand
    {
        public override bool Validate()
        {
            return true;
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing Operating Systems");
            OperatingSystemList operatingSystems = channel.ListOperatingSystems(SubscriptionId);
            Utility.LogObject(operatingSystems);
        }
    }

    class ListOperatingSystemFamiliesCommmand : CSManageCommand
    {
        public override bool Validate()
        {
            return true;
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing Operating System Families");
            OperatingSystemFamilyList operatingSystemFamilies = channel.ListOperatingSystemFamilies(SubscriptionId);
            Utility.LogObject(operatingSystemFamilies);
        }
    }
}
