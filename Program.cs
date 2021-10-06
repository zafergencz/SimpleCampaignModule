using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimpleCampaignModule.Business;
using SimpleCampaignModule.Controller;
using SimpleCampaignModule.EntryPointOperations;
using SimpleCampaignModule.Common;

namespace SimpleCampaignModule
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceConfiguration();
            
            Console.WriteLine("Campaign Module Started. It's Show Time !!! ");
            Console.WriteLine("Enter text file name: ");
            
            string filepath = Console.ReadLine();
            StartCampaignModule(filepath);
            
            Console.WriteLine("-> Program Finished !! ");
            Console.WriteLine("-> Press Any Key To Quit");
            System.Console.ReadKey();
        }

        private static void ServiceConfiguration()
        {
            Services.serviceProvider = new ServiceCollection()
            .AddSingleton<IBusinessActions, BusinessActions>()
            .AddSingleton<IControllers, Controllers>()
            .BuildServiceProvider();            
        }

        private static void StartCampaignModule(string filepath)
        {
            try
            {
                if(!String.IsNullOrEmpty(filepath))
                {
                    string[] lines = System.IO.File.ReadAllLines(@filepath);

                    EntryPoint entryPoint = new EntryPoint();
                    List<string> outputs = entryPoint.ExecuteCommands(lines);

                    foreach (string output in outputs)
                    {
                        Console.WriteLine("\t" + output);
                    }
                } 
                else
                {
                    Console.WriteLine("file path/name is empty");
                }               
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message.ToString());
            }
        }
    }
}
