using System.Runtime.CompilerServices;

namespace HouseRentSystem.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static  IServiceCollection AddApplicationServices(this IServiceCollection service)
        {
            return service;
        }
    }
}
