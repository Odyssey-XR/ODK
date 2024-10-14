using ODK.Authentication.Services;
using ODK.Authentication.Services.Interfaces;
using ODK.Kit.Interfaces;

namespace ODK.Authentication.Interfaces.Extensions
{
  public static class IAppContextExtensions
  {
    public static IAppContext UseAuthentication(this IAppContext appContext)
    {
      appContext.Container.Bind<IAuthenticationService, AuthenticationService>();
      return appContext;
    }
  }
}