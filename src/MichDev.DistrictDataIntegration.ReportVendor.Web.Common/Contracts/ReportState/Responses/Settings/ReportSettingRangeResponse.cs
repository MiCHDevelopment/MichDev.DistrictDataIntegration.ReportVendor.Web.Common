using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings
{
  public class ReportSettingNumberRangeResponse
  {
    public double? Min { get; set; }
    public double? Max { get; set; }
  }

  public class ReportSettingDateRangeResponse
  {
    public DateTime? Min { get; set; }
    public DateTime? Max { get; set; }
  }
}