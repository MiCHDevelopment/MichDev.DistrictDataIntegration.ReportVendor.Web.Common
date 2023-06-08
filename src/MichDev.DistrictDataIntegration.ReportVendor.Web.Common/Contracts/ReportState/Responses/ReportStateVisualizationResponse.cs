using System.Collections.Generic;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState
{
  /// <summary>
  /// Contains the visualization URL and thumbnail URL which are the visualization and thumbnail
  /// for a set of parameter selections for a specific report.
  /// </summary>
  public class ReportStateVisualizationResponse
  {
    public string Url { get; set; } = string.Empty;
    public string ThumbnailUrl { get; set; } = string.Empty;
  }
}