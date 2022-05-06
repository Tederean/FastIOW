using System;
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
      if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        throw new NotImplementedException("Sorry, currently only windows is supported.");
      }


      var assemblyDirectory = AssemblyDirectory();
      var platformIowkitName = $"{IowkitLibrary.NativeLibraryName}.dll";

      var conflicatingFiles = Directory.EnumerateFiles(assemblyDirectory).Where(filePath => platformIowkitName.Equals(Path.GetFileName(filePath), StringComparison.OrdinalIgnoreCase)).ToArray();

      foreach (var conflicatingFile in conflicatingFiles)
      {
        File.Delete(conflicatingFile);
      }

      var iowkitPath = Path.Combine(assemblyDirectory, platformIowkitName);

      if (Environment.Is64BitProcess)
      {
        WriteResourceToFile("FastIOW.Resources.iowkit_64.dll", iowkitPath);
      }
      else
      {
        WriteResourceToFile("FastIOW.Resources.iowkit_x86.dll", iowkitPath);
      }
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
  }
}
