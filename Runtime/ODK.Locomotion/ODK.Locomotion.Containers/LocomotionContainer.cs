using ODK.Locomotion.Controllers;
using ODK.Locomotion.Controllers.Interfaces;
using ODK.Locomotion.Services;
using ODK.Locomotion.Services.Interfaces;
using Omni.Providers;
using UnityEngine;

namespace ODK.Locomotion.ODK.Locomotion.Containers
{
  public class LocomotionContainer : ContainerProvider
  {
    [SerializeField]
    private PointerController _hmd;
    
    private void Awake()
    {
      Bind<ILocomotionService, LinearLocomotionService>();
      Bind<IRotationService, SmoothTurnRotationService>();
      LocalBind<IPointer, PointerController>().AsSingleton(_hmd);
    }
  }
}