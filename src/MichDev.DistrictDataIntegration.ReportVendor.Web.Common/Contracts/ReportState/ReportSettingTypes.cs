using System;
using System.Collections.Generic;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.ReportState
{
  /// <summary>
  /// Static class containing the different values that can be used in the
  /// "Type" property.
  /// </summary>
  public static class ReportSettingTypes
  {
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a basic, single select field. This will
    /// be presented as just a simple drop down in the UI.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "Options",
    /// </para>
    /// </summary>
    public const string TypeSingleSelect = "single-select";
    
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a basic, select field but with the
    /// ability to select multiple values instead of just one. This will
    /// be presented as just a simple drop down in the UI and allow the user
    /// to select multiple values.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValues",
    /// "Options",
    /// </para>
    /// </summary>
    public const string TypeMultiSelect = "multi-select";
    
    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a number field that may or may
    /// not have a minimum or maximum value. Only one number can be selected for
    /// these settings.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "NumberRange" />,
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the value itself is desired to specify a range, use 2 separate settings instead; one for min and
    /// one for max. Each would specify the Range of accepted values and one would be constrained by the other
    /// (e.g. max is constrained by min).
    /// </remarks>
    public const string TypeNumber = "number";

    /// <summary>
    /// <para>
    /// Designator to denote that a setting is a date field that may or may
    /// not have a minimum or maximum value. Only one date can be selected for
    /// these settings.
    /// </para>
    /// <para>
    /// Settings of this type use the following properties:
    /// "Type",
    /// "SelectedValue",
    /// "DateRange" />,
    /// </para>
    /// </summary>
    /// <remarks>
    /// If the value itself is desired to specify a range, use 2 separate settings instead; one for min and
    /// one for max. Each would specify the Range of accepted values and one would be constrained by the other
    /// (e.g. max is constrained by min).
    /// </remarks>
    public const string TypeDate = "date";
    
    /// <summary>
    /// The same as <see cref="TypeNumber"/>. This was the original constant used for settings of the number type
    /// but the range part of the type name was found to be mis-leading. These don't represent a range of numbers,
    /// the represent a number that is constrained by a range.
    /// </summary>
    public const string TypeNumber_Deprecated = "number-range";

    /// <summary>
    /// The same as <see cref="TypeDate"/>. This was the original constant used for settings of the date type
    /// but the range part of the type name was found to be mis-leading. These don't represent a range of dates,
    /// the represent a date that is constrained by a range.
    /// </summary>
    public const string TypeDate_Deprecated = "date-range";
  }
}