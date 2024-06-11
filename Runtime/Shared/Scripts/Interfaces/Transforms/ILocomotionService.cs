#nullable enable

namespace ODK.Shared.Transforms
{
  using UnityEngine;

  /// <summary>
  /// Contract for locomotion services
  /// </summary>
  public interface ILocomotionService
  {
    /// <summary>
    /// Calculates the delta for an object to move in given some direction, assuming that the object is on the ground
    /// and should only move along the x and z axis
    /// </summary>
    /// <param name="direction">
    /// The direction to move in
    /// </param>
    /// <param name="lookDirection">
    /// The direction source the object is looking in (i.e forward, right and up vectors)
    /// </param>
    /// <param name="speed">
    /// The speed to move at
    /// </param>
    /// <returns>
    /// A position delta to move in
    /// </returns>
    Vector3 CalculateGroundPositionDelta(Vector2 direction, Transform? lookDirection, float speed);

    /// <summary>
    /// Calculates the delta for an object to rotate in given some direction, assuming that the object is on the ground
    /// and should only rotate around the y axis
    /// </summary>
    /// <param name="direction">
    /// The direction to rotate in
    /// </param>
    /// <param name="speed">
    /// The speed to rotate at
    /// </param>
    /// <returns>
    /// A quaternion delta to rotate in
    /// </returns>
    Quaternion CalculateGroundRotationDelta(float direction, float speed);
  }
}