using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests.Client;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportNavigation.Responses;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests.Settings;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings;
using Newtonsoft.Json;
using Xunit;
using Xunit.Sdk;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.ClientTests
{
  public class ReportStateTests
  {
    private Faker faker = new Faker();
    private ITestOutputHelper output;

    public ReportStateTests(ITestOutputHelper output)
    {
      this.output = output;
    }

    private static HttpClient client = default!;
    private static TokenResponse? token = null;
    private static IEnumerable<string> subjectReportPathToReport = null!;
    private static ReportStateRequest subjectReportInitialStateRequest = null!;
    private static ReportStateResponse? subjectReportInitialStateResponse = null!;

    private ReportApiConfigs reportConfigs = default!;
    private ReportNavigationApi reportNavigationClient = default!;
    private ReportStateApi reportStateClient = default!;

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
      this.reportStateClient = new ReportStateApi(client, this.reportConfigs);
    }

    private async Task InitializeTestSubjectReport(bool forceNewReport = false)
    {
      if (
        forceNewReport ||
        subjectReportPathToReport == null || subjectReportInitialStateResponse == null)
      {
        subjectReportPathToReport = await this.reportNavigationClient.GetPathToRandomReport();

        subjectReportInitialStateRequest = new ReportStateRequest()
        {
          PathToReport = subjectReportPathToReport,
          Parameters = new ReportStateParameterSetRequest()
        };
        
        subjectReportInitialStateResponse = await this.reportStateClient.GetState(subjectReportInitialStateRequest);
      }
    }

    private async Task InitializeWithAReportWithParameters()
    {
      int maxAttempts = 5;
      int currentAttempt = 0;

      while (currentAttempt < maxAttempts)
      {
        await InitializeTestSubjectReport(forceNewReport: true);

        if (subjectReportInitialStateResponse!.Parameters.Settings.Any())
        {
          return;
        }

        currentAttempt--;
      }

      // Max Attempts reached.
      throw new Exception($"Could not find a report with settings after {maxAttempts} attempts.");
    }

    /// <summary>
    /// Ensure that parameters make sense (has an internal id and a label).
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_TheSettingsAllHaveParameterIdsAndLabels()
    {
      await InitializeClient();
      await InitializeTestSubjectReport();

      ReportStateRequest? stateRequest = subjectReportInitialStateRequest;
      ReportStateResponse? stateResponse = subjectReportInitialStateResponse;

      AssertForRequest(
        stateRequest,
        () => Assert.NotNull(stateResponse));

      AssertForRequest(
        stateRequest,
        () => Assert.NotNull(stateResponse!.Parameters));

      AssertForRequest(
        stateRequest,
        () => Assert.NotNull(stateResponse!.Parameters.Settings));

      if (stateResponse!.Parameters.Settings.Any())
      {
        foreach (ReportSettingResponse settingResponse in stateResponse.Parameters.Settings)
        {
          AssertForSetting(
            stateRequest,
            settingResponse,
            () => Assert.NotNull(settingResponse.ParameterId));

          AssertForSetting(
            stateRequest,
            settingResponse,
            () => Assert.NotNull(settingResponse.Label));
        }
      }
    }

    /// <summary>
    /// Ensure that visualization and thumbnail url is returned in the response.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_TheVisualizationAndThumbnailUrlsAreIncluded()
    {
      await InitializeClient();
      await InitializeTestSubjectReport();

      ReportStateRequest? stateRequest = subjectReportInitialStateRequest;
      ReportStateResponse? stateResponse = subjectReportInitialStateResponse;

      Assert.NotNull(stateResponse);
      
      AssertForRequest(
        stateRequest,
        () => Assert.NotEmpty(stateResponse.Visualization?.Url ?? string.Empty));
      
      AssertForRequest(
        stateRequest,
        () => Assert.NotEmpty(stateResponse.Visualization?.ThumbnailUrl ?? string.Empty));
    }

    /// <summary>
    /// Ensure that I can select a parameter value and have it honored by the report vendor.
    /// </summary>
    /// <returns></returns>
    [Theory]
    [InlineData(ReportSettingTypes.TypeSingleSelect)]
    [InlineData(ReportSettingTypes.TypeMultiSelect)]
    [InlineData(ReportSettingTypes.TypeNumber)]
    [InlineData(ReportSettingTypes.TypeDate)]
    public async Task Test_ItHonorsMyParameter_WhenISetAParameter(string forSettingType)
    {
      await InitializeClient();
      await InitializeWithAReportWithParameters();

      ReportStateResponse? stateResponse = subjectReportInitialStateResponse;

      ReportSettingResponse? settingToSet = stateResponse!.Parameters.Settings.FirstOrDefault(s => s.Type == forSettingType);
      if (settingToSet == null)
      {
        this.output.WriteLine($"No setting of type {forSettingType} in report {JsonConvert.SerializeObject(subjectReportInitialStateRequest.PathToReport)}. Skipping test because it's not applicable for this report.");
        return;
      }

      ReportSettingStateRequest settingRequest = GetRandomRequestForSetting(settingToSet);

      ReportStateRequest stateRequest = new ReportStateRequest()
      {
        PathToReport = subjectReportPathToReport,
        Parameters = new ReportStateParameterSetRequest()
        {
          Settings = [settingRequest]
        }
      };

      ReportStateResponse? stateWithParamSet = await this.reportStateClient.GetState(stateRequest);

      AssertForRequest(
        stateRequest,
        () => Assert.NotNull(stateWithParamSet));

      foreach (ReportSettingResponse settingResponse in stateResponse.Parameters.Settings)
      {
        AssertForSetting(
          stateRequest,
          settingResponse,
          () => Assert.NotNull(settingResponse.ParameterId));

        AssertForSetting(
          stateRequest,
          settingResponse,
          () => Assert.NotNull(settingResponse.Label));
      }

      if (settingRequest.SelectedValue != null)
      {
        // Test in case of single select or other scalar value.
        ReportSettingResponse settingThatWasReset = stateWithParamSet!.Parameters.Settings.Single(param => param.ParameterId == settingToSet.ParameterId);
        AssertForSetting(
          stateRequest,
          settingThatWasReset,
          () => Assert.Equal(settingRequest.SelectedValue, settingThatWasReset.SelectedValue));
      }
      else
      {
        // Test in case of multi select
        ReportSettingResponse settingThatWasReset = stateWithParamSet!.Parameters.Settings.Single(param => param.ParameterId == settingToSet.ParameterId);
        AssertForSetting(
          stateRequest,
          settingThatWasReset,
          () =>
          {
            foreach (string expected in settingRequest.SelectedValues!)
            {
              Assert.Contains(expected, settingThatWasReset.SelectedValues ?? []);
            }
          });
      }
    }

    /// <summary>
    /// Looks through the options or possible values in the setting and picks one or more (in case of multi-select)
    /// and constructs and returns a request object for that selection.
    /// </summary>
    /// <param name="settingToSet"></param>
    /// <returns></returns>
    private ReportSettingStateRequest GetRandomRequestForSetting(ReportSettingResponse settingToSet)
    {
      ReportSettingStateRequest settingRequest = new ReportSettingStateRequest()
      {
        ParameterId = settingToSet.ParameterId,
        Type = settingToSet.Type
      };

      string forSettingType = settingToSet.Type;

      if (forSettingType == ReportSettingTypes.TypeMultiSelect)
      {
        if (!(settingToSet.Options?.Any() ?? false))
        {
          this.output.WriteLine($"No options for parameter {settingToSet.Label} in report {JsonConvert.SerializeObject(subjectReportInitialStateRequest.PathToReport)}. Did you mean for there to be options?");
        }
        bool hasMultipleOptions = settingToSet.Options?.Count() > 1;
        IEnumerable<ReportSettingOptionResponse> optionsToChoose = faker.PickRandom(
          settingToSet.Options,
          hasMultipleOptions ? 2 : 1);
        settingRequest.SelectedValues = optionsToChoose.Select(o => o.Value).ToList();
      }
      else if (forSettingType == ReportSettingTypes.TypeSingleSelect)
      {
        if (!(settingToSet.Options?.Any() ?? false))
        {
          this.output.WriteLine($"No options for parameter {settingToSet.Label} in report {JsonConvert.SerializeObject(subjectReportInitialStateRequest.PathToReport)}. Did you mean for there to be options?");
        }
        ReportSettingOptionResponse optionToChoose = faker.PickRandom(settingToSet.Options);
        settingRequest.SelectedValue = optionToChoose.Value;
      }
      else if (forSettingType == ReportSettingTypes.TypeNumber)
      {
        if (settingToSet.NumberRange == null)
        {
          settingRequest.SelectedValue = faker.Random.Int(0, 10).ToString();
        }
        else if (settingToSet.NumberRange.Min.HasValue)
        {
          if (settingToSet.NumberRange.Step.HasValue)
          {
            settingRequest.SelectedValue = (settingToSet.NumberRange.Min + settingToSet.NumberRange.Step).ToString();
          }
          else
          {
            settingRequest.SelectedValue = (settingToSet.NumberRange.Min + 1).ToString();
          }
        }
        else // if (settingToSet.NumberRange.Max.HasValue)
        {
          if (settingToSet.NumberRange.Step.HasValue)
          {
            settingRequest.SelectedValue = (settingToSet.NumberRange.Max - settingToSet.NumberRange.Step).ToString();
          }
          else
          {
            settingRequest.SelectedValue = (settingToSet.NumberRange.Max - 1).ToString();
          }
        }
      }
      else if (forSettingType == ReportSettingTypes.TypeDate)
      {
        if (settingToSet.DateRange == null)
        {
          settingRequest.SelectedValue = faker.Date.Recent(30).ToString("o");
        }
        else if (settingToSet.DateRange.Min.HasValue)
        {
          settingRequest.SelectedValue = (settingToSet.DateRange.Min.Value + TimeSpan.FromDays(1)).ToString("o");
        }
        else // if (settingToSet.DateRange.Max.HasValue)
        {
          settingRequest.SelectedValue = (settingToSet.DateRange.Max!.Value - TimeSpan.FromDays(1)).ToString("o");
        }
      }

      return settingRequest;
    }

    private void AssertForRequest(
      ReportStateRequest request,
      Action assertion)
    {
      try
      {
        assertion.Invoke();
      }
      catch (Exception e)
      {
        string requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
        throw new XunitException($"Assertion against a report state failed.\nReport state request JSON:\n{request}", e);
      }
    }

    private void AssertForSetting(
      ReportStateRequest request,
      ReportSettingResponse settingResponse,
      Action assertion)
    {
      try
      {
        assertion.Invoke();
      }
      catch (Exception e)
      {
        string requestJson = JsonConvert.SerializeObject(request, Formatting.Indented);
        string settingResponseJson = JsonConvert.SerializeObject(settingResponse, Formatting.Indented);
        throw new XunitException($"Assertion against the '{settingResponse.ParameterId}' setting failed.\nReport state request JSON:\n{requestJson}\nSetting response JSON:\n{settingResponseJson}", e);
      }
    }
  }
}