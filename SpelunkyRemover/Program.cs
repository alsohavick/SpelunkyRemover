using System.Diagnostics;
using System.Runtime.InteropServices;
using static System.Environment;

namespace SpelunkyRemover
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Join("Steam", "steamapps", "common");

            string root = RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 or
                Architecture.Arm64 => GetFolderPath(SpecialFolder.ProgramFilesX86),

                Architecture.X86 or
                Architecture.Arm or
                Architecture.Armv6 => GetFolderPath(SpecialFolder.ProgramFiles),

                _ => throw new PlatformNotSupportedException(),
            };

            path = Path.Combine(root, path);

            try
            {
                IEnumerable<string> directories = Directory.EnumerateDirectories(path, "Spelunky*");

                foreach (string dir in directories)
                {
                    Directory.Delete(dir, recursive: true);
                }
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.Fail("Unable to locate Spelunky.", e.Message);
                throw;
            }
            catch (UnauthorizedAccessException e)
            {
                Debug.Fail("Insufficient permissions to delete Spelunky / Spelunky 2", e.Message);
                throw;
            }
        }
    }
}