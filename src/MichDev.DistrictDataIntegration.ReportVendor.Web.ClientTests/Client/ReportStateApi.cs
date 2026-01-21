using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests;
using Newtonsoft.Json;
using Xunit;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client
{
  public class ReportStateApi
  {
    private Faker faker = new Faker();

    private HttpClient client;
    private ReportApiConfigs reportConfigs;

    public ReportStateApi(HttpClient client, ReportApiConfigs reportConfigs)
    {
      this.client = client;
      this.reportConfigs = reportConfigs;
    }

    private string ReportStateUrl => this.reportConfigs.ReportApiUrlBase.TrimEnd('/') + ReportStateEndpoint.Endpoint;

    public async Task<ReportStateResponse?> GetState(ReportStateRequest requestContent)
    {
      string contentBody = JsonConvert.SerializeObject(requestContent);
      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, this.ReportStateUrl)
      {
        Content = new StringContent(
          contentBody,
          Encoding.UTF8,
          "application/json")
      };

      HttpResponseMessage response = await client.SendAsync(request, TestContext.Current.CancellationToken);

      string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

      try
      {
        ReportStateResponse? stateResponse = JsonConvert.DeserializeObject<ReportStateResponse>(responseContent);
        return stateResponse;
      }
      catch (Exception e)
      {
        throw new Exception($"Got exception when parsing report state response for report: {contentBody}:\n{responseContent}", e);
      }
    }
  }
}