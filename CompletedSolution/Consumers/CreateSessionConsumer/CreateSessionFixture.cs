using System;
using PactNet;
using PactNet.Mocks.MockHttpService;
using Xunit;


namespace Common
{
    // This class is responsible for setting up a shared
    // mock server for Pact used by all the tests.

    public class CreateSessionFixture : IDisposable
    {

        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 9222; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }
        
        public string _consumerName;

        public CreateSessionFixture()
        {
            Common.CommonValues._consumerName = "CreateSessionConsumer";
            Common.CommonValues._providerName = "CreateSessionAPI";
            
            var pactConfig = new PactConfig
            {
                SpecificationVersion = "2.4.6",

                PactDir = @"..\..\..\pacts",
                LogDir = @".\pact_logs"
            };

            PactBuilder = new PactBuilder(pactConfig);

            PactBuilder.ServiceConsumer(Common.CommonValues._consumerName)
                    .HasPactWith(Common.CommonValues._providerName);

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        
        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // This will save the pact file once finished.
                    PactBuilder.Build();
                }

                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            PactBuilder.Build();
            //Dispose(true);
        }
        #endregion
    }

}