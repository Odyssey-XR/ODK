#nullable enable

namespace OdysseyXR.ODK.Services.Logging
{
  using System.Text;
  using UnityEngine;

  /// <summary>
  /// Custom logger implementation which changes
  /// behaviour based on build profile (editor, production etc.)
  /// </summary>
  public static class Logger
  {
    public static void Log(params object[] messages)
    {
      #if UNITY_EDITOR
      Debug.Log(GetMessage(messages));
      #endif
    }

     public static void Warn(params object[] messages)
     {
       #if UNITY_EDITOR
       Debug.LogWarning(GetMessage(messages));
       #endif
     }   
    
    public static void Error(params object[] messages)
    {
      #if UNITY_EDITOR
      Debug.LogError(GetMessage(messages));
      #endif
    }

    public static string GetMessage(params object[] messages)
    {
      var stringBuilder = new StringBuilder();
      foreach (var message in messages)
        stringBuilder.Append(message);
      return stringBuilder.ToString();
    }
  }
}
