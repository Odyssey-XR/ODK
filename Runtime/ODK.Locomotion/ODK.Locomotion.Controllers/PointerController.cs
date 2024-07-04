using ODK.Locomotion.Controllers.Interfaces;
using UnityEngine;

namespace ODK.Locomotion.Controllers
{
  public class PointerController : MonoBehaviour, IPointer 
  {
    public Vector3 Forward => transform.forward;
    public Vector3 Right   => transform.right;
    public Vector3 Up      => transform.up;
  }
}