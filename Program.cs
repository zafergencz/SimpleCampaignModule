using System;
using System.Collections.Generic;
using SimpleCampaignModule.EntryPointOperations;

namespace SimpleCampaignModule
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Enter text file name: ");
            // string filepath = Console.ReadLine();
            string filepath = "testfiles/Scenario5.txt";

            string[] lines = System.IO.File.ReadAllLines(@filepath);

            EntryPoint entryPoint = new EntryPoint();
            List<string> outputs = entryPoint.ExecuteCommands(lines);

            foreach (string output in outputs)
            {
                Console.WriteLine("\t" + output);
            }

            System.Console.ReadKey();
        }
    }
}
