using SimpleCampaignModule.Domain.Campaign;
using SimpleCampaignModule.Domain.Order;
using SimpleCampaignModule.Domain.Product;

namespace SimpleCampaignModule.Business
{
    public interface IBusinessActions
    {
        string OrderAction(Order order);
        string CreateCampaign(Campaign campaign);
        Campaign GetCampaignByName(string campaignName);
        string CreateProduct(Product product);
        Product GetProduct(string productCode);        
        string IncreaseTime(int hour);
    }
}