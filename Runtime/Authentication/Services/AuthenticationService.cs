using System;
using System.Threading.Tasks;
using ODK.Authentication.Collections;
using ODK.Authentication.Enums;
using ODK.Authentication.Services.Interfaces;
using UnityEngine;
using UnityAuthService = Unity.Services.Authentication.AuthenticationService;

namespace ODK.Authentication.Services
{
  public class AuthenticationService : IAuthenticationService
  {
    public async Task<AuthenticationModel> GetAuthenticationAsync()
    {
      if (!UnityAuthService.Instance.IsSignedIn)
        return new AuthenticationModel(string.Empty, AuthenticationState.NotAuthenticated);

      string playerId = await UnityAuthService.Instance.GetPlayerNameAsync();
      return new AuthenticationModel(playerId, AuthenticationState.Authenticated);
    }

    public async Task<AuthenticationModel> SignInAnonymouslyAsync()
    {
      UnityAuthService.Instance.ClearSessionToken();
      
      UnityAuthService.Instance.SignedIn += () => { Debug.Log("Successfully signed in"); };
      Debug.Log("Signing in anonymously");

      await UnityAuthService.Instance.SignInAnonymouslyAsync();
      string playerId = await UnityAuthService.Instance.GetPlayerNameAsync();

      return new AuthenticationModel(playerId, AuthenticationState.Authenticated | AuthenticationState.Anonymous);
    }

    public void SignOut()
    {
      UnityAuthService.Instance.SignOut();
    }
  }
}