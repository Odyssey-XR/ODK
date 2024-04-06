#nullable enable

namespace OdysseyXR.ODK.Core.Logging
{
  using System.Text;
  using UnityEngine;

  /// <summary>
  /// Custom logger implementation which changes
  /// behaviour based on build profile (editor, production etc.)
  /// </summary>
  public static class Logger
  {
    public static void Log(params string[] messages)
    {

      Debug.Log(GetMessage(messages));

    }

    public static void Error(params string[] messages)
    {

      Debug.LogError(GetMessage(messages));

    }

    public static string GetMessage(params string[] messages)
    {
      var stringBuilder = new StringBuilder();
      foreach (var message in messages)
        stringBuilder.Append(message);
      return stringBuilder.ToString();
    }
  }
}
