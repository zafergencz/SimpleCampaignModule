using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Common;
using Microsoft.Extensions.DependencyInjection;
using SimpleCampaignModule.CampaignModuleStarter.Controller;

namespace SimpleCampaignModule.CampaignModuleStarter.EntryPointOperations
{
    public class EntryPoint
    {
        IControllers controller;
        private const string wrongNumberOfParameters = "Wrong number of parameters";

        public EntryPoint()
        {
            controller = Services.serviceProvider.GetService<IControllers>();
        }

        public List<string> ExecuteCommands(string[] lines)
        {
            List<string> outputs = new List<string>();            

            foreach(string str in lines)
            {
                string[] commandAndInputs = str.Split(' ');                

                if(commandAndInputs != null && commandAndInputs.Any())
                {
                    string res;
                    
                    switch(commandAndInputs[0])
                    {
                        case "create_product":
                            res = CreateProduct(commandAndInputs);
                            break;
                        case "get_product_info":
                            res = GetProductInfo(commandAndInputs);
                            break;
                        case "create_order":
                            res = CreateOrder(commandAndInputs);
                            break;
                        case "create_campaign":
                            res = CreateCampaign(commandAndInputs);
                            break;
                        case "get_campaign_info":
                            res = GetCampaignInfo(commandAndInputs);
                            break;
                        case "increase_time":
                            res = IncreaseTime(commandAndInputs);
                            break;
                        default:
                            res = null;
                            break;
                    }

                    if(!String.IsNullOrEmpty(res))
                    {
                        outputs.Add(res);
                    }
                }
            }

            return outputs;
        }

        public string  CreateProduct(string[] inputs)
        {
            string res;
            if(inputs.Length == 4)
            {
                Product product = new Product();
                int stock = 0;
                decimal price = 0;

                product.ProductCode = inputs[1];    

                decimal.TryParse(inputs[2], out price);
                product.Price = price;            
                
                Int32.TryParse(inputs[3], out stock);
                product.Stock = stock;         

                product.PreviousPrice = product.Price;      

                res = controller.CreateProduct(product);
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }  

        public string GetProductInfo(string[] inputs)
        {
            string res;
            if(inputs.Length == 2)
            {
                res = controller.GetProductInfo(inputs[1]);
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }

        public string CreateOrder(string[] inputs)
        {
            string res;

            if(inputs.Length == 3)
            {
                Order order = new Order();
                order.ProductCode = inputs[1];
                
                int quantity = 0;
                Int32.TryParse(inputs[2], out quantity);
                order.Quantity = quantity;

                res = controller.CreateOrder(order);
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }

        public string CreateCampaign(string[] inputs)
        {
            string res;
            if(inputs.Length == 6)
            {
                Campaign campaign = new Campaign();
                campaign.Name = inputs[1];
                campaign.ProductCode = inputs[2];

                int duration = 0;
                Int32.TryParse(inputs[3], out duration);
                campaign.Duration = duration;

                int priceManipulationLimit = 0;
                Int32.TryParse(inputs[4], out priceManipulationLimit);
                campaign.PriceManipulationLimit = priceManipulationLimit;

                int targetSalesCount = 0;
                Int32.TryParse(inputs[5], out targetSalesCount);
                campaign.TargetSalesCount = targetSalesCount;

                res = controller.CreateCampaign(campaign);
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }

        public string GetCampaignInfo(string[] inputs)
        {
            string res;
            if(inputs.Length == 2)
            {
                res = controller.GetCampaignInfo(inputs[1]);
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }

        public string IncreaseTime(string[] inputs)
        {
            string res = "";
            if(inputs.Length == 2)
            {
                int hour;

                if(Int32.TryParse(inputs[1], out hour))
                {
                    res = controller.IncreaseTime(hour);
                }
            }
            else
            {
                res = wrongNumberOfParameters;
            }

            return res;
        }
    }
}