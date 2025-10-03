using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth;
using Xunit;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests
{
  public class AuthTests
  {
    private static TokenResponse? tokenResponse = null;

    private ReportApiConfigs reportConfigs = default!;

    private async Task<TokenResponse?> GetOrLoadTokenResponse()
    {
      this.reportConfigs = await ReportApiConfigs.Load();
      
      if (tokenResponse == null)
      {
        using HttpClient client = new HttpClient();
        tokenResponse = await this.reportConfigs.Auth.GetTokenResponse(client);
      }

      return tokenResponse;
    }
    
    /// <summary>
    /// Ensure that the token request succeeds and it is accessible in the right JSON property.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_ICanGetAnAccessToken()
    {
      TokenResponse? tokenResponse = await GetOrLoadTokenResponse();
      Assert.NotNull(tokenResponse);

      Assert.NotNull(tokenResponse.AccessToken);
      Assert.NotEmpty(tokenResponse.AccessToken);
    }

    /// <summary>
    /// Ensure that the token gives us enough time to work with the API.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_ItExpiresIn30MinutesOrMore()
    {
      TokenResponse? tokenResponse = await GetOrLoadTokenResponse();
      Assert.NotNull(tokenResponse);

      TimeSpan expiresIn = TimeSpan.FromSeconds(tokenResponse.ExpiresIn);
      Assert.True(expiresIn > TimeSpan.FromMinutes(30), $"Expected token to expire 30 minutes or later. It expires in {expiresIn}");
    }

    /// <summary>
    /// Ensure that the scopes persist.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_ItContainsTheScopesWeRequested()
    {
      TokenResponse? tokenResponse = await GetOrLoadTokenResponse();
      Assert.NotNull(tokenResponse);

      Assert.Equal(this.reportConfigs.Auth.Scopes, tokenResponse.Scope.Split(" "));
    }

    /// <summary>
    /// Ensure that the scopes persist.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_ItContainsTheTokenTypeThatWeRequested()
    {
      TokenResponse? tokenResponse = await GetOrLoadTokenResponse();
      Assert.NotNull(tokenResponse);

      Assert.Equal("client_credentials", tokenResponse.GrantType);
    }
  }
}