using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings
{
  public class ReportSettingNumberRangeResponse
  {
    public int? Min { get; set; }
    public int? Max { get; set; }
  }

  public class ReportSettingDateRangeResponse
  {
    public DateTime? Min { get; set; }
    public DateTime? Max { get; set; }
  }
}