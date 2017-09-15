using System;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using XrmMoq;
using XrmMoq.Helpers;

namespace XrmMoqTestPlugin2016.Tests
{
    [TestClass]
    public class ExamplePlugin1Tests
    {
        /// <summary>
        /// Tests a plug-in using a fake Target entity, specifying the MessageName using one of the helper methods, 
        /// using an Execute request, and validating the value from the Target entity.
        /// </summary>
        [TestMethod]
        public void Test_plugin_2016_preupdate_messagetype_execute()
        {
            //Arrange

            //Define the Target entity
            Entity target = new Entity("account", Guid.NewGuid());
            //Create the plug-in mock and set the MessageName using a helper method, CreateMessageName.GetCustom 
            //can be used to specify a message not included or for a custom action
            CrmPluginMock pluginMock = new CrmPluginMock
            {
                PluginExecutionContext = new FakePluginExecutionContext
                {
                    MessageName = XrmMoq.Helpers.CreateMessageName.GetExisting(MessageName.create)
                }
            };
            //Set the Target entity - there is an overload for EntityReference as well
            pluginMock.SetTarget(ref target);

            //Most of the specific OrganizationResponse classes do not have 'setters' on their properties so to get 
            //around this create the specific response but instead define any return values in the 'Results' collection.
            //Define the mock Execute response
            OrganizationResponse response = new WhoAmIResponse
            {
                Results = new ParameterCollection
                {
                    {"UserId", new Guid("00000000-0000-0000-0000-000000000000")}
                }
            };

            //Set the Execute response
            pluginMock.SetMockExecutes(response);


            //Act

            //Execute the plug-in
            pluginMock.Execute<ExamplePlugin1>();


            //Assert

            //Check the value updated in the Target entity matches the expected value being 
            //returned from the plug-in
            Assert.AreEqual(target.GetAttributeValue<string>("name").ToUpper(), "00000000-0000-0000-0000-000000000000");
        }

        /// <summary>
        /// Tests a plug-in using a fake Target entity, specifying the MessageName using one of the helper methods, 
        /// using a Retrieve request, and validating the value from the Target entity.
        /// </summary>
        [TestMethod]
        public void Test_plugin_2016_preupdate_messagetype_retrieve()
        {
            //Arrange

            //Define the Target entity
            Entity target = new Entity("account", Guid.NewGuid());
            //Create the plug-in mock and set the MessageName using a helper method, CreateMessageName.GetCustom 
            //can be used to specify a message not included or for a custom action
            CrmPluginMock pluginMock = new CrmPluginMock
            {
                PluginExecutionContext = new FakePluginExecutionContext
                {
                    MessageName = CreateMessageName.GetExisting(MessageName.update)
                }
            };
            //Set the Target entity
            pluginMock.SetTarget(ref target);

            //Define the entity to be returned from the Retrieve request
            Entity user = new Entity("systemuser", new Guid()) { ["fullname"] = "John Smith" };
            //Set the Retrieve response
            pluginMock.SetMockRetrieves(user);


            //Act

            //Execute the plug-in
            pluginMock.Execute<ExamplePlugin1>();


            //Assert

            //Check the value updated in the Target entity matches the expected value being 
            //returned from the plug-in
            Assert.AreEqual(target.GetAttributeValue<string>("name"), "John Smith");
        }

        [TestMethod]
        public void Test_plugin_2016_methodonly_trace_retrieve()
        {
            //Arrange

            //Create the plug-in mock - not need to create the context as we aren't using it
            CrmPluginMock pluginMock = new CrmPluginMock();

            //Define the entity to be returned from the Retrieve request
            Entity user = new Entity("systemuser", new Guid()) { ["fullname"] = "John Smith" };
            //Set the Retrieve response
            pluginMock.SetMockRetrieves(user);


            //Act

            //Execute the method (which was marked Public)
            ExamplePlugin1 examplePlugin = new ExamplePlugin1();
            string name = examplePlugin.RetrieveUserFullName(pluginMock.FakeOrganizationService, pluginMock.FakeTracingService, Guid.NewGuid());


            //Assert

            //Check the returned value matches the expected value being 
            //returned from the method in the plug-in
            Assert.AreEqual(name, "John Smith");

            //Also check this test's output and you will see that the trace message was written
        }
    }
}