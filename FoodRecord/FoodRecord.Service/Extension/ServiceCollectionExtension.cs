using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FoodRecord.Service.Extension
{
    public static class ServiceCollectionExtension
    {
        public static void AddAllServiceOfInterface<T>(this IServiceCollection services, ServiceLifetime lifetime)
        {
            var subInterfaceTypes = typeof(T).Assembly.GetTypes()
                .Where(item => item.GetInterfaces().Contains(typeof(T)));

            foreach (var subInterfaceType in subInterfaceTypes)
            {
                var serviceDescriptor =
                    new ServiceDescriptor(typeof(T), subInterfaceType, lifetime);
                services.Add(serviceDescriptor);
            }
        }
    }
}
