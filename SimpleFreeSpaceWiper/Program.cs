namespace SimpleFreeSpaceWiper
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var path = args[0];
            Core.FileOperations.WriteLargeFile(path);
        }
    }
}