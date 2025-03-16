﻿using Amazon.Runtime.Internal.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
////////factory patterns //////
namespace IdentityManager.Services.ControllerService.IControllerService
{
    public class ServiceFactory
    {
        private  readonly Dictionary<string, Func<IService>> _serviceRegistry;

        public ServiceFactory()
        {
            _serviceRegistry = new Dictionary<string, Func<IService>>(StringComparer.OrdinalIgnoreCase)
        {
            { "plumber", () => new PlumberService() },
            { "electrician", () => new ElectricianService() },
            { "Tiler", () => new TileranService() },
            { "Painter", () => new PainteranService() },
            { "Carpenteran", () => new CarpenteranService() },
            { "Blacksmith", () => new BlacksmithanService() },
        };
        }

        public  IService CreateService(string serviceType)
        {
            if (_serviceRegistry.TryGetValue(serviceType, out var serviceCreator))
            {
                return serviceCreator();
            }
            throw new ArgumentException($"Unsupported service: {serviceType}");
        }

    }
}
