using UnityEngine;

namespace ODK.Locomotion.Controllers.Interfaces
{
  public interface IPointer
  {
    Vector3 Forward { get; }
    Vector3 Right   { get; }
    Vector3 Up      { get; }
  }
}