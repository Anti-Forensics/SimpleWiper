using SimpleWiper.Core;

namespace SimpleWiper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = "";

            try
            {
                path = args[0];
            }
            catch (System.IndexOutOfRangeException)
            {
                Console.WriteLine("[!] The file path is empty: SimpleFileWiper.exe <path\to\file>");
                FileOperations.ExitApplicationWithError();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                FileOperations.ExitApplicationWithError();
            }

            Console.Clear();

            if (FileOperations.OverwriteFileBlockSize4096(path))
            {
                Console.WriteLine("[+] File overwritten successfully.");
            }

            if (FileOperations.DeleteFileAfterWipe(FileOperations.ChangeFilename(path)))
            {
                Console.WriteLine("[+] File deleted after being overwritten.");
            }
        }
    }
}