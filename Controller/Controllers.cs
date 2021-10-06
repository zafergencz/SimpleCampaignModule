using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleCampaignModule.Domain.Product;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Campaign;
using SimpleCampaignModule.Business;

namespace SimpleCampaignModule.Controller
{
    public class Controllers
    {
        
        BusinessActions businessActions;
        
        public Controllers()
        {
            businessActions = new BusinessActions();
        }        

        public string CreateProduct(Product product)
        {
            string result = businessActions.CreateProduct(product);
            return result;
        }    

        public string GetProductInfo(string productCode)
        {            
            string result;

            Product product = businessActions.GetProduct(productCode);

            if(product != null)
            {
                result = product.ProductToString("");
            }
            else
            {
                result = "Get Product Error";
            }

            return result;
        }

        public string CreateOrder(Order order)
        {
            string result = businessActions.OrderAction(order);
            return result;
        }    

        public string CreateCampaign(Campaign campaign)
        {
            string result = businessActions.CreateCampaign(campaign);
            return result;
        }    

        public string GetCampaignInfo(string campaignName)
        {
            string res;
            Campaign campaign = businessActions.GetCampaignByName(campaignName);

            if(campaign != null)
            {
                res = campaign.CampaignToString("");
            }
            else
            {
                res = "Campaign Not Found";
            }

            return res;
        }

        public string IncreaseTime(int hour)
        {
            // TODO + create EntryPoint which will read files , create objects and direct them Controller
            return businessActions.IncreaseTime(hour);
        }
    }
}