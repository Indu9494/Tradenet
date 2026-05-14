using System.Reflection;

namespace Tradenet_ProgramManager_2.API.Core
{
    /// <summary>
    /// Extension methods for module registration
    /// </summary>
    public static class ModuleExtensions
    {
        /// <summary>
        /// Register all modules that implement IModuleRegistration
        /// </summary>
        public static IServiceCollection RegisterModules(this IServiceCollection services, IConfiguration configuration)
        {
            // Get all types that implement IModuleRegistration
            var moduleType = typeof(IModuleRegistration);
            var modules = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => moduleType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            var interModuleService = new InterModuleService(null!, null!);

            foreach (var module in modules)
            {
                var instance = Activator.CreateInstance(module) as IModuleRegistration;
                if (instance != null)
                {
                    instance.RegisterServices(services, configuration);
                }
            }

            // Register the inter-module service for module discovery
            services.AddSingleton<IInterModuleService>(sp => new InterModuleService(sp, sp.GetRequiredService<ILogger<InterModuleService>>()));

            return services;
        }

        /// <summary>
        /// Register a specific module
        /// </summary>
        public static IServiceCollection RegisterModule<TModule>(this IServiceCollection services, IConfiguration configuration)
            where TModule : IModuleRegistration, new()
        {
            var module = new TModule();
            module.RegisterServices(services, configuration);

            return services;
        }
    }
}
