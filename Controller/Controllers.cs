using SimpleCampaignModule.Domain.Product;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Campaign;
using Microsoft.Extensions.DependencyInjection;
using SimpleCampaignModule.Business;
using SimpleCampaignModule.Common;

namespace SimpleCampaignModule.Controller
{
    public class Controllers: IControllers
    {
        
        IBusinessActions businessActions;
        
        public Controllers()
        {
            businessActions = Services.serviceProvider.GetService<IBusinessActions>();
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
            return businessActions.IncreaseTime(hour);
        }
    }
}