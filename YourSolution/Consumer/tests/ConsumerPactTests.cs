using System;
using Xunit;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Consumer;
using System.Collections.Generic;

namespace tests
{
    public class ConsumerPactTests : IClassFixture<ConsumerPactClassFixture>
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public ConsumerPactTests(ConsumerPactClassFixture fixture)
        {
            _mockProviderService = fixture.MockProviderService;
            _mockProviderService.ClearInteractions(); //NOTE: Clears any previously registered interactions before the test is run
            _mockProviderServiceBaseUri = fixture.MockProviderServiceBaseUri;
        }
    
	[Fact]
	public void ItHandlesInvalidDateParam()
	{
    // Arange
    var invalidRequestMessage = "validDateTime is not a date or time";
    _mockProviderService.Given("There is data")
                        .UponReceiving("A invalid GET request for Date Validation with invalid date parameter")
                        .With(new ProviderServiceRequest 
                        {
                            Method = HttpVerb.Post,
                            Path = "/api/provider",
                            Query = "validDateTime=lolz"
                        })
                        .WillRespondWith(new ProviderServiceResponse {
                            Status = 400,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-Type", "application/json; charset=utf-8" }
                            },
                            Body = new 
                            {
                                message = invalidRequestMessage
                            }
                        });
	// Act
    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("lolz", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
    var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

    // Assert
    Assert.Contains(invalidRequestMessage, resultBodyText);
	
	}
	
	public void ItHandlesEmptyDateParam(){
	
    var invalidRequestMessage = "validDateTime is required";
    _mockProviderService.Given("There is data")
						.UponReceiving("A invalid GET request for Date Validation with invalid date parameter")
						.With(new ProviderServiceRequest 
                        {
                            Method = HttpVerb.Get,
                            Path = "/api/provider",
                            Query = "validDateTime="
                        })
						.WillRespondWith(new ProviderServiceResponse {
                            Status = 400,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-Type", "application/json; charset=utf-8" }
                            },
                            Body = new 
                            {
                                message = invalidRequestMessage
                            }
					});
  
    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
    var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

    // Assert
    Assert.Contains(invalidRequestMessage, resultBodyText);	
	
	}
	
	public void ItParsesDateCorrectly(){
		
	var invalidRequestMessage = "\{\"test\":\"NO\",\"validDateTime\":\"01-05-2018 00:00:00\"\}";
    _mockProviderService.Given("There is data")
						.UponReceiving("A valid GET request for Date Validation with correct date parameter")
						.With(new ProviderServiceRequest 
                        {
                            Method = HttpVerb.Get,
                            Path = "/api/provider",
                            Query = "validDateTime="
                        })
						.WillRespondWith(new ProviderServiceResponse {
                            Status = 400,
                            Headers = new Dictionary<string, object>
                            {
                                { "Content-Type", "application/json; charset=utf-8" }
                            },
                            Body = new 
                            {
                                message = invalidRequestMessage
                            }
                        });
					
  
    var result = ConsumerApiClient.ValidateDateTimeUsingProviderApi("", _mockProviderServiceBaseUri).GetAwaiter().GetResult();
    var resultBodyText = result.Content.ReadAsStringAsync().GetAwaiter().GetResult();

    // Assert
    Assert.Contains(invalidRequestMessage, resultBodyText);		
		
	}
    }	
	
}