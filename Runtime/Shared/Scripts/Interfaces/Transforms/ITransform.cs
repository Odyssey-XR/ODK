namespace ODK.Shared.Transforms
{
  using UnityEngine;

  /// <summary>
  /// Interface for all types that implement some position and rotation values
  /// </summary>
  public interface ITransform
  {
    /// <summary>
    /// The position of the object
    /// </summary>
    Vector3 Position { get; }

    /// <summary>
    /// The rotation of the object
    /// </summary>
    Quaternion Rotation { get; }
  }
}