using LoyaltyOne.Data;
using LoyaltyOne.Services;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace LoyaltyOne.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            container.RegisterType<IApiService, ApiService>();
            container.RegisterType<ITextService, TextService>();
            container.RegisterType<ILocationService, LocationService>();

            container.RegisterType<ICityRepository, CityRepository>();
            container.RegisterType<ITextRepository, TextRepository>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}