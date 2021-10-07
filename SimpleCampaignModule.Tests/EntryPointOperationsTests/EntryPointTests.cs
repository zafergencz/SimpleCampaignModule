using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SimpleCampaignModule.CampaignModuleStarter.Business;
using SimpleCampaignModule.CampaignModuleStarter.Common;
using SimpleCampaignModule.CampaignModuleStarter.Controller;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Campaign;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Order;
using SimpleCampaignModule.CampaignModuleStarter.Domain.Product;
using SimpleCampaignModule.CampaignModuleStarter.EntryPointOperations;
using SimpleCampaignModule.CampaignModuleStarter.Persistence;
using SimpleCampaignModule.Tests.Persistence;

namespace SimpleCampaignModule.Tests.EntryPointOperationsTests
{
    public class EntryPointTests
    {
        EntryPoint ep;
        const string wrongNumberOfParameters = "Wrong number of parameters";
        string[] Input_WrongNumberOfParameters;
        Product sampleProduct = new Product();
        Order sampleOrder = new Order();
        Campaign sampleCampaign = new Campaign();

        public EntryPointTests()
        {
            Services.serviceProvider = new ServiceCollection()
            .AddSingleton<IBusinessActions, BusinessActions>()
            .AddSingleton<IControllers, Controllers>()
            .AddSingleton<IDatabase, MockDatabase>()
            .BuildServiceProvider();   

            ep = new EntryPoint();
            Input_WrongNumberOfParameters = new string[1];
            Input_WrongNumberOfParameters.Append("hey");

            
            sampleProduct.ProductCode = "P1";
            sampleProduct.PreviousPrice = 1;
            sampleProduct.Price = 1;
            sampleProduct.Stock = 10;
            
            sampleCampaign.ProductCode = "P1";
            sampleCampaign.TargetSalesCount = 100;
            sampleCampaign.PriceManipulationLimit = 20;
            sampleCampaign.Name = "P1C";
            sampleCampaign.Duration = 10;

            sampleOrder.ProductCode = "P1";
            sampleOrder.Quantity = 1;
        }

        [Test]
        public void ExecuteCommands_NullInput()
        {
            List<string> result = ep.ExecuteAllCommands(null);
            Assert.IsFalse(result.Any());
        }


        [Test]
        public void CreateProduct_WrongNumberOfParameters()
        {            
            string res = ep.CreateProduct(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }

        [Test]
        public void GetProductInfo_WrongNumberOfParameters()
        {
            string res = ep.GetProductInfo(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }

        [Test]
        public void CreateOrder_WrongNumberOfParameters()
        {
            string res = ep.CreateOrder(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }


        [Test]
        public void CreateCampaign_WrongNumberOfParameters()
        {
            string res = ep.CreateCampaign(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }

        [Test]
        public void GetCampaignInfo_WrongNumberOfParameters()
        {
            string res = ep.GetCampaignInfo(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }

        [Test]
        public void IncreaseTime_WrongNumberOfParameters()
        {
            string res = ep.IncreaseTime(Input_WrongNumberOfParameters);
            Assert.AreEqual(wrongNumberOfParameters, res);
        }

        [Test]
        public void CreateProduct_Success()
        {
            ClearMockDbCaches();
            string[] inputs = new string[4];
            inputs[0] = "create_product";
            inputs[1] = "P1";
            inputs[2] = "1";
            inputs[3] = "10";
            string expected = sampleProduct.ProductToString("create");

            string res = ep.CreateProduct(inputs);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void GetProductInfo_Success()
        {
            ClearMockDbCaches();
            Services.serviceProvider.GetService<IControllers>().CreateProduct(sampleProduct);

            string[] inputs = new string[2];
            inputs[0] = "get_product_info";
            inputs[1] = "P1";

            string expected = sampleProduct.ProductToString("");

            string res = ep.GetProductInfo(inputs);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void CreateOrder_Success()
        {
            ClearMockDbCaches();
            Services.serviceProvider.GetService<IControllers>().CreateProduct(sampleProduct);

            string[] inputs = new string[3];
            inputs[0] = "create_order";
            inputs[1] = sampleOrder.ProductCode;
            inputs[2] = sampleOrder.Quantity.ToString();

            string expected = sampleOrder.OrderToString();

            string res = ep.CreateOrder(inputs);
            Assert.AreEqual(expected, res);
        }


        [Test]
        public void CreateCampaign_Success()
        {
            ClearMockDbCaches();
            Services.serviceProvider.GetService<IControllers>().CreateProduct(sampleProduct);           

            string[] inputs = new string[6];
            inputs[0] = "create_campaign";
            inputs[1] = sampleCampaign.Name;
            inputs[2] = sampleCampaign.ProductCode;
            inputs[3] = sampleCampaign.Duration.ToString();
            inputs[4] = sampleCampaign.PriceManipulationLimit.ToString();
            inputs[5] = sampleCampaign.TargetSalesCount.ToString();           

            string expected = sampleCampaign.CampaignToString("create");

            string res = ep.CreateCampaign(inputs);
            Assert.AreEqual(expected, res);
        }

        [Test]
        public void GetCampaignInfo_Success()
        {
            ClearMockDbCaches();
            IControllers controller = Services.serviceProvider.GetService<IControllers>();   
            controller.CreateProduct(sampleProduct);
            controller.CreateCampaign(sampleCampaign);

            string[] inputs = new string[2];
            inputs[0] = "get_campaign_info";
            inputs[1] = sampleCampaign.Name;
            string expected = sampleCampaign.CampaignToString("");
            string res = ep.GetCampaignInfo(inputs);
            Assert.AreEqual(expected, res);           
        }

        [Test]
        public void IncreaseTime_Success()
        {
            string[] inputs = new string[2];
            inputs[0] = "increase_time";
            inputs[1] = "1";
            string expected = "Time is 01:00";
            string res = ep.IncreaseTime(inputs);
            Assert.AreEqual(expected, res);
        }        

        public void ClearMockDbCaches()
        {
            MockDatabase db = (MockDatabase)Services.serviceProvider.GetService<IDatabase>();
            db.ClearCaches();
        }
    }
}