using System.Reflection;

namespace Tradenet_ProgramManager_2.API.Core
{
    /// <summary>
    /// Service for inter-module communication
    /// </summary>
    public class InterModuleService : IInterModuleService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InterModuleService> _logger;
        private readonly Dictionary<string, Type> _registeredModules;

        public InterModuleService(IServiceProvider serviceProvider, ILogger<InterModuleService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _registeredModules = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
        }

        public void RegisterModule(string moduleName, Type moduleServiceType)
        {
            if (_registeredModules.ContainsKey(moduleName))
            {
                _logger.LogWarning("Module {ModuleName} is already registered", moduleName);
                return;
            }

            _registeredModules[moduleName] = moduleServiceType;
            _logger.LogInformation("Module {ModuleName} registered", moduleName);
        }

        public async Task<T?> GetModuleDataAsync<T>(string moduleName, string endpoint, string? parameterId = null) where T : class
        {
            if (!_registeredModules.TryGetValue(moduleName, out var moduleType))
            {
                _logger.LogWarning("Module {ModuleName} not found", moduleName);
                return null;
            }

            try
            {
                var service = _serviceProvider.GetService(moduleType);
                if (service == null)
                {
                    _logger.LogWarning("Service for module {ModuleName} not found in DI container", moduleName);
                    return null;
                }

                var method = moduleType.GetMethod(endpoint, BindingFlags.Public | BindingFlags.Instance);
                if (method == null)
                {
                    _logger.LogWarning("Endpoint {Endpoint} not found in module {ModuleName}", endpoint, moduleName);
                    return null;
                }

                object? result = null;
                if (parameterId != null && method.GetParameters().Length > 0)
                {
                    result = method.Invoke(service, new object[] { parameterId });
                }
                else
                {
                    result = method.Invoke(service, null);
                }

                if (result is Task task)
                {
                    await task.ConfigureAwait(false);
                    var resultProperty = task.GetType().GetProperty("Result");
                    return resultProperty?.GetValue(task) as T;
                }

                return result as T;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling module {ModuleName} endpoint {Endpoint}", moduleName, endpoint);
                return null;
            }
        }

        public async Task<TResponse?> SendModuleDataAsync<TRequest, TResponse>(string moduleName, string endpoint, TRequest data) 
            where TRequest : class 
            where TResponse : class
        {
            if (!_registeredModules.TryGetValue(moduleName, out var moduleType))
            {
                _logger.LogWarning("Module {ModuleName} not found", moduleName);
                return null;
            }

            try
            {
                var service = _serviceProvider.GetService(moduleType);
                if (service == null)
                {
                    _logger.LogWarning("Service for module {ModuleName} not found in DI container", moduleName);
                    return null;
                }

                var method = moduleType.GetMethod(endpoint, BindingFlags.Public | BindingFlags.Instance);
                if (method == null)
                {
                    _logger.LogWarning("Endpoint {Endpoint} not found in module {ModuleName}", endpoint, moduleName);
                    return null;
                }

                var result = method.Invoke(service, new object[] { data });

                if (result is Task task)
                {
                    await task.ConfigureAwait(false);
                    var resultProperty = task.GetType().GetProperty("Result");
                    return resultProperty?.GetValue(task) as TResponse;
                }

                return result as TResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling module {ModuleName} endpoint {Endpoint}", moduleName, endpoint);
                return null;
            }
        }

        public bool IsModuleRegistered(string moduleName)
        {
            return _registeredModules.ContainsKey(moduleName);
        }
    }
}
