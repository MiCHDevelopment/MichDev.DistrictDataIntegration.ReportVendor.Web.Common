using System.Collections.Generic;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests.Settings;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Requests
{
  /// <summary>
  /// <para>
  /// Represents user selection and the available options for all parameters in a given report.
  /// </para>
  /// <para>
  /// The district code for these requests will be included in the authentication mechanism.
  /// Each request to the Report State endpoint must only be authenticated to a single district.
  /// That district will be the selected district for the report.
  /// </para>
  /// </summary>
  public class ReportStateParameterSetRequest
  {
    /// <summary>
    /// <para>
    /// A list of setting selections for this report state request.
    /// </para>
    /// <para>
    /// Settings in this list are effectively arbitrary to consuming APIs. They are dynamic and used to
    /// build out the report builder UI but the consumer is not concerned about what, if any, settings
    /// are contained in this list from a technical standpoint.
    /// </para>
    /// </summary>
    public IEnumerable<ReportSettingStateRequest> Settings { get; set; } = new List<ReportSettingStateRequest>();
    
    /// <summary>
    /// Whether or not the data in the visualization request has suppressed PII (true if so, false otherwise).
    /// </summary>
    public bool IsDataSuppressed { get; set; }

    /// <summary>
    /// Represents users selection for the display type. The display type represents whether
    /// the user wants the report as, say for example, a bar chart,
    /// line graph, etc.
    /// </summary>
    public string DisplayType { get; set; } = string.Empty;
    
    /// <summary>
    /// Represents users selection for the school year.
    /// </summary>
    public string? SchoolYear { get; set; } = null;
  }
}