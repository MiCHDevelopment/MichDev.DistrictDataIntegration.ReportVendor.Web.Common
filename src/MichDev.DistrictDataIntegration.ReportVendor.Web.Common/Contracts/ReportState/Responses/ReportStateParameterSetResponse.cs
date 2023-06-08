using System.Collections.Generic;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState
{
  /// <summary>
  /// Represents user selection and the available options for all parameters in a given report.
  /// </summary>
  public class ReportStateParameterSetResponse
  {
    /// <summary>
    /// <para>
    /// A list of settings available for this report, along with their state.
    /// </para>
    /// <para>
    /// Settings in this list are effectively arbitrary to consuming APIs. They are dynamic and used to
    /// build out the report builder UI but the consumer is not concerned about what, if any, settings
    /// are contained in this list from a technical standpoint.
    /// </para>
    /// </summary>
    public IEnumerable<ReportSettingResponse> Settings { get; set; } = new List<ReportSettingResponse>();
    
    /// <summary>
    /// Whether or not the data in the visualization response has suppressed PII (true if so, false otherwise).
    /// </summary>
    public bool IsDataSuppressed { get; set; }

    /// <summary>
    /// <para>
    /// Represents users selection and the available options for the display type. The display
    /// type represents whether the user wants the report as, say for example, a bar chart,
    /// line graph, etc.
    /// </para>
    /// <para>
    /// This setting must always be of type <see cref="ReportSettingTypes.TypeSingleSelect" />.
    /// </para>
    /// </summary>
    public ReportSettingStateResponse DisplayType { get; set; } = new ReportSettingStateResponse();
    
    /// <summary>
    /// <para>
    /// Represents users selection and the available options for the school year.
    /// </para>
    /// <para>
    /// This setting must always be of type <see cref="ReportSettingTypes.TypeSingleSelect" />.
    /// </para>
    /// </summary>
    public ReportSettingStateResponse? SchoolYear { get; set; } = null;
  }
}