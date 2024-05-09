namespace ODK.GameObjects
{
  using UnityEngine;

  public class DevelopmentKitVersioning : MonoBehaviour
  {
    public const string Version = "0.0.1";
    
  	public void Awake()
    {
      Debug.Log($"Using ODK version - {Version}");
    }	
  }
}