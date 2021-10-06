using System;

namespace SimpleCampaignModule.Domain.Product
{
    public class Product
    {
        public string ProductCode {get; set;}
        public decimal Price{get; set;}
        public decimal PreviousPrice{get; set;}
        public int Stock{get; set;}

        public string ProductToString(string action)
        {
            string res;
            if(action == "create")
            {
                res = "Product created; code: " + ProductCode + " price: "+ Price.ToString("0.00")  +  " stock: " + Stock.ToString();
            }
            else
            {
                res = "Product " + ProductCode + " info: new price: " + Price.ToString("0.00") + " previous price: " + PreviousPrice.ToString("0.00") + " stock: " + Stock.ToString();   
            }
            return res;
        }
    }
}