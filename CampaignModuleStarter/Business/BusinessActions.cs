using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Persistence;
using SimpleCampaignModule.CampaignModuleStarter.Common;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleCampaignModule.CampaignModuleStarter.Business
{
    public class BusinessActions: IBusinessActions
    {
        private IDatabase database;
        private int globalHour = 0;

        public BusinessActions()
        {
            database = Services.serviceProvider.GetService<IDatabase>();
        }

        private decimal PriceManipulation(string productCode, int newstock, int orderQuantity, decimal currentPrice)
        {
            Campaign campaign = GetActiveCampaignOfProduct(productCode);
            decimal newPrice = currentPrice;

            if(campaign != null)
            {
                decimal percentage = Decimal.Divide((campaign.OriginalStock - newstock), campaign.OriginalStock);
                newPrice = Decimal.Subtract(campaign.OriginalPrice, campaign.PriceManipulationLimit) + (campaign.PriceManipulationLimit * percentage);
                campaign.CurrentTurnOver += Decimal.Multiply(orderQuantity, currentPrice);
                campaign.CurrentSales += orderQuantity;
                campaign.CurrentAverageItemPrice = Decimal.Divide(campaign.CurrentTurnOver, campaign.CurrentSales);
                string res = UpdateCampaign(campaign);
                if(res != null)
                {
                    newPrice = currentPrice;
                }
            }

            return newPrice;
        }

        public string OrderAction(Order order)
        {
            string res;

            Product product = GetProduct(order.ProductCode);

            if(product != null)
            {
                if(order.Quantity <= product.Stock)
                {
                    int newstock = product.Stock - order.Quantity;
                    decimal newPrice = product.Price;  
                    decimal previousPrice = product.Price;               

                    if(ProductHasActiveCampaign(order.ProductCode))
                    {
                        newPrice = PriceManipulation(order.ProductCode, newstock, order.Quantity, product.Price);
                    }                

                    string result = UpdateProduct(order.ProductCode, newPrice, previousPrice, newstock);    

                    if(result == null)
                    {
                        res = SaveOrder(order);
                    }
                    else
                    {
                        res = "Update product error";
                    }
                }
                else
                {
                    res = "Order quantity(" + order.Quantity.ToString() + ") bigger than product stock (" + product.Stock.ToString() +")";
                }                
            }
            else
            {
                res = order.ProductCode + ": There is no such product";
            }   

            return res;
        }

        private bool ProductHasActiveCampaign(string productCode)
        {
            bool res = false;
            Dictionary<string, Campaign> productCampaigns;
            if(database.CampaignCache.TryGetValue(productCode, out productCampaigns))
            {
                Campaign activeCampaign = productCampaigns.Values.SingleOrDefault(x => x.IsActive);

                if(activeCampaign != null)
                {
                    res = true;
                }
            }

            return res;
        }

        private string SaveOrder(Order order)
        {
            if(database.OrderCache.ContainsKey(order.ProductCode))
            {
                database.OrderCache[order.ProductCode].Add(order);
            }
            else
            {
                List<Order> newOrderList = new List<Order>();
                newOrderList.Add(order);
                database.OrderCache.Add(order.ProductCode, newOrderList);
            }
            
            return order.OrderToString();
        }

        public string CreateCampaign(Campaign campaign)
        {
            string res;

            if(ProductHasActiveCampaign(campaign.ProductCode))
            {
                res = "Active Campaign Already Exist For Product: " + campaign.ProductCode;
            }
            else
            { 
                Dictionary<string, Campaign> allCampaignsOfProduct;
                // bu product koda ait kampanyalar var mı (bir ürünün pasif kampanyaları da olabilir)
                if(database.CampaignCache.TryGetValue(campaign.ProductCode, out allCampaignsOfProduct) && allCampaignsOfProduct != null)
                {
                    // aynı isimde başka bir kampanya var mı(aktif ya da pasif)                        
                    if( allCampaignsOfProduct.ContainsKey(campaign.Name))
                    {
                        res = "There is already another campaign with same name for this product " + campaign.ProductCode;                            
                    }
                    else
                    {
                        res = AddAnActiveCampaignForProduct(campaign, allCampaignsOfProduct);                  
                    }
                }
                else
                {
                    // bu product kayıtlı kampnya yoksa sıfırdan yarat
                    res = CreateAnActiveCampaignForProduct(campaign);
                }             
            }

            return res;
        }

        public string CreateAnActiveCampaignForProduct(Campaign campaign)
        {
            string res;
            Product product = GetProduct(campaign.ProductCode);
            if(product == null)
            {
                res = "There is no such product: " + campaign.ProductCode;
            }
            else
            {
                campaign.OriginalPrice = product.Price;
                campaign.OriginalStock = product.Stock;

                campaign.IsActive = true;
                Dictionary<string, Campaign> campaignDict = new Dictionary<string, Campaign>();
                campaignDict.Add(campaign.Name, campaign);
                database.CampaignCache.Add(product.ProductCode, campaignDict);     
                res = campaign.CampaignToString("create");     
            }     

            return res;
        }

        private string AddAnActiveCampaignForProduct(Campaign campaign, Dictionary<string, Campaign> allCampaignsOfProduct)
        {
            string res;
            var activeCampaign = allCampaignsOfProduct.Values.SingleOrDefault(x => x.IsActive);

            if(activeCampaign != null)
            {
                res = "There is already an active campaign";
            }
            else
            {
                Product product = GetProduct(campaign.ProductCode);

                if(product == null)
                {
                    res = "There is no such product: " + campaign.ProductCode;
                }
                else
                {
                    campaign.OriginalPrice = product.Price;
                    campaign.OriginalStock = product.Stock;
                    campaign.IsActive = true;

                    allCampaignsOfProduct.Add(campaign.Name, campaign); 
                    database.CampaignCache[product.ProductCode] = allCampaignsOfProduct;
                    res = campaign.CampaignToString("create");                    
                }    
            }     

            return res;    
        }

        private Campaign GetActiveCampaignOfProduct(string productCode)
        {
            Campaign campaign = null;
            Dictionary<string, Campaign> allCampaignsOfProduct;
            if(database.CampaignCache.TryGetValue(productCode, out allCampaignsOfProduct))
            {
                campaign = allCampaignsOfProduct.Values.SingleOrDefault(x => x.IsActive);    
            }

            return campaign;
        }

        public Campaign GetCampaignByName(string campaignName)
        {
            Campaign campaign = null;
            List<Dictionary<string, Campaign>> allCampaignsOfAllProducts = database.CampaignCache.Values.ToList();

            var allCampaignsOfProduct = allCampaignsOfAllProducts.SingleOrDefault(x => x.ContainsKey(campaignName));

            if(allCampaignsOfProduct != null)
            {
                allCampaignsOfProduct.TryGetValue(campaignName, out campaign);
            }
            
            return campaign;
            
        }
        private string UpdateCampaign(Campaign campaign)
        {
            string result = "error";
            if(database.CampaignCache.ContainsKey(campaign.ProductCode))
            {
                if(database.CampaignCache[campaign.ProductCode].ContainsKey(campaign.Name))
                {
                    database.CampaignCache[campaign.ProductCode][campaign.Name] = campaign;    
                    result = null;
                }                
            }
            return result;

        }

        public string CreateProduct(Product product)
        {
            string res;

            if(database.ProductCache.ContainsKey(product.ProductCode))
            {
                res = "Product Already Exists";
            }
            else 
            {                
                database.ProductCache.Add(product.ProductCode, product);
                res = product.ProductToString("create");
            }

            return res;          
        }

        public Product GetProduct(string productCode)
        {
            Product product = null;
            if(database.ProductCache.ContainsKey(productCode))
            {
                product = database.ProductCache[productCode];
            }

            return product;
        }

        public string UpdateProduct(string productCode, decimal price, decimal previousPrice, int stock)
        {
            string result = "error";
            if(database.ProductCache.ContainsKey(productCode))
            {
                Product product = new Product();
                product.ProductCode = productCode;
                product.Price = price;
                product.PreviousPrice = previousPrice;
                product.Stock = stock;
                database.ProductCache[productCode] = product;
                result = null;
            }
            return result;
        }
        
        public string IncreaseTime(int hour)
        {
            string res;
            
            IncreaseLocalTimeOfCampaigns(hour);

            globalHour += hour;

            if(globalHour > 24)
            {
                globalHour = 0;
            }

            res = globalHour.ToString();
            return "Time is " + (res.Length == 2 ? res : "0" + res ) + ":00";
        }

        private void IncreaseLocalTimeOfCampaigns(int hour)
        {
            foreach (var product in database.CampaignCache.Keys.ToList())
            {
                foreach(var campaignName in database.CampaignCache[product].Keys.ToList())
                {
                    Campaign campaign = database.CampaignCache[product][campaignName];
                    if(campaign.IsActive)
                    {
                        int duration = campaign.Duration;
                        int campaignLocalHour = campaign.CurrentLocalHour;

                        campaignLocalHour += hour;
                        
                        if(campaignLocalHour > duration)
                        {
                            campaign.IsActive = false;
                        }
                        else
                        {
                            campaign.CurrentLocalHour = campaignLocalHour;
                        }                        
                    }
                }                                
            }
        }
    }
}