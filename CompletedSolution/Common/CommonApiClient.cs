using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Text;

 
namespace Common
{
  public class CommonApiClient
    {
     
     static public string PassValueMockApi(string json,string baseUri,string operation)
     {
        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
        var result=CommonApiClient.ValidateUsingMockApi(baseUri,stringContent,operation).Result;
        string outputString=JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result).ToString();
        return outputString;
     }

     static public async Task<HttpResponseMessage> ValidateUsingMockApi(string baseUri,HttpContent content,string operation)
        {
          HttpResponseMessage response = new HttpResponseMessage();
          using (var client = new HttpClient { BaseAddress = new Uri(baseUri)})
            {
                try
                {
                    switch(operation){
                    case "Get":
                    response = await client.GetAsync(baseUri);
                    break;
                    case "Post":
                    response = await client.PostAsync(baseUri,content);
                    break;
                    case "Put":
                    response = await client.PutAsync(baseUri,content);
                    break;
                    case "Delete":
                    response = await client.DeleteAsync(baseUri);
                    break;
                    default:
                    Console.WriteLine("Unknown operation");
                    break;
                    }
                }
                catch (System.Exception ex)
                {
                    throw new Exception("There was a problem connecting to mock server.", ex);
                }
            }
        return response;
        }
    }
}