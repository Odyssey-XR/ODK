namespace ODK.Shared.Core
{
  using UnityEngine;

  /// <summary>
  /// DevelopmentKitVersioning is a MonoBehaviour that logs the version of the ODK to the console.
  /// </summary>
  public class DevelopmentKitVersioning : MonoBehaviour
  {
    /// <summary>
    /// The version of the ODK package.
    /// </summary>
    public const string Version = "0.0.1";
    
    /// <inheritdoc cref="MonoBehaviour"/>
  	public void Awake()
    {
      Debug.Log($"Using ODK version - {Version}");
    }	
  }
}