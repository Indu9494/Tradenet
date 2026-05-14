namespace Tradenet_ProgramManager_2.API.Core
{
    /// <summary>
    /// Interface for registering modules and their services
    /// </summary>
    public interface IModuleRegistration
    {
        /// <summary>
        /// Register module services
        /// </summary>
        void RegisterServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Get module name
        /// </summary>
        string GetModuleName();
    }
}
