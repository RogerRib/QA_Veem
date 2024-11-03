using System;
using Syncronizer.Common;

namespace Syncronizer
{
    internal class Program
    {                
        static void Main(string[] args)
        {
            Console.WriteLine("Insert souce path:");
            var soucePath = string.IsNullOrEmpty(args[0]) ? Console.ReadLine(): args[0];

            Console.WriteLine("Insert replica path:");
            var replicaPath = string.IsNullOrEmpty(args[0]) ? Console.ReadLine() : args[1];

            SyncSetup setup = new SyncSetup(soucePath, replicaPath);
        }
    }
}
