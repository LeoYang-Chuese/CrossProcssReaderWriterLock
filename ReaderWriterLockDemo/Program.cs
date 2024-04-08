using System.Diagnostics;

namespace ReaderWriterLockDemo
{
    internal class Program
    {
        private static readonly string OperationProcessPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ReaderWriterOperationProcess.exe");

        private static void Main(string[] args)
        {
            var locked = false;
            for (var i = 1; i <= 100; i++)
            {
                Process.Start(OperationProcessPath, new[] {i.ToString(), locked.ToString()});
            }
        }
    }
}