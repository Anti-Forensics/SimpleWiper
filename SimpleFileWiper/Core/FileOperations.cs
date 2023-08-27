using System;


namespace SimpleWiper.Core
{
    public class FileOperations
    {
        public static bool OverwriteFileBlockSize4096(string path)
        {
            long fileLength = 0;

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

            var bytes = new Byte[4096];
            new Random().NextBytes(bytes);

            for (int i = 0; i <= Math.Ceiling((decimal)fileLength / bytes.Length); i++)
            {
                streamWriter.BaseStream.Write(bytes, 0, bytes.Length);

                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"Progress: {i:n0} blocks (4096 bytes) of " +
                    $"{fileLength / bytes.Length:n0} blocks.");
            }

            streamWriter.Close();

            return true;
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
