using ODK.Locomotion.Services;
using ODK.Locomotion.Services.Interfaces;
using Omni.Providers;

namespace ODK.Locomotion.ODK.Locomotion.Containers
{
  public class LocomotionContainer : ContainerProvider
  {
    private void Awake()
    {
      Bind<ILocomotionService, XRLinearLocomotionService>();
      Bind<IRotationService, XRSmoothTurnRotationService>();
    }
  }
}