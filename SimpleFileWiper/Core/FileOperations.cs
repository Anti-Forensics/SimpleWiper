using System.Runtime.InteropServices;
using System.Text;

namespace SimpleWiper.Core
{
    public class FileOperations
    {
        public static bool OverwriteFileBlockSize4096(string path)
        {
            decimal fileLength = 0;

            try
            {
                fileLength = new System.IO.FileInfo(path).Length;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine($"[!] Could not find file: {path}");
                ExitApplicationWithError();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ExitApplicationWithError();
            }

            FileStream fileStream = new FileStream(path, FileMode.Open);
            StreamWriter streamWriter = new StreamWriter(fileStream);

            var bytesBuffer = new Byte[4096];
            new Random().NextBytes(bytesBuffer);

            var totalBlocks = Math.Floor(fileLength / bytesBuffer.Length);
            decimal totalBlocksWritten = 0;

            for (int i = 0; i <= totalBlocks; i++)
            {
                if (i == totalBlocks)
                {
                    var totalFileLength = fileLength - (4096 * totalBlocksWritten);
                    bytesBuffer = new byte[((int)totalFileLength)];
                    new Random().NextBytes(bytesBuffer);

                    streamWriter.BaseStream.Write(bytesBuffer, 0, bytesBuffer.Length);

                    Console.WriteLine($"[+] The end of/or entire file has " +
                        $"been overwritten: ({bytesBuffer.Length} bytes of data)");
                }
                else
                {
                    streamWriter.BaseStream.Write(bytesBuffer, 0, bytesBuffer.Length);

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine($"[+] Progress: {i:n0} blocks (4096 bytes) of " +
                        $"{totalBlocks:n0} blocks.");

                    totalBlocksWritten += 1;
                }
            }
            streamWriter.Close();
            return true;
        }

        public static string ChangeFilename(string path)
        {
            var rand = new Random();
            var directoryPath = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            
            const string fileNameChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var randomFileNameArray =  new string(Enumerable.Repeat(fileNameChars, fileName.Length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());

            if (isLinux)
            {

                if (directoryPath == "")
                {
                    directoryPath = ".";
                }

                File.Move(Path.Combine(directoryPath, fileName),
                    Path.Combine(directoryPath, randomFileNameArray));

                return Path.Combine(directoryPath, randomFileNameArray);
            }
            else
            {
                File.Move(Path.Combine(directoryPath, fileName), 
                    Path.Combine(directoryPath, randomFileNameArray));

                return Path.Combine(directoryPath, randomFileNameArray);
            }
        }

        public static bool DeleteFileAfterWipe(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ExitApplicationWithError();
            }

            return true;
        }

        public static void ExitApplicationWithError()
        {
            Console.WriteLine("[!] Exited with error.");
            System.Environment.Exit(1);
        }
    }
}
