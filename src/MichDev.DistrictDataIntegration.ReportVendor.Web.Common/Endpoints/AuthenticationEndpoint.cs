namespace MichDev.DistrictDataIntegration.ReportVendor.Web.Common
{
  /// <summary>
  /// The authorization endpoints follow the OAuth2.0 specification.
  /// For more documentation around this, see https://www.oauth.com/oauth2-servers/access-tokens/client-credentials/.
  /// </summary>
  public static class AuthenticationEndpoint
  {
    /// <summary>
    /// Endpoint at which to fetch authorization tokens.
    /// </summary>
    public const string Endpoint = "/auth/token";
  }
}