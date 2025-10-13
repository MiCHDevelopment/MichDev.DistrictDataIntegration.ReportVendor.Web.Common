using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState.Responses.Settings
{
  /// <summary>
  /// Represents the options that can be set for <see cref="Requests.Settings.ReportSettingStateRequest.SelectedValue"/>
  /// for a number field.
  /// </summary>
  public class ReportSettingNumberRangeResponse
  {
    /// <summary>
    /// The minimum value that can be selected in the setting.
    /// </summary>
    public double? Min { get; set; }

    /// <summary>
    /// The maximum value that can be selected in the setting.
    /// </summary>
    public double? Max { get; set; }

    /// <summary>
    /// The amount by which the selections may increase or decrease the selected value
    /// in the setting. For example, if the setting is to be an integer, set this
    /// to 1.0. If it is to be a money field, set this to 0.01 to allow stepping by cents.
    /// </summary>
    public double? Step { get; set; }
  }

  /// <summary>
  /// Represents the options that can be set for <see cref="Requests.Settings.ReportSettingStateRequest.SelectedValue"/>
  /// for a date field.
  /// </summary>
  public class ReportSettingDateRangeResponse
  {
    public DateTime? Min { get; set; }
    public DateTime? Max { get; set; }
  }
}