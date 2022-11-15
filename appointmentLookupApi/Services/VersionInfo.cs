using System.Globalization;
using System.Reflection;

namespace appointmentLookupApi;
    public class VersionInfo : IVersionInfo
    {
        public string Version { get; set; }
        public string VersionShort { get; set; }

        public VersionInfo()
        {
            var assemblyVersion = Assembly.GetEntryAssembly()?.GetName().Version;
            Version = assemblyVersion!.ToString();
            VersionShort = assemblyVersion.Major.ToString(CultureInfo.InvariantCulture);
            VersionShort += "." + assemblyVersion.Minor.ToString(CultureInfo.InvariantCulture);
        }
    }