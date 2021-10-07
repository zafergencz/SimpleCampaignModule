using System;

namespace SimpleCampaignModule.CampaignModuleStarter.Domain.Order
{
    public class Order
    {
        public string ProductCode {get; set;}
        public int Quantity {get; set;}   

        public string OrderToString()
        {
            string res = "Order Created; product: " + ProductCode + " quantity: " + Quantity.ToString();            
            return res;
        }
    }
}