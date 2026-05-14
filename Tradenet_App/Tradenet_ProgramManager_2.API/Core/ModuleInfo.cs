namespace Tradenet_ProgramManager_2.API.Core
{
    /// <summary>
    /// Information about registered modules
    /// </summary>
    public class ModuleInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0.0";
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
