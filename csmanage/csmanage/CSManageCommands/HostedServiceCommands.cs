﻿//---------------------------------------------------------------------------------
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

    public partial class CSManageCommand
    {
     
        public static string HostedServiceName { get; set; }
        
        public static bool ValidateHostedServiceName()
        {
            return ValidateHostedServiceName(true);
        }

        public static bool ValidateHostedServiceLabelAndOrDescription()
        {
            if(!( ValidateDescription(false) || ValidateLabel(false)) )
            {
                Console.WriteLine("Label and/or description should be specified.");
                return false;
            }
            return true;
        }

        public static string LocationConstraintName { get; set; }
        public static bool ValidateLocationConstraintNameOrAffinityGroupName()
        {
            if(!String.IsNullOrEmpty(LocationConstraintName) && !String.IsNullOrEmpty(AffinityGroupName) )
            {
                Console.WriteLine("Please specify only one of location or affinity-group and not both.");
                return false;
            }

            if(String.IsNullOrEmpty(LocationConstraintName) && String.IsNullOrEmpty(AffinityGroupName)   )
            {
                Console.WriteLine("Location or affinity-group should be specified.");
                return false;
            }
            return true;

        }

        public static bool ValidateHostedServiceName(bool displayError)
        {
            if (string.IsNullOrEmpty(HostedServiceName))
            {
                if (displayError)
                    Console.WriteLine("hosted-service is null or empty.");
                //more usage info. 
                return false;
            }
            return true;
        }

        public static string ShowDeploymentString { get; set; }
        public static bool ValidateShowDeploymentString()
        {
            if (!string.IsNullOrEmpty(ShowDeploymentString))
            {
                bool result = false;
                if (!bool.TryParse(ShowDeploymentString, out result))
                {
                    Console.WriteLine("show-deployment is not a boolean type.");
                    return false;
                }

            }

            return true;
        }

        public static string StorageServiceName { get; set; }
        public static bool ValidateStorageServiceName()
        {
            return ValidateStorageServiceName(true);
        }
        public static bool ValidateStorageServiceName(bool displayError)
        {
            if (string.IsNullOrEmpty(StorageServiceName))
            {
                if (displayError)
                    Console.WriteLine("StorageServiceName is null or empty.");
                return false;
            }
            return true;
        }

        public static string AffinityGroupName { get; set; }
        public static bool ValidateAffinityGroupName()
        {
            return ValidateAffinityGroupName(true);
        }
        public static bool ValidateAffinityGroupName(bool displayError)
        {
            if (string.IsNullOrEmpty(AffinityGroupName))
            {
                if (displayError)
                    Console.WriteLine("AffinityGroupName is null or empty.");
                return false;
            }
            return true;
        }
    }

    class ViewPropertiesCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateAffinityGroupName(false) || ValidateStorageServiceName(false) || ValidateHostedServiceName(false);
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            if (!string.IsNullOrEmpty(HostedServiceName))
            {
                if (!string.IsNullOrEmpty(ShowDeploymentString))
                {
                    bool result = false;
                    if (!bool.TryParse(ShowDeploymentString, out result))
                    {
                        Console.WriteLine("show-deployments is not a boolean type.");
                        Console.WriteLine("Getting HostedService");
                    }
                    else if (result == true)
                    {
                        Console.WriteLine("Getting detailed HostedService");
                    }

                    var service = channel.GetHostedServiceWithDetails(SubscriptionId, HostedServiceName, result);
                    Utility.LogObject(service);
                }
                else
                {
                    Console.WriteLine("Getting HostedService");
                    var service = channel.GetHostedService(SubscriptionId, HostedServiceName);
                    Utility.LogObject(service);
                }
            }
            else if(!string.IsNullOrEmpty(StorageServiceName))
            {
                Console.WriteLine("Getting StorageService");
                var service = channel.GetStorageService(SubscriptionId, StorageServiceName);
                Utility.LogObject(service);
            }
            else if (!string.IsNullOrEmpty(AffinityGroupName))
            {
                Console.WriteLine("Getting AffinityGroup");
                var service = channel.GetAffinityGroup(SubscriptionId, AffinityGroupName);
                Utility.LogObject(service);
            }
        }
    }

    class ListHostedServicesCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return true;
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing HostedServices");
            var list = channel.ListHostedServices(SubscriptionId);
            Utility.LogObject(list);
        }
    }


    class CreateHostedServiceCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateLabel(true) && ValidateLocationConstraintNameOrAffinityGroupName();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {            
            CreateHostedServiceInput input = new CreateHostedServiceInput()
            {
                ServiceName = HostedServiceName,
                Label = ServiceManagementHelper.EncodeToBase64String(CSManageCommand.Label),
                Description = CSManageCommand.Description                
            };

            if (AffinityGroupName != null)
            {
                input.AffinityGroup = AffinityGroupName;
            }
            else if (LocationConstraintName != null)
            {
                input.Location = LocationConstraintName;
            }

            Console.WriteLine("Creating hosted service {0}", HostedServiceName);
            channel.CreateHostedService(SubscriptionId, input);
        }
    }


    class UpdateHostedServiceCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName() && ValidateHostedServiceLabelAndOrDescription();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
           
            if (!String.IsNullOrEmpty(Label))
            {
                Label = ServiceManagementHelper.EncodeToBase64String(Label);
            }

            UpdateHostedServiceInput input = new UpdateHostedServiceInput()
            {
                Label = CSManageCommand.Label,
                Description = CSManageCommand.Description
            };


            Console.WriteLine("Updating hosted service {0}", HostedServiceName);
            channel.UpdateHostedService(SubscriptionId, HostedServiceName, input);
        }
    }

    class DeleteHostedServiceCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return ValidateHostedServiceName();
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Deleting hosted service {0}", HostedServiceName);
            channel.DeleteHostedService(SubscriptionId, HostedServiceName);                    
        }
    }

    class ListLocationsCommand : CSManageCommand
    {
        public override bool Validate()
        {
            return true;
        }

        protected override void PerformOperation(IServiceManagement channel)
        {
            Console.WriteLine("Listing Locations");
            LocationList list = channel.ListLocations(SubscriptionId);
            Utility.LogObject(list);
        }
    }
}
