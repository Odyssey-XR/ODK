using System.Threading.Tasks;
using ODK.Authentication.Collections;

namespace ODK.Authentication.Services.Interfaces
{
  public interface IAuthenticationService
  {
    Task<AuthenticationModel> GetAuthenticationAsync();
    
    Task<AuthenticationModel> SignInAnonymouslyAsync();

    void SignOut();
  }
}