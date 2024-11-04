using Syncronizer.Common;
using System;

namespace Syncronizer
{
    internal class Program
    {                
        static void Main(string[] args)
        {
            Array.Resize(ref args, args.Length + 2);
            args[0] = "C:\\temp\\Testing\\SourceFolder";            
            args[1] = "C:\\temp\\Testing\\ReplicaFolder";

            Monitor.Syncronizer syncronizer = null;
            LogInfomation logInfomation = new LogInfomation();

            var sourcePath = string.Empty;
            var replicaPath = string.Empty;

            if (string.IsNullOrEmpty(args[0]))
            {
                logInfomation.Log("Insert source path:");
                sourcePath = Console.ReadLine();
            }
            else
            {
                sourcePath = args[0];
            }

            if (string.IsNullOrEmpty(args[1]))
            {
                logInfomation.Log("Insert replica path:");
                replicaPath = Console.ReadLine();
            }
            else
            {
                replicaPath = args[0];
            }

            try
            {
                logInfomation.Log("Press any key to stop syncronization");
                syncronizer = new Monitor.Syncronizer(sourcePath, replicaPath, logInfomation);
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                logInfomation.Log($"Error message: {ex}");
            }
            finally 
            {
                syncronizer.StopWatcher();

            }



        }
    }
}
