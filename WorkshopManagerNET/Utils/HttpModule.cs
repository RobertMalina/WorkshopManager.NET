using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WorkshopManager.net.Utils
{
  class HttpModule
  {
    private static readonly HttpClient _client = new HttpClient();
    public async Task<T> Post<T>(string url, Dictionary<string, string> data) where T : class
    {
      try
      {
        HttpResponseMessage response = null;
        var body = new FormUrlEncodedContent(data);
        try
        {
          response = await _client.PostAsync(url, body);
        }
        catch (HttpRequestException e)
        {
          Console.WriteLine("Remote server related error occured...");
          return null;
        }
        var responseString = await response.Content.ReadAsStringAsync();
        var deserialized = JsonConvert.DeserializeObject<T>(responseString);
        return deserialized;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return null;
      }
    }

    public async Task<T> Get<T>(string url)
    {
      try
      {
        string response = null;
        try
        {
          response = await _client.GetStringAsync(url);
        }
        catch (HttpRequestException e)
        {
          Console.WriteLine("Remote server related error occured...");
          return default(T);
        }

        var deserialized = JsonConvert.DeserializeObject<T>(response);
        return deserialized;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return default(T);
      }
    }
  }
}
