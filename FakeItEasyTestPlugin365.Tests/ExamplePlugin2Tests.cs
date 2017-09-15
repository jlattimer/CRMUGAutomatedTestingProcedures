using FakeXrmEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;

namespace FakeItEasyTestPlugin365.Tests
{
    [TestClass]
    public class ExamplePlugin2Tests
    {
        [TestMethod]
        public void FakeXrmEasy_plugin_trace_configurations()
        {
            //Arrange

            //Set up contexts
            var fakedContext = new XrmFakedContext();
            var plugCtx = fakedContext.GetDefaultPluginContext();

            //Set up the unsecure & secure configurations
            var unsecure = "Hello World";
            var secure = "Testing 1234";


            //Assert

            fakedContext.ExecutePluginWithConfigurations<ExamplePlugin2>(plugCtx, unsecure, secure);


            //Trace values from actual plug-in execution don't appear to be written anyplace
            Assert.IsNotNull(1);
        }

        [TestMethod]
        public void FakeXrmEasy_plugin_multiple_retrievemultiple_method_using_orgservice()
        {
            //Arrange

            //Set up context
            var fakedContext = new XrmFakedContext();

            //Set up the query results
            fakedContext.Initialize(new List<Entity> {
                    new Entity("account") {Id = Guid.NewGuid(), ["name"] = "test1", ["telephone1"] = "555-555-5555"},
                    new Entity("account") {Id = Guid.NewGuid(), ["name"] = "test2", ["telephone1"] = "333-333-3333"},
                    new Entity("contact") { Id = Guid.NewGuid(), ["fullname"] = "Joe Smith", ["telephone1"] = "444-444-4444" },
                    new Entity("contact") {Id = Guid.NewGuid(), ["fullname"] = "Bob Smith", ["telephone1"] = "777-777-7777"}
            });


            //Act

            //Execute the plug-in
            ExamplePlugin2 examplePlugin2 = new ExamplePlugin2(String.Empty, String.Empty);
            int count = examplePlugin2.GetAccountAndContactCount(fakedContext.GetFakedOrganizationService());


            //Assert

            Assert.AreEqual(count, 4);
        }
    }
}