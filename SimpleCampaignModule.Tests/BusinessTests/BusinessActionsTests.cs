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
using System.Collections.Generic;

namespace SimpleCampaignModule.Tests.BusinessTests
{
    public class BusinessActionsTests
    {
        BusinessActions business;
        Product sampleProduct;
        Order sampleOrder;
        Campaign sampleCampaign;

        public BusinessActionsTests()
        {
            Services.serviceProvider = new ServiceCollection()
            .AddSingleton<IDatabase, MockDatabase>()
            .BuildServiceProvider();            

            business = new BusinessActions();
            sampleCampaign = SampleCampaign("P1");
            sampleProduct = SampleProduct();
            sampleOrder = SampleOrder();
        }


        [Test]
        public void CreateProduct_Success()
        {
            ClearMockDbCaches();
            
            string res = business.CreateProduct(sampleProduct);
            Assert.AreEqual(sampleProduct.ProductToString("create"), res);
        }

        [Test]
        public void CreateProductTest_ProductAlreadyExists()
        {
            ClearMockDbCaches();
            
            string res; 
            string expected = "Product Already Exists";
            business.CreateProduct(sampleProduct);
            res = business.CreateProduct(sampleProduct);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void GetProductTest()
        {
            ClearMockDbCaches();
            
            business.CreateProduct(sampleProduct);
            Product newProduct = business.GetProduct(sampleProduct.ProductCode);
            Assert.AreEqual(sampleProduct.ProductCode, newProduct.ProductCode);
        }

        [Test]
        public void UpdateProductTest()
        {
            ClearMockDbCaches();

            business.CreateProduct(sampleProduct);
            business.UpdateProduct(sampleProduct.ProductCode, 23, 12, 22);
            Product newProduct = business.GetProduct(sampleProduct.ProductCode);
            bool isEqual = newProduct.ProductCode == sampleProduct.ProductCode && newProduct.Stock != sampleProduct.Stock;
            Assert.IsTrue(isEqual);
        }

        [Test]
        public void CreateAnActiveCampaignForProductTest_NoProductCase()
        {
            ClearMockDbCaches();            
            
            string expected = "There is no such product: " + sampleCampaign.ProductCode;

            string res = business.CreateAnActiveCampaignForProduct(sampleCampaign);

            Assert.AreEqual(expected, res);
            
        }

        [Test]
        public void CreateAnActiveCampaignForProductTest_CreatedCase()
        {
            ClearMockDbCaches();

            business.CreateProduct(sampleProduct);

            string expected = sampleCampaign.CampaignToString("create");
            string res = business.CreateAnActiveCampaignForProduct(sampleCampaign);

            Assert.AreEqual(expected, res);
            
        }

        [Test]
        public void AddAnActiveCampaignForProduct_ThereIsAnActiveCampaignForProduct()
        {
            ClearMockDbCaches();
            string expected = "There is already an active campaign";

            business.CreateProduct(sampleProduct);
           
            string res = GetAddAnActiveCampaignForProduct_FailTestsCommon(true);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void AddAnActiveCampaignForProduct_ThereIsNoSuchProduct()
        {
            ClearMockDbCaches();
            string expected = "There is no such product: P1";

            string res = GetAddAnActiveCampaignForProduct_FailTestsCommon(false);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void AddAnActiveCampaignForProduct_Success()
        {
            ClearMockDbCaches();

            string expected = sampleCampaign.CampaignToString("create");

            business.CreateProduct(sampleProduct);
            Dictionary<string, Campaign> allCampaignsOfProduct = new Dictionary<string, Campaign>();
            string res = business.AddAnActiveCampaignForProduct(sampleCampaign, allCampaignsOfProduct);

            Assert.AreEqual(expected, res);
        }

        [Test]
        public void CreateCampaign_Success()
        {
            ClearMockDbCaches();

            string expected = sampleCampaign.CampaignToString("create");
            business.CreateProduct(sampleProduct);
            string res = business.CreateCampaign(sampleCampaign);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void CreateCampaign_ThereIsAnotherCampaignWithSameName()
        {
            ClearMockDbCaches();
            string expected = "There is already another campaign with same name for this product P1";
            
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            sampleCampaign.IsActive = false;
            Campaign sampleCampaign2 = SampleCampaign("P1");                                          
            string res = business.CreateCampaign(sampleCampaign2);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void CreateCampaign_ActiveCampaignExistForProduct()
        {
            ClearMockDbCaches();

            string expected = "Active Campaign Already Exist For Product: "  + sampleCampaign.ProductCode;
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            string res = business.CreateCampaign(sampleCampaign);    
            Assert.AreEqual(expected, res);  
        }

        [Test]
        public void GetActiveCampaignOfProduct_Success()
        {
            ClearMockDbCaches();

            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            Campaign resCampaign = business.GetActiveCampaignOfProduct(sampleProduct.ProductCode);
            Assert.IsNotNull(resCampaign);            
        }

        [Test]
        public void GetActiveCampaignOfProduct_Fail()
        {
            ClearMockDbCaches();

            business.CreateProduct(sampleProduct);
            Campaign resCampaign = business.GetActiveCampaignOfProduct(sampleProduct.ProductCode);
            Assert.IsNull(resCampaign); 
        }

        [Test]
        public void GetCampaignByName_Success()
        {
            ClearMockDbCaches();

            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            Campaign campaign = business.GetCampaignByName(sampleCampaign.Name);
            Assert.IsNotNull(campaign);
        }

        [Test]
        public void GetCampaignByName_Fail()
        {
            ClearMockDbCaches();

            Campaign campaign = business.GetCampaignByName(sampleCampaign.Name);
            Assert.IsNull(campaign);
        }

        [Test]
        public void UpdateCampaign_Success()
        {
            ClearMockDbCaches();

            Campaign sampleCampaign2 = SampleCampaign("P1");
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);

            sampleCampaign2.Duration = 20;
            string res = business.UpdateCampaign(sampleCampaign2);

            Assert.IsNull(res);
        }


        [Test]
        public void UpdateCampaign_Fail()
        {
            ClearMockDbCaches();

            Campaign sampleCampaign2 = SampleCampaign("P1");
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);

            sampleCampaign2.ProductCode = "P2";
            string res = business.UpdateCampaign(sampleCampaign2);

            Assert.IsNotNull(res);
        }

        [Test]
        public void IncreaseTime_Success()
        {
            ClearMockDbCaches();

            string expected = "Time is 01:00";
            string res = business.IncreaseTime(1);

            Assert.AreEqual(expected, res);
        }

        [Test]
        public void IncreaseLocalTimeOfCampaigns_Success()
        {
            ClearMockDbCaches();

            Campaign sampleCampaign2 = SampleCampaign("P1");
            sampleCampaign2.Duration = 1;
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign2);
            business.IncreaseLocalTimeOfCampaigns(2);
            
            Campaign cmp = business.GetCampaignByName(sampleCampaign2.Name);
            Assert.IsFalse(cmp.IsActive);
        }

        [Test]
        public void SaveOrder_Success()
        {
            ClearMockDbCaches();
            
            string expected = sampleOrder.OrderToString();
            string res = business.SaveOrder(sampleOrder);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void ProductHasActiveCampaign_True()
        {
            bool res;
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            res = business.ProductHasActiveCampaign(sampleProduct.ProductCode);
            Assert.IsTrue(res);
        }

        [Test]
        public void OrderAction_Success()
        {
            string expected = sampleOrder.OrderToString();
            business.CreateProduct(sampleProduct);
            business.CreateCampaign(sampleCampaign);
            string res = business.OrderAction(sampleOrder);
            Assert.AreEqual(expected, res);
        }

        public void ClearMockDbCaches()
        {
            MockDatabase db = (MockDatabase)Services.serviceProvider.GetService<IDatabase>();
            db.ClearCaches();
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

        public Campaign SampleCampaign(string productCode) 
        {
            Campaign campaign = new Campaign();
            campaign.ProductCode = "P1";
            campaign.TargetSalesCount = 100;
            campaign.PriceManipulationLimit = 20;
            campaign.Name = "P1C";
            campaign.Duration = 10;

            return campaign;
        }

        public Order SampleOrder()
        {
            Order order = new Order();
            order.ProductCode = "P1";
            order.Quantity = 100;
            return order;
        }

        public string GetAddAnActiveCampaignForProduct_FailTestsCommon(bool multipleCampaigns)
        {
            business.CreateAnActiveCampaignForProduct(sampleCampaign);
            Dictionary<string, Campaign> allCampaignsOfProduct = new Dictionary<string, Campaign>();
            Campaign sampleCampaign2 = SampleCampaign("P2");

            if(multipleCampaigns)
            {
                allCampaignsOfProduct.Add(sampleCampaign.Name, sampleCampaign);
            }   

            string res = business.AddAnActiveCampaignForProduct(sampleCampaign2, allCampaignsOfProduct);
            return res;
        }
    }
}