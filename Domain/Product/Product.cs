using System;

namespace SimpleCampaignModule.Domain.Product
{
    public class Product
    {
        public string ProductCode {get; set;}
        public decimal Price{get; set;}
        public int Stock{get; set;}

        public Product(){
            
        }
        
        public Product(string productCode, decimal price, int stock)
        {
            ProductCode = productCode;
            Price = price;
            Stock = stock;
        }
        
        public string ProductToString(string action)
        {
            string res;
            if(action == "create")
            {
                res = "Product created; code: " + ProductCode + " price: "+ Price.ToString("0.00")  +  " stock: " + Stock.ToString();
            }
            else
            {
                res = "Product " + ProductCode + " info: price: " + Price.ToString("0.00") + " stock: " + Stock.ToString();   
            }
            return res;
        }
    }
}