using System;
using System.IO;

namespace SimpleFreeSpaceWiper.Core
{
    internal class FileOperations
    {
        public static void WriteLargeFile(string path)
        {
            FileStream fileStream;
            StreamWriter? streamWriter = null;

            try
            {
                fileStream = new FileStream(path, FileMode.Create);
                streamWriter = new StreamWriter(fileStream);
            }
            catch (UnauthorizedAccessException)
            {

                Console.WriteLine($"[!] You do not have Write permission in the current path: {path}");
                System.Environment.Exit(1);
            }


            var bytesBuffer = new Byte[4096];
            new Random().NextBytes(bytesBuffer);

            var driveInfo = new DriveInfo(path);
            decimal freeSpace = driveInfo.TotalFreeSpace;
            decimal totalWiped = 0;
            
            Console.Clear();

            while (true)
            {
                try
                {
                    Console.SetCursorPosition(0, 0);
                    streamWriter.BaseStream.Write(bytesBuffer, 0, bytesBuffer.Length);
                    totalWiped += 4096;
                    Console.WriteLine($"{totalWiped:n0} of {freeSpace:n0} bytes left to overwrite.");
                    if (totalWiped == freeSpace)
                    {
                        Console.SetCursorPosition(0, 1);
                        Console.WriteLine("[+] Operation Completed.");
                        break;
                    }
     
                }
                catch (UnauthorizedAccessException)
                {
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine($"[!] You do not have Write permission to the current path: {path}");
                    break;
                }
                catch (System.IO.IOException e)
                {
                    Console.SetCursorPosition(0, 1);
                    Console.WriteLine($"[!] {e}");
                    break;
                }
            }
            streamWriter.Close();
        }
    }
}
