using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth;
using Newtonsoft.Json;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client
{
  public class AuthConfigs
  {
    public AuthConfigs()
    {
      TokenUrl = string.Empty;
      ClientId = string.Empty;
      ClientSecret = string.Empty;
      Scopes = [];
    }

    public AuthConfigs(string tokenUrl, string clientId, string clientSecret, IEnumerable<string> scopes)
    {
      TokenUrl = tokenUrl;
      ClientId = clientId;
      ClientSecret = clientSecret;
      Scopes = scopes;
    }

    public string TokenUrl { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public IEnumerable<string> Scopes { get; set; }

    public async Task<TokenResponse?> GetTokenResponse(HttpClient client)
    {
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.TokenUrl)
      {
        Content = new FormUrlEncodedContent(new Dictionary<string, string>()
        {
          { "client_id", this.ClientId },
          { "client_secret", this.ClientSecret },
          { "grant_type", "client_credentials" },
          { "scope", string.Join(" ", this.Scopes) }
        })
      };

      HttpResponseMessage response = await client.SendAsync(request);
      string responseContent = await response.Content.ReadAsStringAsync();
      
      try
      {
        TokenResponse? tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
        return tokenResponse;
      }
      catch (Exception e)
      {
        throw new Exception($"Got exception when parsing auth token response:\n{responseContent}", e);
      }
    }
  }
}