using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;

namespace FakeItEasyTestPlugin365.Tests
{
    [TestClass]
    public class ExamplePlugin3Tests
    {
        [TestMethod]
        public void FakeXrmEasy_plugin_preimage()
        {
            //Arrange

            //Set up contexts
            var fakedContext = new XrmFakedContext();
            var plugCtx = fakedContext.GetDefaultPluginContext();

            //Define the Pre-Image
            Entity preImage = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["fullname"] = "Fred Flintstone"
            };

            plugCtx.PreEntityImages.Add("PreImage", preImage);


            //Act

            fakedContext.ExecutePluginWith<ExamplePlugin3>(plugCtx);


            //Assert

            //Review the test Output to see the 'fullname' from the PreImage
            Assert.IsNotNull(1);
        }
    }
}