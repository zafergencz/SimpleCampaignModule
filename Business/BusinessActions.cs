using System;
using System.Collections.Generic;
using System.Linq;
using SimpleCampaignModule.Domain.Product;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Campaign;
using SimpleCampaignModule.Persistence;

namespace SimpleCampaignModule.Business
{
    public class BusinessActions
    {
        private int globalHour = 0;
        public decimal PriceManipulation(string productCode, int newstock, int orderQuantity, decimal currentPrice)
        {
            Campaign campaign = GetCampaign(productCode);
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

                    if(ProductHasCampaign(order.ProductCode))
                    {
                        newPrice = PriceManipulation(order.ProductCode, newstock, order.Quantity, product.Price);
                    }                

                    string result = UpdateProduct(order.ProductCode, newPrice, newstock);    

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

        public bool ProductHasCampaign(string productCode)
        {
            return Database.ActiveCampaignCache.ContainsKey(productCode);            
        }

        public string SaveOrder(Order order)
        {
            if(Database.OrderCache.ContainsKey(order.ProductCode))
            {
                Database.OrderCache[order.ProductCode].Add(order);
            }
            else
            {
                List<Order> newOrderList = new List<Order>();
                newOrderList.Add(order);
                Database.OrderCache.Add(order.ProductCode, newOrderList);
            }
            
            return order.OrderToString();
        }

        public string CreateCampaign(Campaign campaign)
        {
            string res;

            if(Database.ActiveCampaignCache.ContainsKey(campaign.ProductCode))
            {
                res = "Active Campaign Already Exist For Product: " + campaign.ProductCode;
            }
            else
            {
                Product product = GetProduct(campaign.ProductCode);

                if(product != null)
                {
                    campaign.OriginalPrice = product.Price;
                    campaign.OriginalStock = product.Stock;
                    Database.ActiveCampaignCache.Add(campaign.ProductCode, campaign);                
                    res = campaign.CampaignToString("create");
                }
                else
                {
                    res = "There is no such product: " + campaign.ProductCode;
                }                
            }

            return res;
        }

        public Campaign GetCampaign(string productCode)
        {
            Campaign campaign = null;
            if(Database.ActiveCampaignCache.ContainsKey(productCode))
            {
                campaign = Database.ActiveCampaignCache[productCode];
            }

            return campaign;
        }

        public Campaign GetCampaignByName(string campaignName)
        {
            Campaign campaign;
            try
            {
                campaign = Database.ActiveCampaignCache.Values.Single(x => x.Name == campaignName);
            }
            catch(Exception ex) when (ex != null)
            {
                campaign = null;
            }

            return campaign;
            
        }
        public string UpdateCampaign(Campaign campaign)
        {
            string result = "error";
            if(Database.ActiveCampaignCache.ContainsKey(campaign.ProductCode))
            {
                Database.ActiveCampaignCache[campaign.ProductCode] = campaign;
                result = null;
            }
            return result;

        }

        public string CreateProduct(Product product)
        {
            string res;

            if(Database.ProductCache.ContainsKey(product.ProductCode))
            {
                res = "Product Already Exists";
            }
            else 
            {                
                Database.ProductCache.Add(product.ProductCode, product);
                res = product.ProductToString("create");
            }

            return res;          
        }

        public Product GetProduct(string productCode)
        {
            Product product = null;
            if(Database.ProductCache.ContainsKey(productCode))
            {
                product = Database.ProductCache[productCode];
            }

            return product;
        }

        public string UpdateProduct(string productCode, decimal price, int stock)
        {
            string result = "error";
            if(Database.ProductCache.ContainsKey(productCode))
            {
                Database.ProductCache[productCode] = new Product(productCode, price, stock);
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
            foreach (var key in Database.ActiveCampaignCache.Keys.ToList())
            {
                int duration = Database.ActiveCampaignCache[key].Duration;
                int campaignLocalHour = Database.ActiveCampaignCache[key].CurrentLocalHour;
                
                if(campaignLocalHour > duration)
                {
                    Database.ActiveCampaignCache.Remove(key);
                }
                else
                {
                    Database.ActiveCampaignCache[key].CurrentLocalHour = campaignLocalHour;
                }

                campaignLocalHour += hour;                
            }
        }
    }
}