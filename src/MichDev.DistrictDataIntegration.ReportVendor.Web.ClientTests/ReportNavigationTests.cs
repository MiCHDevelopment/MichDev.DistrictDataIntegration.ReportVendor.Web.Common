using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests
{
  public class ReportNavigationTests
  {
    private static HttpClient client = default!;
    private static TokenResponse? token = null;

    private ReportApiConfigs reportConfigs = default!;
    private ReportNavigationApi reportNavigationClient = default!;

    private async Task InitializeClient()
    {
      this.reportConfigs = await ReportApiConfigs.Load();

      if (token == null)
      {
        client = new HttpClient();
        token = await this.reportConfigs.Auth.GetTokenResponse(client);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token!.AccessToken);
      }

      this.reportNavigationClient = new ReportNavigationApi(client, this.reportConfigs);
    }

    /// <summary>
    /// Ensure that root category makes sense (has an internal id and some sub categories).
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_TheRootCategoryHasAnInternalIdAndSomethingForTheUserToSelect()
    {
      await InitializeClient();
      CategoryResponse? rootCategory = await this.reportNavigationClient.GetCategory();

      Assert.NotNull(rootCategory);
      Assert.NotNull(rootCategory.InternalId);
      Assert.NotEqual(string.Empty, rootCategory.InternalId.Trim());

      if (!rootCategory.SubCategories!.Any())
      {
        // If sub categories are empty, there should be reports.
        // Otherwise, there's no content accessible. No categories,
        // no reports = nothing to select.
        Assert.NotNull(rootCategory.Reports);
        Assert.NotEmpty(rootCategory.Reports);
      }
    }

    int maxDepth = 2;

    /// <summary>
    /// Ensure that the token request succeeds and it is accessible in the right JSON property.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_TheNavigationTreeNodesAreValid_WhenIWalkTheTree()
    {
      await InitializeClient();

      CategoryResponse? category = await this.reportNavigationClient.GetCategory();

      IEnumerable<ExpectationFailure> failures = [];

      Assert.NotNull(category);

      failures = failures.Concat(await AssertCategoryIsValid([], category.InternalId, category, 0));

      if (failures.Any())
      {
        throw new AggregateException(failures.Select(failure =>
          new XunitException(failure.Message)));
      }
    }

    private async Task<IEnumerable<ExpectationFailure>> AssertCategoryIsValid(
      IEnumerable<string> pathToCategory,
      string selectedNodeId,
      CategoryResponse category,
      int depth)
    {
      IEnumerable<ExpectationFailure> failures = [];

      failures = failures.Concat(
        AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotNull(category.InternalId),
          $"internalId must not be null"));

      failures = failures.Concat(
        AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotEqual(string.Empty, category.InternalId.Trim()),
          $"internalId must not be empty"));

      failures = failures.Concat(
        AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotNull(category.DisplayName),
          $"dispalyName must not be null"));
          
      failures = failures.Concat(
        AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotEqual(string.Empty, category.DisplayName.Trim()),
          $"dispalyName must not be empty"));

      if (category.SubCategories?.Any() ?? false)
      {
        failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.Distinct(category.SubCategories.Select(c => c.InternalId).ToList()),
          "Sub Categories within the same parent Category may not have the same internalId"));

        if (depth < maxDepth)
        {
          foreach (CategoryResponse subCategoryOption in category.SubCategories)
          {
            IEnumerable<string> pathToSubCategory = pathToCategory.Append(selectedNodeId);
            string selectedNodeIdForSubCategory = subCategoryOption.InternalId;

            CategoryResponse? subCategory = await this.reportNavigationClient.GetCategory(pathToSubCategory, selectedNodeIdForSubCategory);

            failures = failures.Concat(
              AssertExpectation(
                pathToSubCategory,
                selectedNodeIdForSubCategory,
                () => Assert.NotNull(subCategory),
                $"Fetch for category failed"));

            if (subCategory != null)
            {
              failures = failures.Concat(
                await AssertCategoryIsValid(pathToSubCategory, selectedNodeIdForSubCategory, subCategory, depth + 1));
            }
          }
        }
      }
        else
        {
          failures = failures.Concat(AssertExpectation(
            pathToCategory,
            selectedNodeId,
            () => Assert.NotNull(category.Reports),
            "reports cannot be null if there aren't any categories"));

          failures = failures.Concat(AssertExpectation(
            pathToCategory,
            selectedNodeId,
            () => Assert.NotEmpty(category.Reports!),
            "reports cannot be an empty array if there aren't any categories"));
        }

      if (category.Reports?.Any() ?? false)
      {
        // If sub categories are empty, there should be reports.
        // Otherwise, there's no content accessible. No categories,
        // no reports = nothing to select.
        failures = failures.Concat(AssertReportsValid(pathToCategory, selectedNodeId, category.Reports));
      }

      return failures;
    }

    private IEnumerable<ExpectationFailure> AssertReportsValid(
      IEnumerable<string> pathToCategory,
      string selectedNodeId,
      IEnumerable<ReportResponse> categoryReports)
    {
      IEnumerable<ExpectationFailure> failures = [];

      failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.Distinct(categoryReports!.Select(c => c.InternalId).ToList()),
          "Sub Categories within the same parent Category may not have the same internalId"));

      foreach (ReportResponse report in categoryReports!)
      {
        failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotNull(report.InternalId),
          $"internalId for report cannot be null (encountered in report {report.InternalId} named {report.DisplayName})"));

        failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotEqual(string.Empty, report.InternalId.Trim()),
          $"internalId for report cannot be an empty string (encountered in report {report.InternalId} named {report.DisplayName})"));

        failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotNull(report.DisplayName),
          $"displayName for report cannot be null (encountered in report {report.InternalId} named {report.DisplayName})"));

        failures = failures.Concat(AssertExpectation(
          pathToCategory,
          selectedNodeId,
          () => Assert.NotEqual(string.Empty, report.DisplayName.Trim()),
          $"displayName for report cannot be an empty string (encountered in report {report.InternalId} named {report.DisplayName})"));
      }

      return failures;
    }

    private IEnumerable<ExpectationFailure> AssertExpectation(
      IEnumerable<string>? pathToCategory,
      string? selectedNodeId,
      Action assertion,
      string failureMessage)
    {
      try
      {
        assertion.Invoke();
        return [];
      }
      catch (Exception e)
      {
        ExpectationFailure failure = new ExpectationFailure(
          pathToCategory,
          selectedNodeId,
          failureMessage,
          e,
          this.reportNavigationClient.GetReportNavigationUrl);
        return [failure];
      }
    }

    public class ExpectationFailure
    {
      private IEnumerable<string>? pathToCategory;
      private string? selectedNodeId;
      private string issue;
      private Exception failureException;
      private Func<IEnumerable<string>?, string?, string> getReportNavigationUrl;

      public ExpectationFailure(
        IEnumerable<string>? pathToCategory,
        string? selectedNodeId,
        string issue,
        Exception failureException,
        Func<IEnumerable<string>?, string?, string> getReportNavigationUrl)
      {
        this.pathToCategory = pathToCategory;
        this.selectedNodeId = selectedNodeId;
        this.issue = issue;
        this.failureException = failureException;
        this.getReportNavigationUrl = getReportNavigationUrl;
      }

      public string Message
      {
        get
        {
          string message = "Report Navigation Expectation Failure:\n";

          string url = this.getReportNavigationUrl.Invoke(this.pathToCategory, this.selectedNodeId);

          message += $"\tURL: {url}\n";

          message += $"\tIssue: {this.issue}\n";
          message += $"\tException:\n{this.failureException}";
          return message;
        }
      }
    }
  }
}