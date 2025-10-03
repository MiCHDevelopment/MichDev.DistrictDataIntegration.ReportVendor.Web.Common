using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Bogus;
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
        this.reportNavigationClient = new ReportNavigationApi(client, this.reportConfigs);
        this.reportStateClient = new ReportStateApi(client, this.reportConfigs);
      }
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
    /// Ensure that root category makes sense (has an internal id and some sub categories).
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
          AssertForRequest(
            stateRequest,
            () => Assert.NotNull(settingResponse.ParameterId));

          AssertForRequest(
            stateRequest,
            () => Assert.NotNull(settingResponse.Label));
        }
      }
    }

    /// <summary>
    /// Ensure that root category makes sense (has an internal id and some sub categories).
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Test_ItHonorsMyParameter_WhenISetAParameter()
    {
      await InitializeClient();
      await InitializeWithAReportWithParameters();

      ReportStateResponse? stateResponse = subjectReportInitialStateResponse;

      ReportSettingResponse settingToSet = stateResponse!.Parameters.Settings.First();
      ReportSettingOptionResponse optionToChoose = faker.PickRandom(settingToSet.Options!.Skip(1));

      ReportStateRequest stateRequest = new ReportStateRequest()
      {
        PathToReport = subjectReportPathToReport,
        Parameters = new ReportStateParameterSetRequest()
        {
          Settings = [
            new ReportSettingStateRequest()
            {
              ParameterId = settingToSet.ParameterId,
              SelectedValue = optionToChoose.Value,
              Type = settingToSet.Type
            }
          ]
        }
      };
      
      ReportStateResponse? stateWithParamSet = await this.reportStateClient.GetState(stateRequest);

      AssertForRequest(
        stateRequest,
        () => Assert.NotNull(stateWithParamSet));

      foreach (ReportSettingResponse settingResponse in stateResponse.Parameters.Settings)
      {
        AssertForRequest(
          stateRequest,
          () => Assert.NotNull(settingResponse.ParameterId));
          
        AssertForRequest(
          stateRequest,
          () => Assert.NotNull(settingResponse.Label));
      }

      ReportSettingResponse settingThatWasReset = stateWithParamSet!.Parameters.Settings.Single(param => param.ParameterId == settingToSet.ParameterId);
      AssertForRequest(
        stateRequest,
        () => Assert.Equal(optionToChoose.Value, settingThatWasReset.SelectedValue));
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
        throw new XunitException($"Assertion agains a report state failed.\nReport state request JSON:\n{request}", e);
      }
    }
  }
}