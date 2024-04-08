using CrossProcssReaderWriterLock;
using WaffleGenerator;

namespace ReaderWriterOperationProcess
{
    internal class Program
    {
        private static readonly CrossProcessWriteLock WriteLock = new CrossProcessWriteLock("writeLock");

        private static readonly string FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFile.txt");

        private static void Main(string[] args)
        {
            string processIndex = args[0];
            bool locked = bool.Parse(args[1]);
            string text = WaffleEngine.Text(20000, true);
            if (locked)
            {
                WriteOperation(processIndex, text);
            }
            else
            {
                WriteOperationLockFree(processIndex, text);
            }
        }

        private static void WriteOperation(string uid, string text)
        {
            if (WriteLock.TryAcquire(10000))
            {
                WriteLineToConsole(uid, "Start write operation");

                try
                {
                    File.WriteAllText(FilePath, text);
                }
                catch (Exception ex)
                {
                    WriteLineToConsole(uid, ex.Message);
                }
                finally
                {
                    WriteLock.Release();
                }

                WriteLineToConsole(uid, "The write operation is finished");
            }
            else
            {
                WriteLineToConsole(uid, "Failed to acquire the write lock");
            }
        }

        private static void WriteOperationLockFree(string uid, string text)
        {
            WriteLineToConsole(uid, "Start lock free write operation");

            try
            {
                File.WriteAllText(FilePath, text);
            }
            catch (Exception ex)
            {
                WriteLineToConsole(uid, ex.Message);
            }

            WriteLineToConsole(uid, "The lock free write operation is finished");
        }

        private static void WriteLineToConsole(string uid, string text)
        {
            Console.WriteLine($"{ToLongDateTimeString(DateTime.Now)}: {uid} - {text}!");
        }

        private static string ToLongDateTimeString(DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss.fff");
        }
    }
}