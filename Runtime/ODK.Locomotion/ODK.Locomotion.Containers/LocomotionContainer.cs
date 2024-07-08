using ODK.Locomotion.Controllers;
using ODK.Locomotion.Controllers.Interfaces;
using ODK.Locomotion.Services;
using ODK.Locomotion.Services.Interfaces;
using Omni.Providers;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Containers
{
  public class LocomotionContainer : NetworkContainer 
  {
    [SerializeField]
    private PointerController _hmd;
    
    protected override void OnBind(ContainerProvider container)
    {
      container.Bind<ILocomotionService, LinearLocomotionService>();
      container.Bind<IRotationService, SmoothTurnRotationService>();
    }

    protected override void OnOwnerBind(ContainerProvider container)
    {
      container.Bind<IPointer, PointerController>().AsSingleton(_hmd);
    }
  }
}