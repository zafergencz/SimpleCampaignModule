using System;

namespace SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign
{
    public class Campaign
    {
        public string Name {get; set;}
        public string ProductCode {get; set;}
        public int Duration {get; set;}
        public int PriceManipulationLimit {get; set;}
        public int TargetSalesCount {get; set;}
        public decimal OriginalPrice{get; set;}
        public int OriginalStock{get; set;}
        public decimal CurrentTurnOver{get; set;}
        public int CurrentSales{get; set;}
        public decimal CurrentAverageItemPrice{get; set;}

        public bool IsActive {get; set;}

        public int CurrentLocalHour{get; set;}

        public string CampaignToString(string action)
        {
            string res;
            if(action == "create")
            {
                res = "Campaign created; name: " + Name + " product: " + ProductCode + " duration: " + Duration.ToString() + " limit: " 
                + PriceManipulationLimit.ToString() + " target sales count: " + TargetSalesCount.ToString();
            }
            else
            {
                res = "Campaign " + Name + " info; Status " + (IsActive ? "Active":"Pasive") + ", Target Sales: " + TargetSalesCount.ToString() 
                +", Total Sales: " + CurrentSales.ToString() + " Turnover: " + CurrentTurnOver.ToString("0.00") 
                + " Average Item Price: " + CurrentAverageItemPrice.ToString("0.00");   
            }
            return res;
        }
    }
}