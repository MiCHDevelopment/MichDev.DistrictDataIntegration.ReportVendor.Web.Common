using System.Collections.Generic;
using MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState
{
  public class ReportStateResponse
  {
    /// <summary>
    /// The parameter states and options for the report after a report state request
    /// has been processed. Any cascading effects on parameters will be reflected in
    /// these parameters.
    /// </summary>
    public ReportStateParameterSetResponse Parameters { get; set; } = new ReportStateParameterSetResponse();
    
    /// <summary>
    /// Contains the visualization URL and thumbnail URL which are the visualization and thumbnail
    /// corresponding to the parameters in this response.
    /// </summary>
    public ReportStateVisualizationResponse Visualization { get; set; } = new ReportStateVisualizationResponse();
  }
}