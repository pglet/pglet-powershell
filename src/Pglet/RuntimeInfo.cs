using System.Runtime.InteropServices;

namespace Pglet
{
    public static class RuntimeInfo
    {
        public static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        public static bool IsLinux
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            }
        }

        public static bool IsMac
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            }
        }

        public static bool IsDotNetCore
        {
            get
            {
                return RuntimeInformation.FrameworkDescription.Contains(".NET Core");
            }
        }
    }
}