using SimpleCampaignModule.Domain.Campaign;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Product;

namespace SimpleCampaignModule.Controller
{
    public interface IControllers
    {
        string CreateProduct(Product product);
        string GetProductInfo(string productCode);
        string CreateOrder(Order order);
        string CreateCampaign(Campaign campaign);
        string GetCampaignInfo(string campaignName);
        string IncreaseTime(int hour);
    }
}