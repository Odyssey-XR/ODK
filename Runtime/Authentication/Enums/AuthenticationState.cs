using System;

namespace ODK.Authentication.Enums
{
  [Flags]
  public enum AuthenticationState
  {
    Empty = 0,
    NotAuthenticated = 1,
    FailedAuthentication = 2,
    Authenticated = 4,
    Anonymous = 8,
    Known = 16,
  }
}