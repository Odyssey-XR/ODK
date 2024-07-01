using UnityEngine;

namespace ODK.Extensions
{
  public static class VectorExtensions
  {
    public static Vector3 X(this Vector3 vector)
    {
      return new Vector3(vector.x, 0, 0);
    }

    public static Vector3 Y(this Vector3 vector)
    {
      return new Vector3(0, vector.y, 0);
    }

    public static Vector3 Z(this Vector3 vector)
    {
      return new Vector3(0, 0, vector.z);
    }

    public static Vector3 XY(this Vector3 vector)
    {
      return new Vector3(vector.x, vector.y, 0);
    }

    public static Vector3 XZ(this Vector3 vector)
    {
      return new Vector3(vector.x, 0, vector.z);
    }

    public static Vector3 YZ(this Vector3 vector)
    {
      return new Vector3(0, vector.y, vector.z);
    }
  }
}