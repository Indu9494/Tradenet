namespace Tradenet_ProgramManager_2.API.Core
{
    /// <summary>
    /// Base interface for inter-module communication
    /// Allows modules to communicate with each other
    /// </summary>
    public interface IInterModuleService
    {
        /// <summary>
        /// Get data from another module
        /// </summary>
        Task<T?> GetModuleDataAsync<T>(string moduleName, string endpoint, string? parameterId = null) where T : class;

        /// <summary>
        /// Send data to another module
        /// </summary>
        Task<TResponse?> SendModuleDataAsync<TRequest, TResponse>(string moduleName, string endpoint, TRequest data) where TRequest : class where TResponse : class;

        /// <summary>
        /// Check if module exists
        /// </summary>
        bool IsModuleRegistered(string moduleName);
    }
}
