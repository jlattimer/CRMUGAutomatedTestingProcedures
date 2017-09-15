using FakeXrmEasy;
using FakeXrmEasy.FakeMessageExecutors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace FakeItEasyTestPlugin365.Tests
{
    [TestClass]
    public class ExamplePlugin1Tests
    {
        private static readonly Guid UserId = Guid.NewGuid();
        private readonly Entity _user = new Entity("systemuser", UserId) { ["fullname"] = "John Smith" };

        [TestMethod]
        public void FakeXrmEasy_plugin_preupdate_messagetype_execute()
        {
            //Arrange


            //Define the Target entity
            Entity target = new Entity("account", Guid.NewGuid());

            //Add to input parameters
            var inputs = new ParameterCollection
            {
                { "Target", target }
            };

            //Set up contexts
            var fakedContext = new XrmFakedContext();
            var plugCtx = fakedContext.GetDefaultPluginContext();
            plugCtx.MessageName = "create";
            plugCtx.InputParameters = inputs;
            plugCtx.UserId = UserId;
            fakedContext.CallerId = _user.ToEntityReference();

            //Set up WhoAmI
            fakedContext.AddGenericFakeMessageExecutor("WhoAmI", new WhoAmIRequestExecutor());


            //Act

            fakedContext.ExecutePluginWith<ExamplePlugin1>(plugCtx);


            //Assert

            Assert.AreEqual(target.GetAttributeValue<string>("name"), UserId.ToString());
        }

        [TestMethod]
        public void FakeXrmEasy_plugin_preupdate_messagetype_retrieve()
        {
            //Arrange

            //Define the Target entity
            Entity target = new Entity("account", Guid.NewGuid());

            //Add to input parameters
            var inputs = new ParameterCollection
            {
                { "Target", target }
            };

            //Set up contexts
            var fakedContext = new XrmFakedContext();

            var plugCtx = fakedContext.GetDefaultPluginContext();
            plugCtx.MessageName = "update";
            plugCtx.InputParameters = inputs;
            plugCtx.UserId = UserId;

            //Set up Retrieve
            fakedContext.Initialize(new List<Entity> { _user });
            fakedContext.AddExecutionMock<OrganizationRequest>(RetrieveMock);


            //Act

            fakedContext.ExecutePluginWith<ExamplePlugin1>(plugCtx);


            //Assert

            Assert.AreEqual(target.GetAttributeValue<string>("name"), "John Smith");
        }

        private OrganizationResponse RetrieveMock(OrganizationRequest req)
        {
            return new OrganizationResponse
            {
                ResponseName = "Retrieve",
                Results = new ParameterCollection { new KeyValuePair<string, object>("Entity", _user) }
            };
        }
    }
}