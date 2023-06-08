namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings
{
  /// <summary>
  /// Contains all details about a setting, including it's selections, options,
  /// programmatic parameter id and the label to show in the UI above the setting.
  /// </summary>
  public class ReportSettingResponse : ReportSettingStateResponse
  {
    /// <summary>
    /// A programmatic ID used to identify this setting in the Report Vendor API.
    /// Consumers of the Report Vendor API will not depend on this value being in
    /// any specific format or anything. It's value is effectively arbitrary as long
    /// as it is consistent as an identifier.
    /// </summary>
    public string ParameterId { get; set; } = string.Empty;
    
    /// <summary>
    /// A human readable label to show above this setting in the UI.
    /// </summary>
    public string Label { get; set; } = string.Empty;
  }
}