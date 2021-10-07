using System;
using SimpleCampaignModule.CampaignModuleStarter.Common;
using Microsoft.Extensions.DependencyInjection;
using SimpleCampaignModule.Tests.Persistence;
using SimpleCampaignModule.CampaignModuleStarter.Persistence;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Business;
using NUnit.Framework;


namespace SimpleCampaignModule.Tests.BusinessTests
{
    public class BusinessActionsTests
    {
        BusinessActions business;
        public BusinessActionsTests()
        {
            Services.serviceProvider = new ServiceCollection()
            .AddSingleton<IDatabase, MockDatabase>()
            .BuildServiceProvider();

            business = new BusinessActions();
        }

        public Product SampleProduct()
        {
            Product product = new Product();
            product.PreviousPrice = 100;
            product.Price = 100;
            product.ProductCode = "P1";
            product.Stock = 1000;

            return product;
        }

        public Campaign SampleCampaign() 
        {
            Campaign campaign = new Campaign();
            campaign.ProductCode = "P1";
            campaign.TargetSalesCount = 100;
            campaign.PriceManipulationLimit = 20;
            campaign.Name = "P1C";
            campaign.Duration = 10;

            return campaign;
        }

        [Test]
        public void CreateProductTest()
        {
            Product p = SampleProduct();
            string res = business.CreateProduct(p);
            Assert.AreEqual(p.ProductToString("create"), res);
        }

        [Test]
        public void CreateProductTest_CreateSameProductAgain()
        {
            Product p = SampleProduct();
            string res; 
            string expected = "Product Already Exists";
            business.CreateProduct(p);
            res = business.CreateProduct(p);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void GetProductTest()
        {
            Product p = SampleProduct();
            business.CreateProduct(p);
            Product newProduct = business.GetProduct(p.ProductCode);
            Assert.AreEqual(p.ProductCode, newProduct.ProductCode);
        }

        [Test]
        public void UpdateProductTest()
        {
            Product p = SampleProduct();
            business.CreateProduct(p);
            business.UpdateProduct(p.ProductCode, 23, 12, 22);
            Product newProduct = business.GetProduct(p.ProductCode);
            bool isEqual = newProduct.ProductCode == p.ProductCode && newProduct.Stock != p.Stock;
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void CreateAnActiveCampaignForProductTest_NoProductCase()
        {
            Campaign campaign = SampleCampaign();
            string expected = "There is no such product: " + campaign.ProductCode;

            string res = business.CreateAnActiveCampaignForProduct(campaign);

            Assert.AreEqual(expected, res);
            
        }


    }
}