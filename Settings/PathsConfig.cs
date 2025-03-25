// NSurePhysicsWebAPI/Settings/PathsConfig.cs
namespace NSurePhysicsWebAPI.Settings
{
    public class PathsConfig
    {
        public Dictionary<string, string> Paths { get; set; } = new Dictionary<string, string>
        {
            { "SiteContentImages", "sitecontent" }, // Default value
            { "Pdfs", "pdfs" } // Default value for future use
        };
    }
}