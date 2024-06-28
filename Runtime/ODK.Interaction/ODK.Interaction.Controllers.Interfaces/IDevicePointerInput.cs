using UnityEngine;

namespace ODK.Interaction.Controllers.Interfaces
{
  public interface IDevicePointerInput
  {
    Vector3 PointerForward { get; }
    Vector3 PointerRight   { get; }
    Vector3 PointerUp      { get; }
  }
}