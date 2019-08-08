using Xunit;
using System.Collections.Generic;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Common;
using PactNet;
using System;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Consumers 
{
    public class CreateSessionConsumer : IClassFixture<CreateSessionFixture>
    {
        public IPactBuilder PactBuilder { get; private set; }      
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;
        public string _path = "/Security/CreateSession";
                        
        public CreateSessionConsumer(CreateSessionFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }
    
        [Fact]
        public void CreateSession()
        {
        var _toAssert="\"SessionSummary\":";
        //Arrange
         var mockUri = _mockProviderServiceBaseUri + Common.CommonValues._createSessionPath;
         _mockProviderService.Given("New")
                        .UponReceiving("Login with eastlink user id")
                        .With(new ProviderServiceRequest 
                        {
                            Method = HttpVerb.Post,
                            Path = _path,
                            Headers = new Dictionary<string, object>
                            {
                                {"Content-Type", "application/json; charset=utf-8"}
                            },
                            Body = new
                            {
                                Login = "eastLinkUser",
                                Password =  "eastLinkUser1"

                            }
                        })
                        
                        .WillRespondWith(new ProviderServiceResponse {
                            Status = 200,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-Type", "application/json; charset=utf-8" }
                            },
                            Body = new 
                            {
                               SessionSummary = new {Active = true}
                            
                            }
                        });
	    
        //Act
        var postJson = "{\"Login\" : \"eastLinkUser\", \"Password\" :  \"eastLinkUser1\"}";
        var outputString=CommonApiClient.PassValueMockApi(postJson,mockUri,"Post");
       
        //Assert
        Assert.Contains(_toAssert,outputString.ToString());
        }
   
     [Fact]
        public void CreateSessionGet() 
        {
            var _toAssert = "";
            var mockUri=_mockProviderServiceBaseUri+Common.CommonValues._createSessionPath;
            //Arrange
            _mockProviderService.Given("New")
						.UponReceiving("A invalid get call")
						.With(new ProviderServiceRequest 
                        {
                            Method = HttpVerb.Get,
                            Path = Common.CommonValues._createSessionPath,
                         })
						.WillRespondWith(new ProviderServiceResponse {
                            Status = 200,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-Type", "application/json; charset=utf-8" }
                            },
                            Body = new 
                            {
                             Fault = new  {Code =  27}  
                            }
                        });

        //Act
        string outputString =CommonApiClient.PassValueMockApi("",mockUri,"Get");
    
        //Assert
        Assert.Contains(_toAssert,outputString);
        }
    
    }

}