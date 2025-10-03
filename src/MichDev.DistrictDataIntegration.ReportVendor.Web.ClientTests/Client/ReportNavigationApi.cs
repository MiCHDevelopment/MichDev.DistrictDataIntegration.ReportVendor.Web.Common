using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses;
using Newtonsoft.Json;
using Xunit;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client
{
  public class ReportNavigationApi
  {
    private Faker faker = new Faker();
    
    private HttpClient client;
    private ReportApiConfigs reportConfigs;

    public ReportNavigationApi(HttpClient client, ReportApiConfigs reportConfigs)
    {
      this.client = client;
      this.reportConfigs = reportConfigs;
    }

    private string ReportNavigationUrl => this.reportConfigs.ReportApiUrlBase.TrimEnd('/') + ReportNavigationEndpoint.Endpoint;

    public string GetReportNavigationUrl(
      IEnumerable<string>? pathToCategory,
      string? selectedNodeId)
    {

      string url = this.ReportNavigationUrl;

      if ((pathToCategory?.Any() ?? false) || !string.IsNullOrWhiteSpace(selectedNodeId))
      {
        url += "?";
      }

      if (pathToCategory?.Any() ?? false)
      {
        string pathToCategoryString = string.Join(",", pathToCategory);
        url += $"pathToCategory={pathToCategoryString}";
      }

      if ((pathToCategory?.Any() ?? false) && !string.IsNullOrWhiteSpace(selectedNodeId))
      {
        url += "&";
      }

      if (!string.IsNullOrWhiteSpace(selectedNodeId))
      {
        url += $"selectedNodeId={selectedNodeId}";
      }

      return url;
    }

    public async Task<CategoryResponse?> GetCategory(
      IEnumerable<string>? pathToCategory = null,
      string? selectedNodeId = null)
    {
      string url = this.GetReportNavigationUrl(pathToCategory, selectedNodeId);

      HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
      HttpResponseMessage response = await client.SendAsync(request, TestContext.Current.CancellationToken);

      string responseContent = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

      CategoryResponse? rootCategory = JsonConvert.DeserializeObject<CategoryResponse>(responseContent);
      return rootCategory;
    }

    public async Task<IEnumerable<string>> GetPathToRandomReport()
    {
      CategoryResponse rootCategory = (await this.GetCategory())!;
      return await GetPathToRandomReportFrom([rootCategory.InternalId], rootCategory, 10);
    }

    private async Task<IEnumerable<string>> GetPathToRandomReportFrom(
      IEnumerable<string> pathToCategoryIncludingCategory,
      CategoryResponse category,
      int depthLeft)
    {
      if (depthLeft <= 0)
      {
        throw new Exception($"Could not pick a random report because one could not be found without traversing the tree really far.");
      }

      if (category.Reports?.Any() ?? false)
      {
        ReportResponse selectedReport = this.faker.PickRandom(category.Reports);
        return pathToCategoryIncludingCategory.Append(selectedReport.InternalId);
      }
      else if (category.SubCategories?.Any() ?? false)
      {
        CategoryResponse nextCategory = this.faker.PickRandom(category.SubCategories);
        CategoryResponse nextCategoryWithChildren = (await GetCategory(pathToCategoryIncludingCategory, nextCategory.InternalId))!;

        IEnumerable<string> pathToNestedReport = await GetPathToRandomReportFrom(
          pathToCategoryIncludingCategory.Append(nextCategory.InternalId),
          nextCategoryWithChildren,
          depthLeft - 1);

        return pathToNestedReport;
      }
      else
      {
        throw new Exception($"Encountered a category without reports or sub categories at {string.Join(",", pathToCategoryIncludingCategory)}");
      }
    }
  }
}