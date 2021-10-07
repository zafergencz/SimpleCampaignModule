using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SimpleCampaignModule.CampaignModuleStarter.Business;
using SimpleCampaignModule.CampaignModuleStarter.Controller;
using SimpleCampaignModule.CampaignModuleStarter.EntryPointOperations;
using SimpleCampaignModule.CampaignModuleStarter.Common;
using SimpleCampaignModule.CampaignModuleStarter.Persistence;

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
            .AddSingleton<IDatabase, Database>()
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
                    List<string> outputs = entryPoint.ExecuteAllCommands(lines);

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
