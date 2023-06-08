using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common.Contracts.Auth
{
  /// <summary>
  /// A request contract used to request a JWT token which will grant authorization for
  /// subsequent requests to the External Report Vendor API.
  /// </summary>
  public class TokenResponse
  {
    /// <summary>
    /// <para>
    /// The token that will be used as a JWT token to get access to subsequen requests.
    /// These tokens should be stateless in nature. If someone has a token, they should
    /// be able to use that token regardless of any previous requests with that token
    /// (for example, 3 machines with different IP addresses should be able to make requests
    /// at the same time with the same token).
    /// </para>
    /// <para>
    /// Of course, the only exception to this is the expiration which will invalidate the token
    /// after a certain amount of time.
    /// </para>
    /// </summary>
    [JsonPropertyName("access_token")]
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// <para>
    /// The amount of time (in seconds) until this token expires from the time it was retrieved.
    /// </para>
    /// <para>
    /// This should be the length of life of the token, not the date and time that it expires.
    /// This is to reduce complexity of managing timezones and such.
    /// </para>
    /// </summary>
    [JsonPropertyName("expires_in")]
    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    /// <summary>
    /// The interface and flow which will be used to retrieve tokens. This should
    /// always be "client_credentials".
    /// </summary>
    [JsonPropertyName("grant_type")]
    [JsonProperty("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    /// <summary>
    /// The list of scopes requested in the token. Scopes are essentially policy
    /// identifiers that grants access to a certain set of endpoints or features.
    /// For this API, the scope should be the 5 digit district code of the district
    /// to which the request is authenticating (remember, authorization tokens
    /// should only grant access to a single district). There may be other scopes
    /// but we will need to know what those are.
    /// 
    /// This field is a space separated string of scopes like "scope1 scope2 scope2".
    /// </summary>
    [JsonPropertyName("scope")]
    [JsonProperty("scope")]
    public string Scope { get; set; } = string.Empty;
  }
}