using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Tederean.FastIOW.Internal
{

  public class IowkitLibraryHandler
  {

    public static void ExtractNativeLibrary()
    {
      var availableLibraries = new List<PlatformLibrary>()
      {
        new PlatformLibrary(OSPlatform.Windows, Architecture.X86, "iowkit_windows_x86.dll"),
        new PlatformLibrary(OSPlatform.Windows, Architecture.X64, "iowkit_windows_x64.dll"),

        new PlatformLibrary(OSPlatform.Linux, Architecture.X64, "libiowkit_linux_amd64.so"),
        new PlatformLibrary(OSPlatform.Linux, Architecture.Arm, "libiowkit_linux_arm32.so"),
        new PlatformLibrary(OSPlatform.Linux, Architecture.Arm64, "libiowkit_linux_aarch64.so"),
      };

      var platformLibraryDictionary = new Dictionary<OSPlatform, string>()
      {
        { OSPlatform.Windows, $"{IowkitLibrary.NativeLibraryName}.dll" },
        { OSPlatform.Linux, $"lib{IowkitLibrary.NativeLibraryName}.so" },
        { OSPlatform.OSX, $"{IowkitLibrary.NativeLibraryName}.dylib" },
      };

      var assemblyDirectory = AssemblyDirectory();
      var operatingSystem = GetOperatingSystem();
      var processArchitecture = RuntimeInformation.ProcessArchitecture;
      var platformLibraryName = platformLibraryDictionary[operatingSystem];
      var platformLibraryPath = Path.Combine(assemblyDirectory, platformLibraryName);

      var platformLibrary = availableLibraries.FirstOrDefault(availableLibrary => availableLibrary.Architecture == processArchitecture && availableLibrary.OperatingSystem == operatingSystem) ?? throw new NotSupportedException($"Unsupported environment: {operatingSystem} [{processArchitecture}]");

      File.Delete(platformLibraryPath);

      WriteResourceToFile(platformLibrary.Path, platformLibraryPath);
    }


    private static void WriteResourceToFile(string resourceName, string fileName)
    {
      using (var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
      {
        using (var file = new FileStream(fileName, FileMode.Create, FileAccess.Write))
        {
          resource.CopyTo(file);
        }
      }
    }

    private static string AssemblyDirectory()
    {
      var codeBase = Assembly.GetExecutingAssembly().CodeBase;
      var uri = new UriBuilder(codeBase);
      var path = Uri.UnescapeDataString(uri.Path);

      return Path.GetDirectoryName(path);
    }

    private static OSPlatform GetOperatingSystem()
    {
      var platforms = new[] { OSPlatform.Windows, OSPlatform.Linux, OSPlatform.OSX };

      return platforms.First(platform => RuntimeInformation.IsOSPlatform(platform));
    }
  }


  internal class PlatformLibrary
  {

    public PlatformLibrary(OSPlatform operatingSystem, Architecture architecture, string filename)
    {
      OperatingSystem = operatingSystem;
      Architecture = architecture;
      Path = $"FastIOW.Resources.{filename}";
    }


    public OSPlatform OperatingSystem { get; }

    public Architecture Architecture { get; }

    public string Path { get; }
  }
}
